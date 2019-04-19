using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



    public class WorldManager : MonoBehaviour
    {
        
        [SerializeField]
        public class Count
        {
            public int minimum;
            public int maximum;

            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }

        [CreateAssetMenu(fileName = "NewBiome~", menuName = "Scriptable Object/New Biome")]
        public class Biome : ScriptableObject
        {
            [SerializeField]
            public List<Tile> titles = new List<Tile>();
            public List<Tile> DynamicTiles = new List<Tile>();
        }

        public List<Biome> Biomes = new List<Biome>();


        [SerializeField]
        public int seed;
        public int columns = 10;
        public int rows = 10;
        private Tilemap[] Boards;
        private Tilemap StaticBoard;
        private Tilemap DynamicBoard;

        FastNoise StaticNoiseGen = new FastNoise();
        FastNoise DynamicnoiseGen = new FastNoise();


        void BoardSetUp()
        {
            int biome = 0;
            int dynamicMapItem = 0;
            Boards = GetComponentsInChildren<Tilemap>();
            StaticBoard = Boards[0];
            DynamicBoard = Boards[1];
        
            for (int y = (rows / 2); y > -rows / 2; y--)
            {
                for (int x = (-columns / 2); x < columns / 2; x++)
                {

                    biome = getBiome(x, y);
                    Tile Statictile = Biomes[biome].titles[Random.Range(0, Biomes[biome].titles.Count)];
                    StaticBoard.SetTile(new Vector3Int(x, y, 0), Statictile);

                    if (Biomes[biome].DynamicTiles.Count != 0)
                    {
                        dynamicMapItem = GetDynamicMapItem(x, y, biome);
                        if(dynamicMapItem != 0)
                        {
                            Tile DynamicTile = Biomes[biome].DynamicTiles[Random.Range(0, Biomes[biome].DynamicTiles.Count)];
                            DynamicBoard.SetTile(new Vector3Int(x, y, 0), DynamicTile);  
                        }
                    }
                }
            }
        }

        public int GetDynamicMapItem(int x, int y, int biome)
        {
            int dynamicMapItem = 0;
            float noise = 0;
            noise = DynamicnoiseGen.GetNoise(x, y);
            float distance = 0.25f;
            float range = distance / (Biomes[biome].DynamicTiles.Count);
            float left = 0.75f;
            int count = 1;
            while ((left + range) <= 1.0)
            {
                if (noise > left && noise < (left + range))
                {
                 dynamicMapItem = count;
                }
                count++;
                left = left + range;
            }

            if(Random.Range(0,20) != 1)
            {
                dynamicMapItem = 0;
            }

            if(dynamicMapItem == 0)
            {
                if (Random.Range(0, 150) == 1)
                {
                    dynamicMapItem = Random.Range(1, Biomes[biome].DynamicTiles.Count);
                }
            }


        return dynamicMapItem;
        }
        
        public int getBiome(int x, int y)
        {
            int biome = 0;
            float noise = 0;
            float distance = 2f;
            float range = distance / (Biomes.Count + 1);
            noise = StaticNoiseGen.GetNoise(x, y);
            int count = 0;
            biome = 0;
            float left = -1.0f;
            while ((left + range) <= 1.0)
            {
                if (noise > left && noise < (left + range))
                {
                    biome = count;

                }
                count++;
                left = left + range;
            }

            return biome;
        }

        void HandleSeed()
        {
            if (seed == 0)
            {
                seed = Random.Range(1000, 9999);
            }
            Random.InitState(seed);
            StaticNoiseGen.SetSeed(seed);
            DynamicnoiseGen.SetSeed(seed*2);
        }

        void NoiseSetUp()
        {

            StaticNoiseGen.SetNoiseType(FastNoise.NoiseType.Cellular);
            StaticNoiseGen.SetFrequency((float)0.02);
            StaticNoiseGen.SetInterp(FastNoise.Interp.Quintic);
            StaticNoiseGen.SetFractalType(FastNoise.FractalType.Billow);
            StaticNoiseGen.SetFractalOctaves(5);
            StaticNoiseGen.SetFractalLacunarity((float)2.0);
            StaticNoiseGen.SetFractalGain((float)0.5);
            StaticNoiseGen.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Manhattan);
            StaticNoiseGen.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);

            DynamicnoiseGen.SetNoiseType(FastNoise.NoiseType.Cellular);
            DynamicnoiseGen.SetFrequency((float)0.02);
            DynamicnoiseGen.SetInterp(FastNoise.Interp.Quintic);
            DynamicnoiseGen.SetFractalType(FastNoise.FractalType.Billow);
            DynamicnoiseGen.SetFractalOctaves(5);
            DynamicnoiseGen.SetFractalLacunarity((float)2.0);
            DynamicnoiseGen.SetFractalGain((float)0.5);
            DynamicnoiseGen.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Manhattan);
            DynamicnoiseGen.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);


        }

        public void Awake()
        {
            HandleSeed();
            NoiseSetUp();
            BoardSetUp();

        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }


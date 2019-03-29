using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public List<GameObject> titles = new List<GameObject>();
    }

    public List<Biome> Biomes = new List<Biome>();

    struct biomeLocation
    {
        public int x, y, biome;
    }

    List<biomeLocation> biomeLocations = new List<biomeLocation>();

    [SerializeField]
    public int seed;
    public float BiomeSize;
    public int columns = 10;
    public int rows = 10;
    private Transform boardHolder;

    FastNoise noiseGen = new FastNoise();


    void BoardSetUp()
    {
        int biome = 0;
        float noise = 0;
        float distance = 2f;
        float range = distance / Biomes.Count;

        boardHolder = new GameObject("Board").transform;
        for (int x = (-columns/2); x < columns/2; x++)
        {
            for (int y = (-rows/2); y < rows/2; y++)
            {
               // biome = CheckBiome(x, y);
                noise = noiseGen.GetNoise(x, y);
                int count = 0;
                biome = 0;
                float left = -1.0f;
                while ((left + range) <= 1.0)
                {
                    if(noise > left && noise < (left + range))
                    {
                        biome = count;
                        
                    }
                    count++;
                    left = left + range;
                }
                GameObject toInstantiate = Biomes[biome].titles[Random.Range(0, Biomes[biome].titles.Count)];
                
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);

            }
        }
    }
    
    void HandleSeed()
    {
        if (seed == 0)
        {
            seed = Random.Range(1000, 9999);
        }
        Random.InitState(seed);
        noiseGen.SetSeed(seed);
    }

    int CheckBiome(int x, int y)
    {
        int biome= Random.Range(0, Biomes.Count);

        foreach (biomeLocation biomeloc in biomeLocations)
        {
            float Radius = Mathf.Sqrt( Mathf.Pow((x - biomeloc.x),2) + Mathf.Pow((y - biomeloc.y),2) );
            if(Radius <= BiomeSize)
            {
                biome = biomeloc.biome;
            }
        }

        return biome;
    }

    void GenerateBiomes()
    {
        if(BiomeSize == 0)
        {
            BiomeSize = 25;
        }
        float tileArea = Mathf.PI * (BiomeSize * BiomeSize);
        float numOfBiomes = ((columns * rows)) / (tileArea/4);

        for(int i = 0; i < numOfBiomes; i++)
        {
            biomeLocation newLocation = new biomeLocation();
            newLocation.x = Random.Range((-rows / 2), (rows/2));
            newLocation.y = Random.Range((-columns / 2), (columns/2));
            newLocation.biome = Random.Range(0, Biomes.Count);
            biomeLocations.Add(newLocation);
        }
    }

    void NoiseSetUp()
    {
        /*
        noiseGen.SetNoiseType(FastNoise.NoiseType.Cubic);
        noiseGen.SetFrequency((float)0.05);
        noiseGen.SetInterp(FastNoise.Interp.Quintic);
        noiseGen.SetFractalType(FastNoise.FractalType.FBM);
        noiseGen.SetFractalOctaves(5);
        noiseGen.SetFractalLacunarity((float)2.0);
        noiseGen.SetFractalGain((float)0.5);
        noiseGen.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
        noiseGen.SetCellularReturnType(FastNoise.CellularReturnType.NoiseLookup);
        
        */
    }

    public void Awake()
    {
        HandleSeed();
       // GenerateBiomes();
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

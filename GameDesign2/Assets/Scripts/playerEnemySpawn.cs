using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEnemySpawn : MonoBehaviour
{ 

    [SerializeField]
    public List<GameObject> enemylist = new List<GameObject>();
    [SerializeField]
    float radius=12;
    [SerializeField]
    int biome=4;
    [SerializeField]
    float SpawnTime=3;


    GameObject player;
    WorldManager world;
    float time=0;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.gameObject;
        world = GameObject.Find("WorldManager").GetComponent<WorldManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (world.getBiome((int)player.transform.position.x, (int)player.transform.position.y) == biome)
        {
            if (time == 0) time = Time.time + Random.Range(SpawnTime - 2, SpawnTime + 2);
            if (Time.time > time)
            {
                time = Time.time + Random.Range(SpawnTime - 2, SpawnTime + 2);

                Instantiate(enemylist[Random.Range(0, enemylist.Count)], RandomCircle(), Quaternion.identity);
            }
        }
        else {
            time = 0;
        }
    }

    public Vector3 RandomCircle() {
        // create random angle between 0 to 360 degrees
        float ang = Random.Range(0,360f);
        Vector3 pos;
        pos.x = player.transform.position.x + radius* Mathf.Sin(ang* Mathf.Deg2Rad);
        pos.y = player.transform.position.y + radius* Mathf.Cos(ang* Mathf.Deg2Rad);
        pos.z = player.transform.position.z;
        return pos;
    }
}

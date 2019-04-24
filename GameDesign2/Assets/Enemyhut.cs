using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyhut : CombatController, IDamageable
{

    private GameObject target = null;

    [SerializeField]
    public List<GameObject> enemylist = new List<GameObject>();
    [SerializeField]
    float HP = 50;

    [SerializeField]
    float triggerRadius = 2;
    [SerializeField]
    float resetRange = 15;
    [SerializeField]
    float spawnRadius = 2;
    [SerializeField]
    int minEnemy = 2;
    int number;

    public void TakeDamage(float damage)
    {
        HP = HP - damage;
        if (HP <= 0)
        {
            commitSuicide();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        CircleCollider2D targetCircle = this.GetComponent<CircleCollider2D>();
        targetCircle.radius = triggerRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance >= resetRange)
            {
                target = null;
            }
        }

    }

    IEnumerable spawn()
    {

        float distance = Vector3.Distance(gameObject.transform.position, Vector3.zero) / 50;
        number = (int)Random.Range(Mathf.Floor(distance), Mathf.Ceil(distance));
        number = Mathf.Max(minEnemy, number);
        for (int x = 0; x < number; x++)
        {
            Instantiate(enemylist[Random.Range(0, enemylist.Count)], RandomCircle(), Quaternion.identity);
        }
        Debug.Log("enemy hut spawn");
        return null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && target == null)
        {
            target = other.gameObject;
            StartCoroutine("spawn");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }


    private void commitSuicide()
    {
        Destroy(gameObject);
    }

    public Vector3 RandomCircle()
    {
        // create random angle between 0 to 360 degrees
        float ang = Random.Range(0, 360f);
        Vector3 pos;
        pos.x = gameObject.transform.position.x + spawnRadius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = gameObject.transform.position.y + spawnRadius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = gameObject.transform.position.z;
        return pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{

    private GameObject target = null;
    [SerializeField]
    float minRange = 1;
    [SerializeField]
    float maxRange = 15;
    [SerializeField]
    float speed = 9;
    [SerializeField]
    float attackCoolDownSec = 1f;
    [SerializeField]
    float spawnMoveCoolDown = 1f;
    float coolDown;

    float enemySpeed;

    Vector3 dirNormalized;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        CircleCollider2D targetCircle = this.GetComponent<CircleCollider2D>();
        targetCircle.radius = (float)maxRange;
        GetClosestPlayerInRadius();
        enemySpeed = speed;
        coolDown = Time.time + spawnMoveCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (Time.time > coolDown)
        {
            dirNormalized = (target.transform.position - transform.position).normalized;
            if (Vector3.Distance(target.transform.position, transform.position) <= minRange)
            {
                enemySpeed = 0;
                AttackTarget();
                enemySpeed = speed;

            }
            else
            {
                transform.position = transform.position + dirNormalized * speed * Time.deltaTime;
            }
        }

    }

    public void AttackTarget() {
        if (target.tag == "Player")
        {
            PlayerCombatController playerCombat = target.GetComponent<PlayerCombatController>();
            if (playerCombat != null)
            {
                this.GetComponent<EnemyCombatController>().AttackPlayer(playerCombat);
            }
            coolDown = Time.time + attackCoolDownSec;

        }
        else
        {
            Debug.LogError(this + " has a none player target!");
        }
        
    }

    void GetClosestPlayerInRadius()
    {
        float smallestDistance = Mathf.Pow(maxRange, 2); ;
        foreach (GameObject tr in GameObject.FindGameObjectsWithTag("Player"))
        {
            float distanceSqr = (transform.position - tr.transform.position).sqrMagnitude;
            float maxSqr = Mathf.Pow(maxRange,2);
            if (distanceSqr < maxSqr && distanceSqr < smallestDistance ) {
                target = tr;
                smallestDistance = Mathf.Abs(maxSqr - distanceSqr);
            }
                
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") target = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") target = null;
    }

}

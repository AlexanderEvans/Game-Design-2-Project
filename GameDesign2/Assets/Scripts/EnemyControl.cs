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
    float oldAngle=0;
    Animator myAnimator;

    Vector3 dirNormalized;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        myAnimator = gameObject.GetComponent<Animator>();
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
        setAnimationDir();
        if (Time.time > coolDown)
        {
            dirNormalized = (target.transform.position - transform.position).normalized;
            if (Vector3.Distance(target.transform.position, transform.position) <= minRange)
            {

                myAnimator.SetBool("up", false);
                myAnimator.SetBool("down", false);
                myAnimator.SetBool("left", false);
                myAnimator.SetBool("right", false);
                enemySpeed = 0;
                AttackTarget();
                enemySpeed = speed;

            }
            else
            {
                transform.position = transform.position + dirNormalized * speed * Time.deltaTime;
                setAnimationDir();

            }
        }

    }

    public void setAnimationDir() {
        float angle = Vector2.SignedAngle(Vector2.right, new Vector2(dirNormalized.x,dirNormalized.y));
        if (angle != oldAngle) {
            oldAngle = angle;
            if (angle < 45 && angle >= -45) {
                myAnimator.SetBool("up", false);
                myAnimator.SetBool("down", false);
                myAnimator.SetBool("left", false);
                myAnimator.SetBool("right", true);
            }
            else if (angle >= 45 && angle < 135)
            {
                myAnimator.SetBool("up", true);
                myAnimator.SetBool("down", false);
                myAnimator.SetBool("left", false);
                myAnimator.SetBool("right", false);

            }
            else if (angle >= 135 || angle < -135)
            {
                myAnimator.SetBool("up", false);
                myAnimator.SetBool("down", false);
                myAnimator.SetBool("left", true);
                myAnimator.SetBool("right", false);

            }
            else if (angle >= -135 && angle < -45)
            {
                myAnimator.SetBool("up", false);
                myAnimator.SetBool("down", true);
                myAnimator.SetBool("left", false);
                myAnimator.SetBool("right", false);
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

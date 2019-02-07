using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
    [SerializeField]
    float speedX = 10.0f;
    [SerializeField]
    float speedY = 10.0f;

    Rigidbody2D myRigidody2D;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        run();   
    }

    Vector2 runVelocity = Vector2.zero;
    void run()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");
        runVelocity.x = directionX * speedX;
        runVelocity.y = directionY * speedY;
        myRigidody2D.velocity = runVelocity;

        
        



        bool movingRight  =myRigidody2D.velocity.x > Mathf.Epsilon;
        bool movingLeft = myRigidody2D.velocity.x < -Mathf.Epsilon;
        bool movingUp = myRigidody2D.velocity.y > Mathf.Epsilon;
        bool movingDown = myRigidody2D.velocity.y < -Mathf.Epsilon;


        myAnimator.SetBool("PlayerRunLeft", movingLeft);
        myAnimator.SetBool("PlayerRunRight", movingRight);
        myAnimator.SetBool("PlayerRunUp", movingUp);
        myAnimator.SetBool("PlayerRunDown", movingDown);

    }
}

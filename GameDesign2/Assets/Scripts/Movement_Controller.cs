using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
    [SerializeField]
    float speedX = 10.0f;
    [SerializeField]
    float speedY = 10.0f;

    [SerializeField]
    Rigidbody2D myRigidody2D;
    [SerializeField]
    Animator myAnimator;

    //setup references prior to compile time
    private void Reset()
    {
        myRigidody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    private void OnValidate()
    {
        if(myRigidody2D==null)
        {
            myRigidody2D = GetComponent<Rigidbody2D>();
        }
        if (myAnimator == null)
        {
            myAnimator = GetComponent<Animator>();
        }
    }
    
    private void Awake()
    {
        //attempt lazy load
        if (myRigidody2D == null)
        {
            Debug.LogWarning("Warning: " + this + " does not allow 'rigidbody2d' to be null! Attempting correction...");
            myRigidody2D = GetComponent<Rigidbody2D>();
        }
        //attempt lazy load
        if (myAnimator == null)
        {
            Debug.LogWarning("Warning: " + this + " does not allow 'myAnimator' to be null! Attempting correction...");
            myAnimator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        Debug.Assert(myRigidody2D != null, "Error: " + this + " does not allow 'rigidbody2d' to be null!  Corection failed!");
        Debug.Assert(myAnimator != null, "Error: " + this + " does not allow 'myAnimator' to be null!  Corection failed!");
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

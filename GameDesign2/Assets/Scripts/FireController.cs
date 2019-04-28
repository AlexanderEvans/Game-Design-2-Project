using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player on fire!");
            PlayerCombatController Target = other.gameObject.GetComponent<PlayerCombatController>();
            if(Target != null)
            {
                Target.TakeDamage(5);
            }
            
        }
    }
}

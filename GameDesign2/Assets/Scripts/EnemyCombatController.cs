using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour, IDamageable
{
    [SerializeField]
    float HP = 10;
    public float hitPoints
    {
        get
        {
            return HP;
        }
        set
        {
            if(HP!=value)
            {
                HP = Mathf.Max(0, HP - value);
                if(HP==0)
                {
                    Debug.Log(this + " has passed away");
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(this + " says: \"ouch!\"");
        HP = HP - amount;
    }
}

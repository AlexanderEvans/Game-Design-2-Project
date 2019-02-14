using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : CombatController, IDamageable
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
            if(HP!=value && value>=0)
            {
                HP = Mathf.Max(0, value);
                if(HP==0)
                {
                    Debug.Log(this + " has passed away");
                    Destroy(gameObject);
                }
                else if(HP<0)
                {
                    Debug.LogError(" "+this+" has a negative HP value!");
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(this + " says: \"ouch!\"");
        hitPoints = hitPoints - amount;
    }
}

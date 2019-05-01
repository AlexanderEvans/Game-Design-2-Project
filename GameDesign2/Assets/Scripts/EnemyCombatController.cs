using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : CombatController, IDamageable
{
    [SerializeField]
    float HP = 10;
    [SerializeField]
    float Attack = 1;
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
                    commitSuicide();
                }
                else if(HP<0)
                {

                    Debug.LogError(this+" has a negative HP value!");
                }
            }
        }
    }

    public float AttackPoints {
        get { return Attack; }
        set
        {
            if (Attack != value && value > 0) {
                Attack = value;
            }
        }
    }

    private void commitSuicide()
    {
        int randNum = UnityEngine.Random.Range(0, int.MaxValue);
        List<string> strings = new List<string>();

        strings.Add(this + " has passed away");
        strings.Add(this + " longed for death");
        strings.Add(this + " has cast off their physical form");
        strings.Add(this + " left this world behind");
        strings.Add(this + " committed sepuku");
        strings.Add(this + " was game-ended");
        strings.Add(this + " uninstalled life");
        strings.Add(this + " removed themselves from existence");
        strings.Add(this + " committed unlife");
        strings.Add(this + " Alt+F4-ed life");
        strings.Add(this + " escaped from their mortal body");
        strings.Add(this + " is visiting the afterlife... forever");

        Debug.Log(strings[randNum % strings.Count]);
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(this + " says: \"ouch!\"");
        hitPoints = hitPoints - amount;
    }

    public void AttackPlayer(PlayerCombatController target) {
        target.TakeDamage(AttackPoints);
        Debug.Log(this + " says: \"attack!\"");
    }
}

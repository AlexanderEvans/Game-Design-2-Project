using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour, IDamageable
{
    float HP = 100;
    MonoBehaviour weapon;
    MonoBehaviour weaponMonoBehaviour
    {
        get
        {
            return weapon;
        }
        set
        {
            MonoBehaviour[] MBs = value.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour mb in MBs)
            {
                if(mb is IWeapon)
                {
                    weapon = value;
                }
                else
                {
                    Debug.LogWarning("Error: " + value + " does not implement IWeapon!");
                }
            }

            return;
        }
    }
    Vector2 attackDirection;
    Rigidbody2D rigidbody2d;

    public void TakeDamage(float damage)
    {
        Mathf.Max(0, HP - damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody2d.velocity != Vector2.zero)
            attackDirection = rigidbody2d.velocity;

        if (Input.GetButtonDown("Fire1"))
            ((IWeapon) weapon).Attack(transform, attackDirection);

    }
}
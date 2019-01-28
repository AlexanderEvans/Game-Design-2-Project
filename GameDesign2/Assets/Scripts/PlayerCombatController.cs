using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour, IDamageable
{
    float HP = 100;
    [System.Serializable]
    struct Weapon
    {
        public IWeapon weaponInterface;
        public Item component;
    }
    [SerializeField]
    Weapon weapon;

    private void Awake()
    {
        if(weapon.component is IWeapon)
        {
            weapon.weaponInterface = (IWeapon) weapon.component;
        }
        else
        {
            Debug.LogError("Error: "+weapon.component+" does not implement IWeapon!");
        }
    }

    public Item weaponGameObject
    {
        get
        {
            return weapon.component;
        }
        set
        {
            Item[] MBs = value.GetComponents<Item>();
            bool errorDetected = true;
            foreach(Item mb in MBs)
            {
                if(mb is IWeapon)
                {
                    weapon.weaponInterface = (IWeapon) mb;
                    weapon.component = mb;
                    errorDetected = false;
                }
            }
            if(errorDetected==true)
            {
                Debug.LogError("Error: " + value + " does not implement IWeapon!");
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
            weapon.weaponInterface.Attack(transform, attackDirection);
    }
}
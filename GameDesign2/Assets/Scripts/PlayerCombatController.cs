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
        //grab the reference to the physics component
        rigidbody2d = GetComponent<Rigidbody2D>();

        //use a "dirty hack" to dynamically link the interface on object load
        if (weapon.component is IWeapon)
        {
            weapon.weaponInterface = (IWeapon) weapon.component;//cast to an interface
            weapon.weaponInterface.SetTargetLayer(1<<9);//Targets Enemies
        }
        else
        {
            //This component should always be of a weapon type
            Debug.LogError("Error: "+weapon.component+" does not implement IWeapon!");
        }
    }

    /// <summary>
    /// This slot accepts an item that implements the IWeapon Interface
    /// </summary>
    public Item weaponGameObject
    {
        get
        {
            //get the weapon MonoBehaviour
            return weapon.component;
        }
        set
        {
            //get all the Item Monobehaviours on the weapon GameObject
            Item[] MBs = value.GetComponents<Item>();
            bool errorDetected = true;//create a flag
            foreach(Item mb in MBs)//loop over all the item components
            {
                if(mb is IWeapon)//if the MonoBehaviour implements the Iweapon interface
                {
                    weapon.weaponInterface = (IWeapon) mb;//assign the interface
                    weapon.weaponInterface.SetTargetLayer(1<<9);//Targets Enemies
                    weapon.component = mb;//cache the MonoBehaviour Component
                    errorDetected = false;//set the flag
                }
            }
            if(errorDetected==true)//If the Item didn't impliment IWeapon, the flag never gets cleared
            {
                Debug.LogError("Error: " + value + " does not implement IWeapon!");
            }

            return;
        }
    }

    //chache the attack direction to reduce garbage collection
    Vector2 attackDirection = Vector2.down;
    Rigidbody2D rigidbody2d;

    /// <summary>
    /// Damages the player
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        Mathf.Max(0, HP - damage);
    }
    

    // Update is called once per frame
    void Update()
    {
        //update the attatck direction
        if (rigidbody2d.velocity != Vector2.zero)
            attackDirection = rigidbody2d.velocity;
        else
            attackDirection = Vector2.down;

        //trigger an attack with the held weapon
        if (Input.GetButtonDown("Fire1"))
            weapon.weaponInterface.Attack(transform, attackDirection);
    }
}
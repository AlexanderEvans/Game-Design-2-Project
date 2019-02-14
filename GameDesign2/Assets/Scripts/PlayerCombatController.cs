using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatController : CombatController, IDamageable
{
    float HP = 100;

    #region weapon
    [System.Serializable]
    struct Weapon
    {
        public IWeapon weaponInterface;
        public Item component;
    }
    [SerializeField]
    Weapon weapon;
    /// <summary>
    /// use a "dirty hack" to dynamically link the interface on object load
    /// </summary>
    void initializeWeapon()
    {
        //use a "dirty hack" to dynamically link the interface on object load
        Item[] items = GetComponents<Item>();
        foreach (Item item in items)
        {
            bool errorFlag = true;
            if (weapon.component is IWeapon)
            {
                weapon.weaponInterface = (IWeapon)weapon.component;//cast to an interface
                weapon.weaponInterface.SetTargetLayer(1 << 9);//Targets Enemies on layer 9
                errorFlag = false;
            }

            if (errorFlag == true)
            {
                //This component should always be of a weapon type
                Debug.LogError("Error: " + weapon.component + " does not implement IWeapon!");
            }
        }
    }
    #endregion

    private void Awake()
    {
        //grab the reference to the physics component
        rigidbody2d = GetComponent<Rigidbody2D>();

        initializeWeapon();
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
            if(value is IWeapon)//if the MonoBehaviour implements the Iweapon interface
            {
                weapon.weaponInterface = (IWeapon) value;//assign the interface
                weapon.weaponInterface.SetTargetLayer(1<<9);//Targets Enemies
                weapon.component = value;
            }
            else//If the Item didn't impliment IWeapon, the flag never gets cleared
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
        HP = Mathf.Max(0, HP - damage);
    }
    

    // Update is called once per frame
    void Update()
    {
        //update the attatck direction
        if (rigidbody2d.velocity != Vector2.zero)
            attackDirection = rigidbody2d.velocity;

        //trigger an attack with the held weapon
        if (Input.GetButtonDown("Fire1") && weapon.component != null)
        {
            weapon.weaponInterface.Attack(this, attackDirection);
        }
    }
}
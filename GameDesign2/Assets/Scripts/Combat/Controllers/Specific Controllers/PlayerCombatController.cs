using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatController : CombatController, IDamageable
{
    public float HP = 100;
    AudioSource audiosource;
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
        if (weapon.component is IWeapon != true)
        {
            Debug.LogWarning("Error: " + weapon.component + " does not implement IWeapon!");
            weapon.component = null;
        }
        else
        {
            weapon.weaponInterface = (IWeapon)weapon.component;//cast to an interface
            weapon.weaponInterface.SetTargetLayer(1 << 9);//Targets Enemies on layer 9
        }
    }
    #endregion

    private void OnValidate()
    {
        
        if(weapon.component !=null)
            initializeWeapon();


        if(rigidbody2d==null)
        {
            //grab the reference to the physics component
            rigidbody2d = GetComponent<Rigidbody2D>();
        }
    }

    private void Reset()
    {
        //grab the reference to the physics component
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
        //attempt lazy load
        if (rigidbody2d==null)
        {
            Debug.LogWarning("Warning: " + this + " does not allow 'rigidbody2d' to be null! Attempting correction...");
            rigidbody2d = GetComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        Debug.Assert(rigidbody2d != null, "Error: "+this+ " does not allow 'rigidbody2d' to be null!  Corection failed!");
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
            Debug.Assert(value is IWeapon, "Error: " + value + " does not implement IWeapon!");

            weapon.weaponInterface = (IWeapon) value;//assign the interface
            weapon.weaponInterface.SetTargetLayer(1<<9);//Targets Enemies
            weapon.component = value;

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
        audiosource.Play();
    }
    

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //float offset = 1;
        //Vector2 spawnOffset = (mouseWorldPosition - (Vector2)transform.position).normalized * offset;
        //Vector3 spawnLocation = transform.position + (Vector3)spawnOffset;
        //spawnLocation.z = transform.position.z;


        attackDirection = mouseWorldPosition - (Vector2)transform.position;
        //float rotation = Mathf.Rad2Deg * (Mathf.Atan(attackDirection.y / attackDirection.x));
        //rotation += -90;
        //if (attackDirection.x < 0)
        //{
        //    rotation += 180;
        //}

        //update the attatck direction
        //if (rigidbody2d.velocity != Vector2.zero)
        //    attackDirection = rigidbody2d.velocity;

        //trigger an attack with the held weapon
        if (Input.GetButtonDown("Fire1") && weapon.component != null)
        {
            weapon.weaponInterface.Attack(this, attackDirection);
        }
    }
}
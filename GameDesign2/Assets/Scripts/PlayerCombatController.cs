using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour
{
    IWeapon weapon = null;
    Vector2 attackDirection;
    Rigidbody2D rigidbody2d;

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
            weapon.Attack(transform, attackDirection);
    }
}
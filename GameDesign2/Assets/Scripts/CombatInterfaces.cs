using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class WeaponProperties
{
    public float weaponReach=1;
    public float hitDuration=1;
    public float damage=1;
    public float weaponCooldown=1;
    public float currentCooldown=0;
}

interface IWeapon
{
    void Attack(Transform parentTransform, Vector2 attackDirection);
}

interface IDamageable
{
    void TakeDamage(float damage);
}



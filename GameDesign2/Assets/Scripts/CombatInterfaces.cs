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
    public int targetLayer = 9;
}

interface IWeapon
{
    void SetTargetLayer(int targetLayer);
    void Attack(Transform parentTransform, Vector2 attackDirection);
}

interface IDamageable
{
    void TakeDamage(float damage);
}



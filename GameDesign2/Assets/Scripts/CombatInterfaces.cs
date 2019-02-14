using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class WeaponProperties
{
    public bool constantReach = false;
    public float weaponReach=1;
    public float hitDuration=1;
    public float damage=1;
    public float weaponCooldown=1;
    [HideInInspector]
    public int targetLayer = 1<<9;
}

/// <summary>
/// indicates that this object can attack a target physics layer
/// </summary>
interface IWeapon
{
    /// <summary>
    /// Set the target physics layer to make attacks at.
    /// </summary>
    /// <param name="targetLayer"></param>
    void SetTargetLayer(int targetLayer);
    /// <summary>
    /// Make an attack in the direction of attackDirection from an origin point of parentTransform
    /// </summary>
    /// <param name="combatController"></param>
    /// <param name="attackDirection"></param>
    void Attack(CombatController combatController, Vector2 attackDirection);
}

/// <summary>
/// indicates that this object can take damage
/// </summary>
interface IDamageable
{
    /// <summary>
    /// Damages the entity
    /// </summary>
    /// <param name="damage"></param>
    void TakeDamage(float damage);
}



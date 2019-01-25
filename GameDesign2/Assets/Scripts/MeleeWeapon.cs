using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MeleeWeapon : Item, IWeapon
{
    public WeaponProperties weaponProperties = new WeaponProperties();

    void Update()
    {
        weaponProperties.currentCooldown = Mathf.Max(weaponProperties.currentCooldown - Time.deltaTime, 0);
    }

    public void Attack(Transform parentTransform, Vector2 attackDirection)
    {
        if(weaponProperties.currentCooldown<=0)
        {
            StartCoroutine(MeleeAttack(parentTransform, attackDirection));
            weaponProperties.currentCooldown = weaponProperties.weaponCooldown;
        }


        return;
    }

    IEnumerator MeleeAttack(Transform parentTransform, Vector2 attackDirection)
    {
        List<GameObject> objectsHit = new List<GameObject>();
        float endTime = Time.time + weaponProperties.hitDuration;
        while (Time.time < endTime)
        {
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(parentTransform.position, attackDirection, Mathf.Lerp(0, weaponProperties.attackDistance, endTime - Time.time));
            foreach(RaycastHit2D hit in hits)
            {
                MonoBehaviour[] list = hit.collider.gameObject.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour mb in list)
                {
                    if (mb is IDamageable)
                    {
                        IDamageable damageable = (IDamageable)mb;
                        if (objectsHit.Contains(hit.collider.gameObject) != true)
                        {
                            damageable.TakeDamage(weaponProperties.damage);
                            objectsHit.Add(hit.collider.gameObject);
                        }
                    }
                }

            }
            yield return null;
        }
        objectsHit.Clear();
    }
}

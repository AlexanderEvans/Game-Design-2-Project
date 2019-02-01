using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MeleeWeapon : Item, IWeapon
{
    public WeaponProperties weaponProperties = new WeaponProperties();
    [SerializeField]
    Material weaponFlashMaterial;
    static GameObject lineContainer = new GameObject("Line Container");

    private void Awake()
    {
        if (lineContainer.GetComponent<LineManager>() == null)
            lineContainer.AddComponent<LineManager>();
    }

    void Update()
    {
        weaponProperties.currentCooldown = Mathf.Max(weaponProperties.currentCooldown - Time.deltaTime, 0);
    }

    public void Attack(Transform parentTransform, Vector2 attackDirection)
    {
        if(weaponProperties.currentCooldown<=0)
        {
            bool needIndex = true;
            int newIndex = 0;
            while(needIndex)
            {
                if (linerendererIndex.Contains(newIndex))
                    newIndex++;
                else
                {
                    if (newIndex >= lineRenderers.Count)
                        lineRenderers.Add(new LineRenderer());
                    linerendererIndex.Add(newIndex);
                }
            }

            StartCoroutine(MeleeAttack(parentTransform, attackDirection, newIndex));
            weaponProperties.currentCooldown = weaponProperties.weaponCooldown;
        }


        return;
    }

    IEnumerator MeleeAttack(Transform parentTransform, Vector2 attackDirection, int lineRendererIndex)
    {
        List<GameObject> objectsHit = new List<GameObject>();
        float endTime = Time.time + weaponProperties.hitDuration;
        attackDirection.Normalize();
        float angle = Vector2.SignedAngle(Vector2.right, attackDirection);

        lineRenderers[lineRendererIndex].enabled = true;
        lineRenderers[lineRendererIndex].endColor = Color.white;
        lineRenderers[lineRendererIndex].startColor = Color.white;
        lineRenderers[lineRendererIndex].startWidth = 1f;
        lineRenderers[lineRendererIndex].endWidth = 1f;

        lineRenderers[lineRendererIndex].material = weaponFlashMaterial;

        Vector3 target;
        Vector3[] linePoints = { Vector3.zero, Vector3.zero};
        while (Time.time < endTime)
        {
            RaycastHit2D[] hits;

            float castDistance = Mathf.Lerp(0, weaponProperties.attackDistance, endTime - Time.time);

            hits = Physics2D.RaycastAll(parentTransform.position, attackDirection, castDistance);
            target.x = (attackDirection * castDistance).x;
            target.y = (attackDirection * castDistance).y;
            target.z = 0;

            linePoints[0] = parentTransform.position;
            linePoints[1] = parentTransform.position + target;

            lineRenderers[lineRendererIndex].SetPositions(linePoints);

            foreach (RaycastHit2D hit in hits)
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
        lineRenderers[lineRendererIndex].enabled = false;
        linerendererIndex.Remove(lineRendererIndex);
    }
}

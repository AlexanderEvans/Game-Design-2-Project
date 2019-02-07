using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MeleeWeapon : Item, IWeapon
{
    public WeaponProperties weaponProperties = new WeaponProperties();
    
    public void SetTargetLayer(int targetLayer)
    {
        weaponProperties.targetLayer = targetLayer;
    }

    [SerializeField]
    Material weaponFlashMaterial;
    static GameObject lineContainer;
    static LineManager lineManager;

    private void Awake()
    {
        if (lineContainer == null)
        {
            lineContainer = new GameObject("Line Container");
            lineManager = lineContainer.AddComponent<LineManager>();
        }
    }

    void Update()
    {
        weaponProperties.currentCooldown = Mathf.Max(weaponProperties.currentCooldown - Time.deltaTime, 0);
    }

    public void Attack(Transform parentTransform, Vector2 attackDirection)
    {
        if(weaponProperties.currentCooldown<=0)
        {

            StartCoroutine(MeleeAttack(attackDirection));
            weaponProperties.currentCooldown = weaponProperties.weaponCooldown;
        }


        return;
    }

    IEnumerator MeleeAttack(Vector2 attackDirection)
    {
        List<GameObject> objectsHit = new List<GameObject>();
        float endTime = Time.time + weaponProperties.hitDuration;
        attackDirection.Normalize();
        float angle = Vector2.SignedAngle(Vector2.right, attackDirection);

        LineRenderer lineRenderer = lineManager.getLine();

        lineRenderer.enabled = true;
        lineRenderer.endColor = Color.white;
        lineRenderer.startColor = Color.white;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
                  
        lineRenderer.material = weaponFlashMaterial;

        Vector3 target;
        Vector3[] linePoints = {Vector3.zero, Vector3.zero};
        while (Time.time < endTime)
        {
            RaycastHit2D[] hits;

            float castDistance = Mathf.Lerp(0, weaponProperties.weaponReach, 1-((endTime - Time.time)/weaponProperties.hitDuration));

            hits = Physics2D.RaycastAll(transform.position, attackDirection, castDistance);
            target.x = (attackDirection * castDistance).x;
            target.y = (attackDirection * castDistance).y;
            target.z = 0;

            linePoints[0] = transform.position;
            linePoints[1] = transform.position + target;

            lineRenderer.SetPositions(linePoints);

            foreach (RaycastHit2D hit in hits)
            {
                //fallback code
                //if (hit.collider.gameObject.layer = LayerMask.NameToLayer("Enemy"))
                if(hit.collider.gameObject.layer==weaponProperties.targetLayer)
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

            }
            yield return null;
        }
        objectsHit.Clear();
        lineManager.removeLine(lineRenderer);
    }
}

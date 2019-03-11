using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeleeWeaponPooler))]
class MeleeWeapon : Item, IWeapon
{
    private void OnEnable()
    {
        lastAttackTime = 0 - weaponProperties.weaponCooldown;
    }

    //holds the weapon stat block
    public WeaponProperties weaponProperties = new WeaponProperties();

    [SerializeField]
    private Material weaponFlashMaterial = null;
    [SerializeField]
    private ObjectPool objectPool = null;
    [SerializeField]
    private LineRendererPooler lineRendererPooler = null;

    private void Start()
    {
        Debug.Assert(objectPool != null, "Error: objectPool is null in " + this);
        Debug.Assert(lineRendererPooler != null, "Error: lineRendererPooler is null in " + this);
        Debug.Assert(weaponFlashMaterial != null, "Error: weaponFlashMaterial is null in " + this);
    }

    
    float lastAttackTime;

    /// <summary>
    /// Set the target physics layer to make attacks at.
    /// </summary>
    /// <param name="targetLayer"></param>
    public void SetTargetLayer(int targetLayer)
    {
        weaponProperties.targetLayer = targetLayer;
    }

    /// <summary>
    /// Make an attack in the direction of attackDirection from an origin point of parentTransform
    /// </summary>
    /// <param name="parentTransform"></param>
    /// <param name="attackDirection"></param>
    public void Attack(CombatController combatController, Vector2 attackDirection)
    {
        if(gameObject.activeInHierarchy==true)
        {
            //do error checking
            Debug.Assert(objectPool != null, "Error: " + this + " needs acess to an objectPool Scriptable Object, but is null");

            if (Time.time - lastAttackTime > weaponProperties.weaponCooldown)//make sure the cooldown has expired
            {
                lastAttackTime = Time.time;
                Transform parentTransform = combatController.transform;
                combatController.StartCoroutine(MeleeAttack(parentTransform, attackDirection));//start a new attatck coroutine(time sharing parraleleism)
            }
        }
    }

    /// <summary>
    /// Manages a single attack over it's lifetime
    /// </summary>
    /// <param name="attackDirection"></param>
    /// <returns></returns>
    IEnumerator MeleeAttack(Transform origin, Vector2 attackDirection)
    {
        //store a list of objects we already hit with this attack to prevent multi-attacking with a single swing
        List<GameObject> objectsHit = new List<GameObject>();
        //calculate when the attack should end
        float endTime = Time.time + weaponProperties.hitDuration;
        //gige the direction vector a magnitude on 1
        attackDirection.Normalize();
        //determine the angle we are swinging at.
        float angle = Vector2.SignedAngle(Vector2.right, attackDirection);

        //grab a lineRenderer from the pool
        LineRendererPooler lineRendererPooler = (LineRendererPooler) objectPool.PopObject(this.lineRendererPooler);
        LineRenderer lineRenderer = lineRendererPooler.GetComponent<LineRenderer>();

        //set the lines default values
        lineRenderer.enabled = true;
        lineRenderer.endColor = Color.white;
        lineRenderer.startColor = Color.white;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        
        //set the line material to render
        lineRenderer.material = weaponFlashMaterial;

        Vector3 target;//this is what we will attack toward.  It is defined here to avoid producing garbage on the heap
        Vector3[] linePoints = {Vector3.zero, Vector3.zero};
        while (Time.time < endTime)//attack until the duration for the hit has elapsed
        {
            RaycastHit2D[] hits;//store everything we hit this frame


            #region positional data
            float castDistance;
            if (weaponProperties.constantReach!=true)
            {
                //determine how far out we have swung the weapon on this frame
                castDistance = Mathf.Lerp(0, weaponProperties.weaponReach, 1 - ((endTime - Time.time) / weaponProperties.hitDuration));
            }
            else
            {
                castDistance = weaponProperties.weaponReach;
            }
            #endregion
            
            #region render
            //determine the target offset vector
            target.x = (attackDirection * castDistance).x;
            target.y = (attackDirection * castDistance).y;
            target.z = 0;

            //set the points in the array that will form the line's verticies in order
            linePoints[0] = origin.position;
            linePoints[1] = origin.position + target;

            //load the array of points
            lineRenderer.SetPositions(linePoints);
            #endregion

            #region physics
            //set the layer that we want to hit
            LayerMask layerMask = weaponProperties.targetLayer;

            //raycast to everythinging the layer mask
            hits = Physics2D.RaycastAll(origin.position, attackDirection, castDistance, layerMask);

            //examine the results of the clollision events from the raycast
            foreach (RaycastHit2D hit in hits)
            {
                if (objectsHit.Contains(hit.collider.gameObject) != true)//check if the weapon already hit this object
                {
                    //get a list of all the gomponents attatched to the incoming collider on the colision event
                    MonoBehaviour[] list = hit.collider.gameObject.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour mb in list)//itterate over the components
                    {
                        if (mb is IDamageable)//check if the MonoBehaviour implements IDamageable
                        {
                            IDamageable damageable = (IDamageable)mb;//if it does, cast it to the interface
                                                                     //deal damage
                            damageable.TakeDamage(weaponProperties.damage);
                            //mark that we have already hit this object with this attack
                            objectsHit.Add(hit.collider.gameObject);

                        }
                    }
                }
            }
            //proceed 1 frame
            yield return null;
        }
        #endregion

        objectPool.PushObject(lineRendererPooler);//release line renderer back to the pool
        objectsHit.Clear();//release the memory of objects hit
    }
}


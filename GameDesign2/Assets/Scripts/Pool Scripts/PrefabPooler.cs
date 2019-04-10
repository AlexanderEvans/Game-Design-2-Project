using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;

[DisallowMultipleComponent]
public abstract class PrefabPooler : MonoBehaviour, IPoolableObject
{
    [SerializeField]
    bool isPrefabbedPooler = false;

    private void Awake()
    {
        if (isPrefabbedPooler == true)
            Debug.LogWarning("Is not prefab: " + this);
        isPrefabbedPooler = false;
    }

    public void OnValidate()
    {
        isPrefabbedPooler = (PrefabStageUtility.GetPrefabStage(gameObject) != null);
    }
    public void Reset()
    {
        isPrefabbedPooler = (PrefabStageUtility.GetPrefabStage(gameObject) != null);
    }

    public virtual IPoolableObject CreateInstance()
    {
        //Debug.Assert(isPrefabbedPooler, "Error, not a prefab + "+gameObject+this);
        GameObject newGameObject = Instantiate(gameObject);
        return newGameObject.GetComponent<PrefabPooler>();
    }

    public virtual void ReleaseSelf()
    {
        Destroy(gameObject);
    }

}

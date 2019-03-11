using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;

[DisallowMultipleComponent]
public abstract class PrefabPooler : MonoBehaviour, IPoolableObject
{
    public virtual IPoolableObject CreateInstance(string objectProperties = "")
    {
        Debug.Assert(PrefabStageUtility.GetPrefabStage(gameObject)!=null, "Error, not a prefab");
        GameObject newGameObject = Instantiate(gameObject);
        return newGameObject.GetComponent<PrefabPooler>();
    }

    public virtual void Activate(string objectProperties = "")
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public virtual object getObjRef()
    {
        return this;
    }

    public virtual void ReleaseSelf()
    {
        Destroy(gameObject);
    }

}

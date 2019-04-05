using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;

[DisallowMultipleComponent]
public abstract class PrefabPooler : MonoBehaviour, IPoolableObject
{
    public virtual IPoolableObject CreateInstance()
    {
        Debug.Assert(PrefabStageUtility.GetPrefabStage(gameObject)!=null, "Error, not a prefab");
        GameObject newGameObject = Instantiate(gameObject);
        return newGameObject.GetComponent<PrefabPooler>();
    }

    public virtual void ReleaseSelf()
    {
        Destroy(gameObject);
    }

}

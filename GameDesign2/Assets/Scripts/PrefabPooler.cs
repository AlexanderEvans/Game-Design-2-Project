using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrefabPooler : MonoBehaviour, IPoolableObject
{
    public IPoolableObject CreateInstance()
    {
        Debug.Assert(gameObject.scene.name == null, "Error, not a prefab");
        GameObject newGameObject = Instantiate(gameObject);
        return newGameObject.GetComponent<PrefabPooler>();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public object GetValue()
    {
        return this;
    }

    public void ReleaseSelf()
    {
        Destroy(gameObject);
    }

}

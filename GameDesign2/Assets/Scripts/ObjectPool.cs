using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IPoolableObject
{
    void Activate();
    void Deactivate();
    System.Type GetType();
    object GetValue();
    IPoolableObject CreateInstance();
}

[CreateAssetMenu(fileName = "New Object Pool Singleton", menuName = "Scriptable Object/Object Pool Singleton")]
public class ObjectPool : ScriptableObject
{
    static Dictionary<System.Type, List<IPoolableObject>> pools = new Dictionary<System.Type, List<IPoolableObject>>();

    static void PushObject(object objectToPool)
    {
        IPoolableObject poolableObject = (IPoolableObject) objectToPool;
        poolableObject.Deactivate();

        System.Type type = objectToPool.GetType();
        List<IPoolableObject> poolableObjects;
        if (pools.TryGetValue(type, out poolableObjects))
        {
            poolableObjects.Add(poolableObject);
        }
        else
        {
            poolableObjects = new List<IPoolableObject>();
            poolableObjects.Add(poolableObject);
            pools.Add(type, poolableObjects);
        }
    }

    static object PopObject(object inPrefab) 
    {
        Debug.Assert((inPrefab is IPoolableObject) == true, "Error: " + inPrefab + " does now implement IPoolableObject");

        object rtnVal = null;
        System.Type type = inPrefab.GetType();
        List<IPoolableObject> poolableObjects;
        
        if (pools.TryGetValue(type, out poolableObjects))
        {
            if (poolableObjects.Count == 0)
            {
                IPoolableObject poolableObj = (IPoolableObject) inPrefab;
                rtnVal = poolableObj.CreateInstance();
            }
            else
            {
                IPoolableObject poolableObj = poolableObjects[poolableObjects.Count];
                poolableObjects.Remove(poolableObj);
                poolableObj.Activate();
                rtnVal = poolableObj.GetValue();
            }
        }
        else
        {
            IPoolableObject poolableObj = (IPoolableObject) inPrefab;
            rtnVal = poolableObj.CreateInstance().GetValue();
        }
        return rtnVal;
    }
}

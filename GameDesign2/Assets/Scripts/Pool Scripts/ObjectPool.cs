using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPoolableObject
{
    void Activate(string objectProperties = "");
    void Deactivate();
    System.Type GetType();
    object getObjRef();
    IPoolableObject CreateInstance(string objectProperties = "");
    void ReleaseSelf();
}

[CreateAssetMenu(fileName = "New Object Pool Singleton", menuName = "Scriptable Object/Object Pool Singleton")]
public class ObjectPool : ScriptableObject
{
    Dictionary<System.Type, List<IPoolableObject>> pools = new Dictionary<System.Type, List<IPoolableObject>>();

    public void ClearAllPools()
    {
        foreach (List<IPoolableObject> pool in pools.Values)
        {
            if(pool!=null)
            {
                foreach(IPoolableObject poolableObject in pool)
                {
                    poolableObject.ReleaseSelf();
                }
                pool.Clear();
            }
        }
        pools.Clear();
    }
    public void ClearPool(System.Type type)
    {
        List<IPoolableObject> pool;
        pools.TryGetValue(type, out pool);
        if(pool!=null)
        {
            foreach(IPoolableObject poolableObject in pool)
            {
                poolableObject.ReleaseSelf();
            }
            pool.Clear();
        }
    }

    public void PushObject(object objectToPool) 
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

    public object PopObject(object inPrefab, string objectProperties = "") 
    {
        Debug.Assert((inPrefab is IPoolableObject) == true, "Error: " + inPrefab + " does not implement IPoolableObject");

        object rtnVal = null;
        System.Type type = inPrefab.GetType();
        List<IPoolableObject> poolableObjects;
        
        if (pools.TryGetValue(type, out poolableObjects))
        {
            if (poolableObjects.Count == 0)
            {
                IPoolableObject poolableObj = (IPoolableObject) inPrefab;
                rtnVal = poolableObj.CreateInstance(objectProperties);
            }
            else
            {
                IPoolableObject poolableObj = poolableObjects[poolableObjects.Count-1];
                poolableObjects.Remove(poolableObj);
                poolableObj.Activate(objectProperties);
                rtnVal = poolableObj.getObjRef();
            }
        }
        else
        {
            IPoolableObject poolableObj = (IPoolableObject) inPrefab;
            rtnVal = poolableObj.CreateInstance(objectProperties).getObjRef();
        }
        return rtnVal;
    }
}

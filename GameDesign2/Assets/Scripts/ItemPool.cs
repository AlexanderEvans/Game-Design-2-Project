using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IPoolableObject
{
    void Activate();
    void Deactivate();
    System.Type GetType();
    IPoolableObject CreateInstance();
}

public static class ItemPool
{
    static Dictionary<System.Type, List<IPoolableObject>> pools = new Dictionary<System.Type, List<IPoolableObject>>();

    static void pushObject<T>(T objectToPool) where T : IPoolableObject
    {
        objectToPool.Deactivate();

        System.Type type = objectToPool.GetType();
        List<IPoolableObject> poolableObjects;
        if (pools.TryGetValue(type, out poolableObjects))
        {
            poolableObjects.Add(objectToPool);
        }
        else
        {
            poolableObjects = new List<IPoolableObject>();
            poolableObjects.Add(objectToPool);
            pools.Add(type, poolableObjects);
        }
    }

    static T popObject<T>(T prefab) where T : class, IPoolableObject
    {
        T rtnVal = null;
        System.Type type = prefab.GetType();

        List<IPoolableObject> poolableObjects;
        if (pools.TryGetValue(type, out poolableObjects))
        {
            if(poolableObjects.Count==0)
            {
                IPoolableObject poolableObj = prefab;
                rtnVal = (T) poolableObj.CreateInstance();
            }
            else
            {
                IPoolableObject poolableObj = poolableObjects[poolableObjects.Count];
                T tempObj = (T)poolableObj;
                poolableObjects.Remove(tempObj);
                poolableObj.Activate();
                rtnVal = tempObj;
            }
        }
        else
        {
            IPoolableObject poolableObj = prefab;
            rtnVal = (T) poolableObj.CreateInstance();
        }
        return rtnVal;
    }
}
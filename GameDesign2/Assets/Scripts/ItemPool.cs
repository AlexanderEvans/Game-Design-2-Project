using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IPoolableObject
{
    void Activate();
    void Deactivate();
    System.Type GetType();
    Item GetValue();
    IPoolableObject CreateInstance();
}

public static class ItemPool
{
    static Dictionary<System.Type, List<IPoolableObject>> pools = new Dictionary<System.Type, List<IPoolableObject>>();

    static void pushObject<T>(T objectToPool) where T : IPoolableObject
    {
        IPoolableObject poolableObject = objectToPool;
        objectToPool.Deactivate();

        System.Type type = objectToPool.GetType();
        List<IPoolableObject> items;
        if (pools.TryGetValue(type, out items))
        {
            items.Add((IPoolableObject) objectToPool.GetValue());
        }
        else
        {
            items = new List<IPoolableObject>();
            items.Add( (IPoolableObject) objectToPool.GetValue());
            pools.Add(type, items);
        }
    }

    static T popObject<T>(T obj) where T : class, IPoolableObject
    {
        T rtnVal = null;
        System.Type type = obj.GetType();

        List<IPoolableObject> objects;
        if (pools.TryGetValue(type, out objects))
        {
            if(objects.Count==0)
            {
                IPoolableObject poolableObjects = obj;
                rtnVal = (T) poolableObjects.CreateInstance();
            }
            else
            {
                IPoolableObject poolableObj = objects[objects.Count];
                T tempObj = (T)poolableObj;
                objects.Remove(tempObj);
                poolableObj.Activate();
                rtnVal = tempObj;
            }
        }
        else
        {
            IPoolableObject poolableItem = obj;
            rtnVal = (T) poolableItem.CreateInstance();
        }
        return rtnVal;
    }
}
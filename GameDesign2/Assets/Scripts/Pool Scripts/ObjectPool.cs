using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPoolableObject
{
    IPoolableObject CreateInstance();
    void ReleaseSelf();
}


[CreateAssetMenu(fileName = "New Object Pool Singleton", menuName = "Scriptable Object/Object Pool Singleton")]
public class ObjectPool : ScriptableObject
{
    Dictionary<System.Type, List<object>> pools = new Dictionary<System.Type, List<object>>();

    public void ClearAllPools()
    {
        foreach (List<object> pool in pools.Values)
        {
            if(pool!=null)
            {
                foreach(object poolableObject in pool)
                {
                    if(poolableObject.GetType() is IPoolableObject)
                    {
                        ((IPoolableObject)poolableObject).ReleaseSelf();
                    }
                }
                pool.Clear();
            }
        }
        pools.Clear();
    }
    public void ClearPool(System.Type type)
    {
        List<object> pool;
        pools.TryGetValue(type, out pool);
        if(pool!=null)
        {
            foreach(object poolableObject in pool)
            {
                if (poolableObject.GetType() is IPoolableObject)
                {
                    ((IPoolableObject)poolableObject).ReleaseSelf();
                }
            }
            pool.Clear();
        }
    }

    public void PushObject<T>(T objectToPool) where T : class
    {

        System.Type type = objectToPool.GetType();
        List<object> poolableObjects;
        if (pools.TryGetValue(type, out poolableObjects))
        {
            poolableObjects.Add(objectToPool);
        }
        else
        {
            poolableObjects = new List<object>();
            poolableObjects.Add(objectToPool);
            pools.Add(type, poolableObjects);
        }
    }

    public T PopObject<T>(T classTemplate) where T : class, IPoolableObject
    {
        Debug.Assert(classTemplate != null, "Error:  classTemplate can not be null!");
        T rtnVal=null;
        System.Type type = typeof(T);
        List<object> poolableObjects;

        if (pools.TryGetValue(type, out poolableObjects))
        {
            if (poolableObjects.Count == 0)
            {
                IPoolableObject poolableObj = classTemplate;
                rtnVal = (T) (poolableObj.CreateInstance());
            }
            else
            {
                object poolableObj = poolableObjects[poolableObjects.Count - 1];
                poolableObjects.Remove(poolableObj);
                rtnVal = (T)poolableObj;
            }
        }
        else
        {
            IPoolableObject poolableObj = classTemplate;
            rtnVal = (T)(poolableObj.CreateInstance());
        }
        return rtnVal;
    }

    public T PopObject<T>() where T : class, new()
    {
        T rtnVal = null;
        System.Type type = typeof(T);
        List<object> poolableObjects;

        if (pools.TryGetValue(type, out poolableObjects))
        {
            if (poolableObjects.Count == 0)
            {
                rtnVal = new T();
                Debug.Log("Count == " + poolableObjects.Count);
            }
            else
            {
                Debug.Log("Count == " + poolableObjects.Count);
                object poolableObj = poolableObjects[poolableObjects.Count - 1];
                poolableObjects.Remove(poolableObj);
                rtnVal = (T)poolableObj;
            }
        }
        else
        {
            Debug.Log("Failed get pool");
            rtnVal = new T();
        }
        return rtnVal;
    }
}
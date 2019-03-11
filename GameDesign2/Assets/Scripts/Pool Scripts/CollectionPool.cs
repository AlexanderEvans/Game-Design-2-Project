using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPool : ScriptableObject
{
    Dictionary<System.Type, object> stacks;
    
    public void pushBack<T>(T col)
    {
        object genStack;
        if (stacks.TryGetValue(col.GetType(), out genStack))
        {
            Stack<T> tempStack =(Stack<T>) genStack;
            tempStack.Push(col);
        }
        else
        {
            Stack<T> tempStack;
            tempStack = new Stack<T>();
            tempStack.Push(col);
            stacks.Add(col.GetType(), tempStack);
        }
    }

    public T popBack<T>() where T : new()
    {
        object genStack;
        if (stacks.TryGetValue(typeof(T), out genStack))
        { 
            Stack<T> tempStack = (Stack<T>)genStack;

            if (tempStack.Count > 0)
                return tempStack.Pop();
            else
                return new T();
        }
        else
        {
            return new T();
        }
    }
}

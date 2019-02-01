using System;
using Unity;
using UnityEngine;
using System.Collections.Generic;

class LineManager : MonoBehaviour
{

    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    List<LineRenderer> pool = new List<LineRenderer>();

    public void removeLineStruct(LineRenderer temp)
    {
        lineRenderers.Remove(temp);
        pool.Add(temp);
        temp.enabled = false;
    }

    public LineRenderer getLineStruct()
    {
        LineRenderer temp;
        if (pool.Count==0)
        {
            temp = gameObject.AddComponent<LineRenderer>();
            lineRenderers.Add(temp);
        }
        else
        {
            temp = pool[0];
            pool.Remove(pool[0]);
            temp.enabled = true;
            lineRenderers.Add(temp);
        }
        return temp;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LinePoolable : MonoBehaviour
{
    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    List<LineRenderer> pool = new List<LineRenderer>();
    
    

    public void removeLine(LineRenderer temp)
    {
        pool.Add(temp);
        lineRenderers.Remove(temp);
        temp.enabled = false;
    }

    public LineRenderer getLine()
    {
        LineRenderer temp;
        if (pool.Count == 0)
        {
            GameObject tempObject = new GameObject("Line " + (lineRenderers.Count + 1));
            tempObject.transform.parent = gameObject.transform;

            temp = tempObject.AddComponent<LineRenderer>();
            lineRenderers.Add(temp);
        }
        else
        {
            temp = pool[0];
            pool.Remove(pool[0]);
            lineRenderers.Add(temp);
            temp.enabled = true;
        }
        return temp;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LinePoolable : MonoBehaviour
{
    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    List<LineRenderer> pool = new List<LineRenderer>();

    [SerializeField]
    LinePoolSingleton linePoolSingleton;

    private void Reset()
    {
        linePoolSingleton = AssetManagement.FindAssetByType<LinePoolSingleton>();
    }

    private void OnEnable()
    {
        if(linePoolSingleton.linePoolable!=this)
        {
            linePoolSingleton.linePoolable = this;
        }
    }
    private void OnDisable()
    {
        if (linePoolSingleton.linePoolable == this)
        {
            linePoolSingleton.linePoolable = null;
        }
    }

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

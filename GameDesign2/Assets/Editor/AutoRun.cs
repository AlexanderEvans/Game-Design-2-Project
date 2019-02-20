using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public class Autorun
{
    public static event EventHandler ReoptimizeEvent;
    static Autorun()
    {
        EditorApplication.update += RunOnce;
    }

    static void RunOnce()
    {
        if(ReoptimizeEvent!=null)
            ReoptimizeEvent.Invoke(null, EventArgs.Empty);
        EditorApplication.update -= RunOnce;
    }
}
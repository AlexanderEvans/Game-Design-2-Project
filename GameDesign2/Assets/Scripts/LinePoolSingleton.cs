using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Line Pool Singleton", menuName = "Custom Actions/Create ScriptableObject/Line Pool Singleton")]
public class LinePoolSingleton : ScriptableObject
{
    public LinePoolable linePoolable;
}

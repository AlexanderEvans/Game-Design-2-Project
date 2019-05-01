using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceConnection : MonoBehaviour
{
    public ResourceBuildings input;
    public ResourceBuildings output;
    [SerializeField]
    ObjectPool objectPool; 
    LineRenderer lineRenderer;
}

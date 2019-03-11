using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScriptInterfaces : MonoBehaviour
{
}


public interface IGameObjectAddedToSceneHierarchy
{
    bool IsAdded();
    void AddToHierarchy();
}
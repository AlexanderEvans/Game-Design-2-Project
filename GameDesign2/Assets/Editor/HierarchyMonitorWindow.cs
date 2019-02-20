using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class HierarchyMonitorWindow : EditorWindow
{
    [MenuItem("Window/Hierarchy Monitor")]
    static void createWindow()
    {
        GetWindow<HierarchyMonitorWindow>();
    }
    void OnHierarchyChange()
    {
        var addedObjects = Resources.FindObjectsOfTypeAll<Object>().Where(x => x is IGameObjectAddedToSceneHierarchy);

        foreach (var item in addedObjects)
        {
            IGameObjectAddedToSceneHierarchy gameObjectAddedToHierarchy = (IGameObjectAddedToSceneHierarchy)item;

            //if (item.isAdded == 0) early setup

            if (gameObjectAddedToHierarchy.IsAdded() == false)
            {
                gameObjectAddedToHierarchy.AddToHierarchy();
            }

        }
    }
}

using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using System;

[InitializeOnLoad]
public class Autorun
{
    static Autorun()
    {
        EditorApplication.update += RunOnce;
    }

    static void RunOnce()
    {
        Item.SyncGUIDS();
        PrefabUtility.prefabInstanceUpdated = new PrefabUtility.PrefabInstanceUpdated(PrefabInstanceUpdated);
        EditorApplication.update -= RunOnce;
    }

    public static void PrefabInstanceUpdated(UnityEngine.GameObject gameObject)
    {
        Item item = gameObject.GetComponent<Item>();
        if(item!=null)
        {
            item.CheckIfIsPrefab();
            if (item.IsPrefab==false)
            {
                Debug.Log("instance callback: " + item);
                SerializedObject serializedObject = new SerializedObject(item);
                SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
                PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
            }
            else
            {
                Debug.Log("prefab callback: " + item);
                //item.RegeneratePrefabGuid();
                Item iRef = Item.GetPrefabComponent(item);
                if (item.GUID == "")
                    item.RegeneratePrefabGuid();
            }
        }
    }
}
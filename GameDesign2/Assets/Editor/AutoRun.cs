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
        Item.GetLargestGUIDAndRebuildPrefabsDictionary();
        PrefabUtility.prefabInstanceUpdated = new PrefabUtility.PrefabInstanceUpdated(PrefabInstanceUpdated);
        EditorApplication.update -= RunOnce;
    }

    public static void PrefabInstanceUpdated(UnityEngine.GameObject gameObject)
    {
        Item item = gameObject.GetComponent<Item>();
        if(item!=null)
        {
            if(PrefabStageUtility.GetPrefabStage(gameObject)==null)
            {
                //Debug.Log("instance callback");
                SerializedObject serializedObject = new SerializedObject(item);
                SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
                PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour
{
    Sprite icon;

    int GUID;
    static int GUIDCount;

    [MenuItem("Custom Actions/Prefab Management/Optimize GUIDS")]
    static void OptimizeGUIDS()
    {
        GUIDCount = 0;
        List<Item> items = AssetManagement.FindAssetsByType<Item>();
        List<Item> prefabs = new List<Item>();
        List<Item> instances = new List<Item>();
        foreach (Item item in items)
        {
            if (item.gameObject.scene.name == null)
                prefabs.Add(item);
            else
                instances.Add(item);
        }

        foreach(Item prefab in prefabs)
        {
            prefab.GUID = GUIDCount;
            GUIDCount++;
        }

        foreach(Item instance in instances)
        {
            SerializedObject serializedObject = new SerializedObject(instance);

            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("GUID");

            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }

    private void OnValidate()
    {
        SerializedObject serializedObject = new SerializedObject(this);

        SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("GUID");

        PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

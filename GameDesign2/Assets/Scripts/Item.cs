using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Item : MonoBehaviour
{
    Sprite icon;

    int GUID;
    static int GUIDCount;
    static List<Item> prefabs;

    static Item getPrefab(Item instance)
    {
        foreach (Item prefab in prefabs.Where((prefab) => prefab.GUID == instance.GUID))
            return prefab;
        return null;
    }

    static public void updatePrefabsList()
    {
        prefabs.Clear();
        List<Item> items = AssetManagement.FindAssetsByType<Item>();
        foreach (Item item in items.Where((item) => item.gameObject.scene.name == null))
        {
            prefabs.Add(item);
        }
    }


    private void Reset()
    {
        if (gameObject.scene.name == null)
        {
            GUID = GUIDCount;
            GUIDCount++;
        }
        Item[] items = FindObjectsOfType<Item>();
        foreach (Item item in items.Where((item) => item.gameObject.scene.name != null))
        {
            SerializedObject serializedObject = new SerializedObject(item);

            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("GUID");

            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }

    [MenuItem("Custom Actions/Prefab Management/Optimize GUIDS")]
    static void OptimizeGUIDS()
    {
        GUIDCount = 0;
        List<Item> prefabs = AssetManagement.FindAssetsByType<Item>();
        Item[] instances = FindObjectsOfType<Item>();
        foreach (Item prefab in prefabs.Where( (prefab) => prefab.gameObject.scene.name == null))
        {
            prefab.GUID = GUIDCount;
            GUIDCount++;
        }
        

        foreach(Item instance in instances.Where( (instance) => instance.gameObject.scene.name!=null))
        {
            SerializedObject serializedObject = new SerializedObject(instance);

            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("GUID");

            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }

    private void OnValidate()
    {
        if (this.gameObject.scene.name != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("GUID");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
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

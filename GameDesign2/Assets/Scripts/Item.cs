using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[System.Serializable]
public class Item : MonoBehaviour
{
    [System.Serializable]
    public class ItemsSlot
    {
        public int count=0;
        public int itemGUID=-1;
        //public string itemProperties;
    }
    [SerializeField]
    Sprite icon;

    [SerializeField]
    public int GUID { get; private set; }
    static int GUIDCount;
    static List<Item> prefabs = new List<Item>();
    

    static Item GetPrefab(Item instance)
    {
        foreach (Item prefab in prefabs.Where((prefab) => prefab.GUID == instance.GUID))
            return prefab;
        return null;
    }

    static Item GetPrefab(int itemGUID)
    {
        foreach (Item prefab in prefabs.Where((prefab) => prefab.GUID == itemGUID))
            return prefab;
        return null;
    }

    static public void UpdatePrefabsList()
    {
        prefabs.Clear();
        List<Item> items = AssetManagement.FindAssetsByComponent<Item>();

        foreach (Item item in items.Where((item) => item.gameObject.scene.name == null))
        {
            prefabs.Add(item);
        }
    }

    static public void updatePrefabsList(List<Item> items)
    {
        prefabs.Clear();
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
            prefabs.Add(this);
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
        List<Item> prefabs = AssetManagement.FindAssetsByComponent<Item>();
        Item[] instances = (Item[]) Resources.FindObjectsOfTypeAll(typeof(Item));
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

        updatePrefabsList(prefabs);
    }

    private void OnValidate()
    {
        if (gameObject.scene.name != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("GUID");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }
}

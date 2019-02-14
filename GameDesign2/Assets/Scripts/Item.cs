using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[System.Serializable]
public class Item : MonoBehaviour
{
    Sprite icon;

    [SerializeField]
    [HideInInspector]
    int GUID;
    static int GUIDCount;
    static List<Item> prefabs = new List<Item>();

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

        updatePrefabsList(prefabs);
    }

    private void OnValidate()
    {
        if (gameObject.scene.name != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            Debug.Log("s1 "+ serializedObject);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("GUID");
            Debug.Log("s2 "+ serializedPropertyGUID.intValue);
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
            Debug.Log("s3");
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

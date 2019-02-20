using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

[System.Serializable]
[DisallowMultipleComponent]
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
    public Sprite icon;

    [SerializeField]
    bool isPrefab = true;
    public bool IsPrefab
    {
        get
        {
            return isPrefab;
        }
        private set
        {
            isPrefab = value;
        }
    }
    [SerializeField]
    private int guid=-1;
    public int GUID
    {
        get
        {
            return guid;
        }
        private set
        {
            if (guid == -1)
                guid = value;
            else if (IsPrefab != true)
                guid = value;
            else
                Debug.LogWarning("Warning: Reseting GUID on " + this + " is not allowed!");
        }
    }

    public static int GUIDCount = 0;
    static Dictionary<int, Item> prefabs = new Dictionary<int, Item>();


    private void OnValidate()
    {
        //Debug.Log("Validating..."+this);
        if (CheckIfIsPrefab() != true)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }

    private void Reset()
    {
        if (CheckIfIsPrefab())
        {
            GUID = GUIDCount;
            GUIDCount++;
            prefabs.Add(GUID, this);
        }

        List<Item> items = GetAllItemsInAllScenes();
        foreach (Item item in items.Where((item) => item.IsPrefab != true))
        {
            SerializedObject serializedObject = new SerializedObject(item);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }

    private bool CheckIfIsPrefab()
    {
        PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
        //Debug.Log("IsPrefabSource: " + gameObject + " : " + prefabStage+", "+ (prefabStage != null));
        return IsPrefab = (prefabStage != null);
    }

    /// <summary>
    /// Warning!!! SLOW AF!!!  EDITOR ONLY!!!
    /// </summary>
    /// <returns></returns>
    static List<Item> GetAllItemsInAllScenes()
    {
        List<Scene> allScenes = new List<Scene>();
        for(int i = 0; i<SceneManager.sceneCount; i++)
        {
            allScenes.Add(SceneManager.GetSceneAt(i));
        }
        
        List<Transform> transforms = new List<Transform>();
        foreach (Scene scene in allScenes)
        {
            GameObject[] GOs = scene.GetRootGameObjects();
            foreach (GameObject gameObject in GOs)
            {
                transforms.AddRange(gameObject.GetComponentsInChildren<Transform>());
            }
        }
        List<Item> items = new List<Item>();
        foreach (Transform transform in transforms)
        {
            Item temp = transform.GetComponent<Item>();
            if (temp!= null && temp.CheckIfIsPrefab() != true)
                items.Add(temp);
        }
        return items;
    }


    /// <summary>
    /// Warning!!! SLOW AF!!!  EDITOR ONLY!!!
    /// </summary>
    /// <returns></returns>
    static List<Item> GetAllItemPrefabsInProjectFolder()
    {
        //List<Item>;
        List<Item> items = AssetManagement.FindAssetsByComponent<Item>();
        Debug.Log("Items: "+items.Count);
        List<Item> itemList = new List<Item>();
        foreach(Item item in items)
        {
            itemList.Add(item);
        }
        return itemList;
    }

    public virtual void InitializeItemFromPrefab(Item PrefabItemComponent)
    {
        Debug.Assert(IsPrefab != true, "Error: Can not InitializeItemFromPrefab() because "+this+ " is a PrefabItemComponent!");
        guid = PrefabItemComponent.GUID;
        icon = PrefabItemComponent.icon;
        PrefabUtility.SavePrefabAsset(gameObject);
    }

    private static int GetLargestGUID()
    {
        List<Item> items = GetAllItemPrefabsInProjectFolder();

        int LargestGUID = 0;
        foreach (Item item in items.Where((item) => item.IsPrefab == true))
        {
            if (item.GUID > LargestGUID)
                LargestGUID = item.GUID;
        }
        return LargestGUID;
    }

    public static Item GetPrefab(Item instance)
    {
        prefabs.TryGetValue(instance.GUID, out Item temp);
        return temp;
    }

    public static Item GetPrefab(int itemGUID)
    {
        prefabs.TryGetValue(itemGUID, out Item temp);
        return temp;
    }

    
    /// <summary>
    /// Initialise Item GUID System
    /// </summary>
    [MenuItem("Custom Actions/Prefab Management/Initialise Item GUID Sys./GetLargestGUIDAndRebuildPrefabsDictionary")]
    public static int GetLargestGUIDAndRebuildPrefabsDictionary()
    {
        List<Item> items = GetAllItemPrefabsInProjectFolder();

        int LargestGUID = 0;
        prefabs.Clear();
        foreach (Item item in items.Where((item) => item.IsPrefab == true))
        {
            if (item.GUID >= LargestGUID)
                LargestGUID = item.GUID+1;
            prefabs.Add(item.GUID, item);
        }
        return LargestGUID;
    }

    /// <summary>
    /// Warning, this might break Item Prefab Initialization!  Use with caution!!!
    /// </summary>
    [MenuItem("Custom Actions/Prefab Management/Set GUIDCount=0")]
    static void SetGUIDCountToZero()
    {
        GUIDCount = 0;
    }

    /// <summary>
    /// Get the largest GUID and add one to restart the counter
    /// </summary>
    [MenuItem("Custom Actions/Prefab Management/Set GUIDCount to current Largest GUID")]
    static void SetGUIDCountToCurrentLargestGUID()
    {
        GUIDCount = GetLargestGUID()+1;
    }


    [MenuItem("Custom Actions/Prefab Management/Sync GUIDS")]
    static void SyncGUIDS()
    {
        List<Item> instances = GetAllItemsInAllScenes();

        foreach (Item instance in instances.Where((instance) => instance.IsPrefab!=true))
        {
            SerializedObject serializedObject = new SerializedObject(instance);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
        UpdatePrefabsDictionary();
    }


    [MenuItem("Custom Actions/Prefab Management/Display prefab GUIDS")]
    static void DisplayGUIDS()
    {
        List<Item> prefabs = GetAllItemPrefabsInProjectFolder();
        foreach (Item prefab in prefabs)
        {
            Debug.Log(prefab.GUID);
        }
    }
    /// <summary>
    /// Warning, will break between save/load between runs!
    /// </summary>
    [MenuItem("Custom Actions/Prefab Management/Reset GUIDS")]
    static void ResetGUIDS()
    {
        GUIDCount = 0;
        List<Item> prefabs = GetAllItemPrefabsInProjectFolder();
        List<Item> instances = GetAllItemsInAllScenes();
        Debug.Log("Prefabs: " + prefabs.Count);
        foreach (Item prefab in prefabs)
        {
            prefab.guid = GUIDCount;
            GUIDCount++;
            PrefabUtility.SavePrefabAsset(prefab.gameObject);
        }
        Debug.Log("Instances: " + instances.Count);
        foreach (Item instance in instances)
        {
            SerializedObject serializedObject = new SerializedObject(instance);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }

        UpdatePrefabsDictionary();
    }

    static public void UpdatePrefabsDictionary()
    {
        prefabs.Clear();
        List<Item> items = GetAllItemPrefabsInProjectFolder();

        foreach (Item item in items)
        {
            Debug.Log("Adding Item: " + item + "\nAdding GUID:" + item.GUID);
            prefabs.Add(item.GUID, item);
        }
    }
}

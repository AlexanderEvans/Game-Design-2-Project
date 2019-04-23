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
public class Item : PrefabPooler, ISaveable
{
    public Sprite icon;
    [SerializeField]
    private int maxStackSize = 99;
    public int MaxStackSize
    {
        get
        {
            return maxStackSize;
        }
        private set
        {
            maxStackSize = value;
        }
    }

    public bool NeedsLoad { get; private set; } = false;

    [SerializeField]
    bool isPrefab = false;
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
    private string guid = "";
    public string GUID
    {
        get
        {
            return guid;
        }
        private set
        {
            if (guid == "")
                guid = value;
            else if (IsPrefab != true)
                guid = value;
            else
                Debug.LogWarning("Warning: Reseting GUID on " + this + " is not allowed!");
        }
    }

    [SerializeField]
    PrefabGuidMap prefabGuidMap;
    static bool prefabsSet = false;
   

    public new void OnValidate()
    {
        base.OnValidate();
        CheckIfIsPrefab();
        //Debug.Log("Validating..."+this);
        if(IsPrefab == true)
        {
            if (GUID == "")
            {
                GUID = name + " + " + System.DateTime.Now + " + " + System.DateTime.UtcNow.Ticks;
                prefabGuidMap.Add(GUID, this);
            }
        }
        else
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }

    public new void Reset()
    {
        base.Reset();
        if (CheckIfIsPrefab()==true)
        {
            if(GUID == "")
            {
                GUID = name + " + " + System.DateTime.Now + " + " + System.DateTime.UtcNow.Ticks;
                prefabGuidMap.Add(GUID, this);
            }
        }

        List<Item> items = GetAllItemsInAllScenes();
        foreach (Item item in items.Where((item) => item.IsPrefab != true))
        {
            SerializedObject serializedObject = new SerializedObject(item);
            SerializedProperty serializedPropertyGUID = serializedObject.FindProperty("guid");
            PrefabUtility.RevertPropertyOverride(serializedPropertyGUID, InteractionMode.AutomatedAction);
        }
    }

    public bool CheckIfIsPrefab()
    {
        PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
        //Debug.Log("IsPrefabSource: " + gameObject + " : " + prefabStage+", "+ (prefabStage != null));
        IsPrefab = (prefabStage != null);
        return IsPrefab;
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
        List<Item> itemList = new List<Item>();
        foreach(Item item in items)
        {
            itemList.Add(item);
        }
        return itemList;
    }

    public virtual void InitializeItemFromPrefab(Item PrefabItemComponent)
    {
        Debug.Assert(IsPrefab != true, "Error: Can not InitializeItemFromPrefab() because " + this + " is a PrefabItemComponent!");
        guid = PrefabItemComponent.GUID;
        icon = PrefabItemComponent.icon;
        PrefabUtility.SavePrefabAsset(gameObject);
    }
    public void RegeneratePrefabGuid()
    {
        GUID = name + " + " + System.DateTime.Now + " + " + System.DateTime.UtcNow.Ticks;
        prefabGuidMap.Add(GUID, this);
    }

    static PrefabGuidMap staticPrefabGuidMap=null;

    private void Awake()
    {
        if(staticPrefabGuidMap==null || staticPrefabGuidMap.getCount()==0)
            staticPrefabGuidMap = prefabGuidMap;
    }

    public static Item GetPrefabComponent(Item instance)
    {
        if (staticPrefabGuidMap == null)
        {
            staticPrefabGuidMap = AssetManagement.FindAssetByType<PrefabGuidMap>();
        }
        staticPrefabGuidMap.TryGetValue(instance.GUID, out Item temp);
        return temp;
    }

    public static void dumpAll()
    {
        if (staticPrefabGuidMap == null)
        {
            staticPrefabGuidMap = AssetManagement.FindAssetByType<PrefabGuidMap>();
        }
        staticPrefabGuidMap.DumpData();
    }

    public static Item GetPrefabComponent(string itemGUID)
    {
        if (staticPrefabGuidMap == null)
        {
            staticPrefabGuidMap = AssetManagement.FindAssetByType<PrefabGuidMap>();
        }
        staticPrefabGuidMap.TryGetValue(itemGUID, out Item temp);
        return temp;
    }
    
    

    [MenuItem("Custom Actions/Prefab Management/Sync GUIDS")]
    public static void SyncGUIDS()
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

    [MenuItem("Custom Actions/Prefab Management/Regenerate prefab GUIDS")]
    static void RegenGUIDS()
    {
        List<Item> prefabs = GetAllItemPrefabsInProjectFolder();
        foreach (Item prefab in prefabs)
        {
            Debug.Log(prefab.GUID);
        }
    }

    [MenuItem("Custom Actions/Prefab Management/Manual update prefab GUIDS dictionary")]
    static public void UpdatePrefabsDictionary()
    {
        if(staticPrefabGuidMap==null)
        {
            staticPrefabGuidMap = AssetManagement.FindAssetByType<PrefabGuidMap>();
        }

        staticPrefabGuidMap.Clear();
        List<Item> items = GetAllItemPrefabsInProjectFolder();

        foreach (Item item in items)
        {
            //Debug.Log("Adding Item: " + item + "\nAdding GUID:" + item.GUID);
            if(staticPrefabGuidMap.ContainsKey(item.GUID))
            {
                Item old;
                staticPrefabGuidMap.TryGetValue(item.GUID, out old);
                //Debug.Log("Error: " + item.GUID + "already exists!\nOld: " + old + "\nNew: " + item);
            }
            else
            {
                staticPrefabGuidMap.Add(item.GUID, item, false);
                //Debug.Log("Adding: " + item.GUID + " : " + item);
            }
        }
        staticPrefabGuidMap.Populate();
    }

    public virtual  string SavePropertiesToJSONString()
    {
        NeedsLoad = true;
        return "";
    }

    public virtual void LoadPropertiesFromJSONString(string data)
    {
        NeedsLoad = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "New PrefabGuidMap", menuName = "Scriptable Object/PrefabGuidMap Singleton")]
public class PrefabGuidMap : ScriptableObject
{
    [SerializeField]
    ObjectPool objectPool;

    [Serializable]
    class GuidPrefabPair
    {
        [SerializeField]
        string GUID_p;
        public string GUID
        {
            get
            {
                return GUID_p;
            }
            private set
            {
                GUID_p = value;
            }
        }
        [SerializeField]
        Item item_p;
        public Item item
        {
            get
            {
                return item_p;
            }
            private set
            {
                item_p = value;
            }
        }

        public GuidPrefabPair()
        {

        }
        public GuidPrefabPair(string guid, Item item)
        {
            GUID_p = guid;
            item_p = item;
        }
        public void Init(string guid, Item item)
        {
            GUID_p = guid;
            item_p = item;
        }


    }

    [SerializeField]
    List<GuidPrefabPair> guidPrefabPairs = new List<GuidPrefabPair>();
    
    public int getCount()
    {
        int count = 0;
        if (prefabsSet == true)
            count = prefabs.Count;
        else
            count = guidPrefabPairs.Count;

        return count;
    }

    Dictionary<string, Item> prefabs = new Dictionary<string, Item>();
    bool prefabsSet = false;

    public void DumpData()
    {
        Debug.Log("Dumping...");

        foreach (GuidPrefabPair guidPrefabPair in guidPrefabPairs)
        {
            if (guidPrefabPair != null)
                Debug.Log("Element: " + guidPrefabPair.GUID + " : " + guidPrefabPair.item);
        }
        Debug.Log("Dumping pt2...");
        foreach (KeyValuePair<string, Item> guidPrefabPair in prefabs)
        {
            Debug.Log("Element: " + guidPrefabPair.Key + " : " + guidPrefabPair.Value);
        }
        Debug.Log("Done Dumping!");
        Debug.Log("Dumped: "+prefabs.Count);
    }

    void Populate()
    {
        Debug.Log("Populating...");
        prefabs.Clear();
        foreach (GuidPrefabPair guidPrefabPair in guidPrefabPairs)
        {
            if (guidPrefabPair != null)
                prefabs.Add(guidPrefabPair.GUID, guidPrefabPair.item);
        }
    }

    private void Awake()
    {
        if (prefabsSet != true)
        {
            Populate();
            prefabsSet = true;
        }
    }

    private void OnEnable()
    {
        if (prefabsSet != true)
        {
            Populate();
            prefabsSet = true;
        }
    }

    public void TryGetValue(string GUID, out Item temp)
    {
        if (prefabsSet != true)
        {
            Populate();
            prefabsSet = true;
        }
        prefabs.TryGetValue(GUID, out temp);
    }

    public void Add(string guid, Item item)
    {
        GuidPrefabPair temp = objectPool.PopObject<GuidPrefabPair>();
        temp.Init(guid, item);

        guidPrefabPairs.Add(temp);
        Populate();
    }

    public void Clear()
    {
        foreach(GuidPrefabPair guidPrefabPair in guidPrefabPairs)
        {
            objectPool.PushObject(guidPrefabPair);
        }
        guidPrefabPairs.Clear();
        prefabs.Clear();
    }

    public bool ContainsKey(string gUID)
    {
        if (prefabsSet != true)
        {
            Populate();
            prefabsSet = true;
        }
        return prefabs.ContainsKey(gUID);
    }
}

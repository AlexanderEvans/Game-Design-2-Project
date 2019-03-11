using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData : ScriptableObject
{
    ObjectPool objectPool = null;
    CollectionPool collectionPool = null;

    int itemsPerPage = 12;
    int maxPages = 5;

    public int MaxPages
    {
        get
        {
            return maxPages;
        }
        set
        {
            if(maxPages<value)

            MaxPages = value;
        }
    }
    

    public struct GenericItemSlot
    {
        public bool isLive;

        public string guid;
        public int itemCount;
        
        public List<string> itemData;
        public Item item;//use to check if we are working with live item!!
    }

    [System.Serializable]
    public struct StaticItemSlot
    {
        public string guid;
        public string[] itemData;
        public int positionInList;
    }

    [System.Serializable]
    public struct LiveItemSaveSlot
    {
        public string guid;
        public string itemData;
        public int positionInList;
    }

    [SerializeField]
    public List<GenericItemSlot> allItems = new List<GenericItemSlot>();

    [SerializeField]
    public List<StaticItemSlot> staticGUIDItems = new List<StaticItemSlot>();

    public List<Item> dynamicRuntimeItems = new List<Item>();
    [SerializeField]
    public List<LiveItemSaveSlot> staticRuntimeItems = new List<LiveItemSaveSlot>();


    private void Awake()
    {
        GenericItemSlot temp;
        temp.guid = "";
        temp.item = null;
        temp.itemCount = 0;
        temp.isLive = false;

        for (int i = 0; i<(itemsPerPage*MaxPages); i++)
        {
            temp.itemData = collectionPool.popBack<List<string>>();
            allItems.Add(temp);
        }
    }

    void clearItems()
    {
        GenericItemSlot temp;

        for (int i = 0; i < (itemsPerPage * MaxPages); i++)
        {
            if(allItems[i].itemData!=null)
            {
                collectionPool.pushBack(allItems[i].itemData);
            }
        }

        allItems.Clear();

        temp.guid = "";
        temp.item = null;
        temp.itemCount = 0;
        temp.isLive = false;
        for (int i = 0; i < (itemsPerPage * MaxPages); i++)
        {
            temp.itemData = collectionPool.popBack<List<string>>();
            allItems.Add(temp);
        }
    }

    void saveInventoryData()
    {
        staticRuntimeItems.Clear();

        LiveItemSaveSlot temp;

        foreach (GenericItemSlot genericItemSlot in allItems)
        {
            if(genericItemSlot.isLive)
            {

            }
            else
            {
                temp.guid = item.GUID;
                temp.itemData = item.saveItemPropertiesToString();
                temp.positionInList =

                staticRuntimeItems.Add(temp);
            }

        }

        //write to json
    }

    void loadInventoryData()
    {
        dynamicRuntimeItems.Clear();

        //read from json

        GenericItemSlot genericItemSlot;

        foreach(LiveItemSaveSlot liveItemSaveSlot in staticRuntimeItems)
        {
            Item prefabComponent = Item.GetPrefabComponent(liveItemSaveSlot.guid);
            ItemPooler itemPooler;
            
            itemPooler = (ItemPooler)objectPool.PopObject(prefabComponent.GetComponent<ItemPooler>(), liveItemSaveSlot.itemData);
            Item item = itemPooler.GetComponent<Item>();
            
            //item.loadItemPropertiesFromString(liveItemSaveSlot.itemData);
            dynamicRuntimeItems.Add(item);//add to runtime


            genericItemSlot.guid = item.GUID;
            genericItemSlot.item = item;
            genericItemSlot.isLive = true;
            genericItemSlot.itemCount = 1;
            genericItemSlot.itemData = collectionPool.popBack<List<string>>();
            if (allItems[liveItemSaveSlot.positionInList].itemData != null)
                collectionPool.pushBack(allItems[liveItemSaveSlot.positionInList].itemData);//cache list... probably not thread safe

            //overwrite
            allItems[liveItemSaveSlot.positionInList] = genericItemSlot;
        }

        foreach(StaticItemSlot staticItemSlot in staticGUIDItems)
        {
            genericItemSlot.isLive = false;
            genericItemSlot.item = null;
            genericItemSlot.guid = staticItemSlot.guid;
            genericItemSlot.itemCount = allItems[staticItemSlot.positionInList].itemCount + 1;
            genericItemSlot.itemData = collectionPool.popBack<List<string>>();
            genericItemSlot.itemData.AddRange(staticItemSlot.itemData);
            if(allItems[staticItemSlot.positionInList].itemData!=null)
                collectionPool.pushBack(allItems[staticItemSlot.positionInList].itemData);//cache list... probably not thread safe

            //overwrite
            allItems[staticItemSlot.positionInList] = genericItemSlot; ;

        }
    }
}

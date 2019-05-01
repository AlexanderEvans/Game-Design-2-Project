using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemStack : PrefabPooler, ISaveable
{
    [SerializeField]
    ObjectPool objectPool;

    [System.Serializable]
    struct TempStack
    {
        public bool isDynamic;
        public string GUID;

        public List<string> properties;
        public List<Item> items;

        public void SetStack(ItemStack other)
        {
            if (other != null)
            {
                isDynamic = other.isDynamic;
                GUID = other.GUID;
                if(other.properties != null && other.properties.Count!=0)
                {
                    if (properties == null)
                        properties = new List<string>();
                    
                    foreach (string property in other.properties)
                    {
                        properties.Add(property);
                        if (items != null)
                            items.Clear();
                    }
                }

                if(other.items != null && other.items.Count != 0)
                {
                    if (items == null)
                        items = new List<Item>();
                    
                    foreach (Item item in other.items)
                    {
                        items.Add(item);
                        if (properties != null)
                            properties.Clear();
                    }
                }
                
            }
            else
            {
                isDynamic = false;
                GUID = "";
                if (properties != null)
                    properties.Clear();
                if (items != null)
                    items.Clear();
            }
        }
        public void SetStack(TempStack other)
        {

            isDynamic = other.isDynamic;
            GUID = other.GUID;
            if (other.properties != null)
            {
                if (properties == null)
                    properties = new List<string>();
                foreach (string property in other.properties)
                {
                    properties.Add(property);
                }
            }
            if (other.items != null)
            {
                if (items == null)
                    items = new List<Item>();
                foreach (Item item in other.items)
                {
                    items.Add(item);
                }
            }
        }
    }


    public bool isDynamic { get; private set; }
    [SerializeField]
    public string GUIDBack = "";
    public string GUID
    {
        get
        {
            return GUIDBack;
        }
        private set
        {
            GUIDBack = value;
        }
    }
    
    public List<string> properties { get; private set; }
    public List<Item> items { get; private set; }

    [SerializeField]
    public ItemStack itemstackPrefab=null;
    public ItemStack itemStackPrefab
    {
        get
        {
            return itemstackPrefab;
        }
        private set
        {
            itemstackPrefab = value;
        }
    }

    [SerializeField]
    SpriteRenderer spriteRenderer = null;
    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    new private void OnValidate()
    {
        base.OnValidate();
        if(spriteRenderer==null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        //Item.dumpAll();
        //Debug.Log("guid: "+GUID);
        //Debug.Log("prefab: "+Item.GetPrefabComponent(GUID));
        gameObject.name = GUID;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (GUID!="" && spriteRenderer != null)
            spriteRenderer.sprite = Item.GetPrefabComponent(GUID).icon;
    }

    private void SetStack(ItemStack other)
    {
        if (other != null)
        {
            isDynamic = other.isDynamic;
            GUID = other.GUID;
            if (other.properties != null)
            {
                if (properties == null)
                    properties = new List<string>();

                foreach (string property in other.properties)
                {
                    properties.Add(property);
                    if (items != null)
                        items.Clear();
                }
            }

            if (other.items != null)
            {
                if (items == null)
                    items = new List<Item>();

                foreach (Item item in other.items)
                {
                    items.Add(item);
                    if (properties != null)
                        properties.Clear();
                }
            }

        }
        else
        {
            isDynamic = false;
            GUID = "";
            if (properties != null)
                properties.Clear();
            if (items != null)
                items.Clear();
        }

        UpdateIcon();
    }
    private void SetStack(TempStack other)
    {
        isDynamic = other.isDynamic;
        GUID = other.GUID;
        if (other.properties != null)
        {
            if (properties == null)
                properties = new List<string>();
            foreach (string property in other.properties)
            {
                properties.Add(property);
            }
        }
        if (other.items != null)
        {
            if (items == null)
                items = new List<Item>();
            foreach (Item item in other.items)
            {
                items.Add(item);
            }
        }
        UpdateIcon();
    }

    public void Swap(ref ItemStack other)
    {
        TempStack temp = new TempStack();
        temp.SetStack(this);
        SetStack(other);

        if(other==null)
        {
            other = objectPool.PopObject(itemstackPrefab);
        }
        other.SetStack(temp);
        other.UpdateIcon();
        UpdateIcon();
    }

    public bool IsEmpty()
    {
        bool isEmpty;
        if ((isDynamic == true && items.Count == 0) || (isDynamic != true && properties.Count == 0))
            isEmpty = true;
        else
            isEmpty = false;
        return isEmpty;
    }

    public int GetStackSize()
    {
        int rtnVal;
        if (isDynamic == true)
        {
            rtnVal = items.Count;
        }
        else
        {
            rtnVal = properties.Count;
        }
        return rtnVal;
    }

    ReturnStruct StoreItem(ItemStack itemStack, int Amount = -1)
    {
        ReturnStruct returnStruct = new ReturnStruct();
        if(GetStackSize()==0)
        {
            returnStruct.AmountNotStored = 0;
            returnStruct.AmountStored = itemStack.GetStackSize();
            returnStruct.returnCode = ReturnStruct.ReturnCode.ALL;
            if (itemStack.isDynamic == true)
            {
                items.AddRange(itemStack.items);
                itemStack.items.Clear();
            }
            else
            {
                properties.AddRange(itemStack.properties);
                itemStack.properties.Clear();
            }
        }
        else if(itemStack.isDynamic!=isDynamic)
        {
            returnStruct.AmountNotStored = itemStack.GetStackSize();
            returnStruct.AmountStored = 0;
            returnStruct.returnCode = ReturnStruct.ReturnCode.IS_DYNAMIC_MISSMATCH;
        }
        else if(itemStack.GUID!=GUID)
        {
            returnStruct.AmountNotStored = itemStack.GetStackSize();
            returnStruct.AmountStored = 0;
            returnStruct.returnCode = ReturnStruct.ReturnCode.GUID_MISSMATCH;
        }
        else if (Item.GetPrefabComponent(GUID).MaxStackSize <= GetStackSize())
        {
            returnStruct.AmountNotStored = itemStack.GetStackSize();
            returnStruct.AmountStored = 0;
            returnStruct.returnCode = ReturnStruct.ReturnCode.NONE;
        }
        else if (itemStack.GetStackSize()==0)
        {
            returnStruct.AmountNotStored = itemStack.GetStackSize();
            returnStruct.AmountStored = 0;
            returnStruct.returnCode = ReturnStruct.ReturnCode.NONE;
        }
        else if (((Item.GetPrefabComponent(GUID).MaxStackSize - GetStackSize()) >= itemStack.GetStackSize()))
        {
            returnStruct.AmountNotStored = 0;
            returnStruct.AmountStored = itemStack.GetStackSize();
            returnStruct.returnCode = ReturnStruct.ReturnCode.ALL;
            if (itemStack.isDynamic == true)
            {
                items.AddRange(itemStack.items);
                itemStack.items.Clear();
            }
            else
            {
                properties.AddRange(itemStack.properties);
                itemStack.properties.Clear();
            }
        }
        else if ((Item.GetPrefabComponent(GUID).MaxStackSize - GetStackSize() < itemStack.GetStackSize()))
        {
            int remainingSpace = Item.GetPrefabComponent(GUID).MaxStackSize - GetStackSize();
            int numToStore = itemStack.GetStackSize();

            int possibleToStore = numToStore - remainingSpace;

            returnStruct.AmountNotStored = numToStore-possibleToStore;
            returnStruct.AmountStored = possibleToStore;
            returnStruct.returnCode = ReturnStruct.ReturnCode.PART;
            if (itemStack.isDynamic == true)
            {
                for(int i = 0; i<possibleToStore;i++)
                {
                    items.Add(itemStack.items[0]);
                    itemStack.items.Remove(itemStack.items[0]);
                }
            }
            else
            {
                for (int i = 0; i < possibleToStore; i++)
                {
                    properties.Add(itemStack.properties[0]);
                    itemStack.properties.Remove(itemStack.properties[0]);
                }
            }
        }
        else
        {
            Debug.LogError("ItemStack unhandled case in: " + this);
        }
        UpdateIcon();
        return returnStruct;
    }

    public struct ReturnStruct
    {
        public enum ReturnCode
        {
            ALL,
            NONE,
            PART,
            GUID_MISSMATCH,
            IS_DYNAMIC_MISSMATCH
        }
        public int AmountStored;
        public int AmountNotStored;
        public ReturnCode returnCode;
    }

    public string SavePropertiesToJSONString()
    {
        TempStack tempStack = new TempStack();
        tempStack.SetStack(this);
        if(tempStack.isDynamic==true)
        {
            foreach(Item item in tempStack.items)
            {
                tempStack.properties.Add(item.SavePropertiesToJSONString());
            }
        }
        return JsonUtility.ToJson(tempStack);
    }
    public void LoadPropertiesFromJSONString(string data)
    {
        TempStack tempStack = JsonUtility.FromJson<TempStack>(data);
        if(tempStack.isDynamic==true)
        {
            foreach(string Property in tempStack.properties)
            {
                GameObject itemObj = Instantiate(Item.GetPrefabComponent(tempStack.GUID).gameObject);
                Item item = itemObj.GetComponent<Item>();
                item.LoadPropertiesFromJSONString(Property);
                tempStack.items.Add(item);
            }
        }
        UpdateIcon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectorEntity collectorEntity = collision.GetComponent<ICollectorEntity>();
        if(collectorEntity!=null)
        {
            collectorEntity.collect(this);
        }
    }
}


interface ICollectorEntity
{
    void collect(ItemStack itemStack);
}
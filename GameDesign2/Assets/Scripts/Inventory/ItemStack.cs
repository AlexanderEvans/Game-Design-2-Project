using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour, ISaveable, IPoolableObject
{
    public void Activate(string str="")
    {

    }

    public void Deactivate()
    {

    }

    public object getObjRef()
    {
        return this;
    }

    public void ReleaseSelf()
    {
        //do nothing in this case
    }

    public IPoolableObject CreateInstance(string str = "")
    {
        return new ItemStack();
    }


    [System.Serializable]
    struct TempStack
    {
        public bool isDynamic;
        public string GUID;

        public List<string> properties;
        public List<Item> items;

        public void SetStack(ItemStack other)
        {
            isDynamic = other.isDynamic;
            GUID = other.GUID;
            foreach (string property in other.properties)
            {
                properties.Add(property);
            }
            foreach (Item item in other.items)
            {
                items.Add(item);
            }
        }
        public void SetStack(TempStack other)
        {
            isDynamic = other.isDynamic;
            GUID = other.GUID;
            foreach (string property in other.properties)
            {
                properties.Add(property);
            }
            foreach (Item item in other.items)
            {
                items.Add(item);
            }
        }
    }


    bool isDynamic;
    string GUID;

    List<string> properties;
    List<Item> items;


    private void SetStack(ItemStack other)
    {
        isDynamic = other.isDynamic;
        GUID = other.GUID;
        foreach(string property in other.properties)
        {
            properties.Add(property);
        }
        foreach (Item item in other.items)
        {
            items.Add(item);
        }
    }
    private void SetStack(TempStack other)
    {
        isDynamic = other.isDynamic;
        GUID = other.GUID;
        foreach (string property in other.properties)
        {
            properties.Add(property);
        }
        foreach (Item item in other.items)
        {
            items.Add(item);
        }
    }

    public void Swap(ItemStack other)
    {
        TempStack temp = new TempStack();
        temp.SetStack(this);
        SetStack(other);
        other.SetStack(temp);
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

    int GetStackSize()
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
        if(itemStack.isDynamic!=isDynamic)
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
        else if ((Item.GetPrefabComponent(GUID).MaxStackSize - GetStackSize() >= itemStack.GetStackSize()))
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
    }
}

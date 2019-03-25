using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
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

    ReturnStruct StoreItem(ItemStack itemStack, int Amount = -1)
    {
        if(isDynamic==true)
        {

        }

        ReturnStruct returnStruct;
        if(itemStack.isDynamic!=isDynamic)
        {
            if (itemStack.isDynamic == true)
                returnStruct.AmountNotStored = itemStack.items.Count;
            else
                returnStruct.AmountNotStored = itemStack.properties.Count;
            returnStruct.AmountStored = 0;
            returnStruct.returnCode = ReturnStruct.ReturnCode.IS_DYNAMIC_MISSMATCH;
        }
        else if(itemStack.GUID!=GUID)
        {
            if (itemStack.isDynamic == true)
                returnStruct.AmountNotStored = itemStack.items.Count;
            else
                returnStruct.AmountNotStored = itemStack.properties.Count;
            returnStruct.AmountStored = 0;
            returnStruct.returnCode = ReturnStruct.ReturnCode.GUID_MISSMATCH;
        }
        else if(Item.GetPrefabComponent(GUID).MaxStackSize==stack)
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
}

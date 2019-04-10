using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class InventoryData : ScriptableObject//, ISaveable
{
    ObjectPool objectPool = null;
    //CollectionPool collectionPool = null;
    
    List<ItemStack> itemStacks = new List<ItemStack>();
    List<int> inputItemSlots;
    List<int> outputItemSlots;

    public void PushItem(ItemStack itemStack)
    {
        foreach(int index in inputItemSlots.TakeWhile(index => itemStack.GetStackSize()>0))
        {

        }
    }

    string SavePropertiesToJSONString()
    {
        return "";
    }
    void LoadPropertiesFromJSONString(string str)
    {

    }

    private void Awake()
    {
        AdjustInventorySize();

        if (inputItemSlots == null)
        {
            inputItemSlots = new List<int>();
            for(int i = 0; i< itemStacks.Count; i++)
            {
                inputItemSlots.Add(i);
            }
        }

        if (outputItemSlots == null)
        {
            outputItemSlots = new List<int>();
            for (int i = 0; i < itemStacks.Count; i++)
            {
                outputItemSlots.Add(i);
            }
        }
    }

    int inventorySize = 128;
    public int InventorySize { get; private set; }
    
    public bool SwapItemStacksAt(int index, ItemStack itemStackToSwap)
    {
        bool validOperation = false;

        if (index < itemStacks.Count)
        {
            validOperation = true;
            if(itemStackToSwap!=null)
            {
                itemStackToSwap.Swap(itemStacks[index]);
            }
            else if(itemStacks[index]!=null)
            {
                itemStacks[index].Swap(itemStackToSwap);
            }
        }
        return validOperation;
    }

    public bool SetInventorySize(int newSize)
    {
        int oldInvSize = inventorySize;
        inventorySize = newSize;
        bool success = AdjustInventorySize();
        if(success!=true)
        {
            inventorySize = oldInvSize;
            if (AdjustInventorySize() != true)
                Debug.LogError("Can't revert inventory size!");
        }
        return success;
    }

    private void OnEnable()
    {
        AdjustInventorySize();
    }

    bool AdjustInventorySize()
    {
        bool success = true;
        if (itemStacks.Count < inventorySize)
            GrowInventory();
        else if (itemStacks.Count > inventorySize)
            success = ShrinkInventory();
        return success;
    }

    readonly Predicate<ItemStack> IsNullCheckerDelegate = delegate (ItemStack a) { return a == null; };
    private bool ShrinkInventory()
    {
        bool success = true;
        while (itemStacks.Count>inventorySize && success)
        {
            int nullIndex = itemStacks.FindLastIndex(0, itemStacks.Count, IsNullCheckerDelegate);
            if (nullIndex != -1)
            {
                itemStacks[nullIndex].gameObject.SetActive(false);
                objectPool.PushObject(itemStacks[nullIndex]);
                itemStacks.RemoveAt(nullIndex);
            }
            else
                success = false;
        }
        return success;
    }

    private void GrowInventory()
    {
        while (itemStacks.Count < inventorySize)
        {
            itemStacks.Add(null);
        }
    }
}
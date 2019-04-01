using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData : ScriptableObject, ISaveable
{
    ObjectPool objectPool = null;
    CollectionPool collectionPool = null;
    
    List<ItemStack> itemStacks = new List<ItemStack>();
    int inventorySize = 60;
    
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

    Predicate<ItemStack> isNullLamda = delegate (ItemStack a) { return a == null; };
    private bool ShrinkInventory()
    {
        bool success = true;
        while (itemStacks.Count>inventorySize)
        {
            int nullIndex = itemStacks.FindLastIndex(0, itemStacks.Count, isNullLamda);
            if (nullIndex != -1)
                itemStacks.RemoveAt(nullIndex);
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
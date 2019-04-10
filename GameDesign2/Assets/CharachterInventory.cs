using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterInventory : MonoBehaviour, IOutputResource
{
    InventoryData inventoryData;

    private void Awake()
    {
        if (inventoryData == null)
            inventoryData = new InventoryData();
    }

    public bool Search(string GUID)
    {

    }

    public bool TryTakeOutput(string GUID, int amount = 1)
    {
        int itemCount;
        bool found = items.TryGetValue(GUID, out itemCount);
        if (found == true)
        {
            found = (itemCount >= amount);
            if (found == true)
            {
                itemCount -= amount;
            }
        }
        return found;
    }
    
    public int CheckOutput(string GUID, int amountRequested)
    {
        int itemCount;
        int amountFound = 0;
        bool found = items.TryGetValue(GUID, out itemCount);
        if (found == true)
        {
            if (itemCount < amountRequested)
            {
                amountFound = itemCount;
            }
            else if (itemCount == amountRequested)
            {
                amountFound = itemCount;
            }
            else
            {
                amountFound = amountRequested;
            }
        }
        else
        {
            amountFound = 0;
        }
        return amountFound;
    }

    public int CheckOutput(string GUID)
    {
        int itemCount;
        bool found = items.TryGetValue(GUID, out itemCount);
        if (found == true)
        {
            found = (itemCount >= amount);
        }
        return found;
    }

    public int TakePartialOutput(string GUID, int amountRequested)
    {
        int itemCount;
        int amountFound = 0;
        bool found = items.TryGetValue(GUID, out itemCount);
        if (found == true)
        {
            if(itemCount<amountRequested)
            {
                amountFound = itemCount;
                items.Remove(GUID);
            }
            else if(itemCount == amountRequested)
            {
                amountFound = itemCount;
                items.Remove(GUID);
            }
            else
            {
                itemCount -= amountRequested;
                amountFound = amountRequested;
            }
        }
        else
        {
            amountFound = 0;
        }
        return amountFound;
    }
}

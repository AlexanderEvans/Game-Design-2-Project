using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterInventory : MonoBehaviour
{
    Dictionary<int, int> items = new Dictionary<int, int>();

    public bool Search(int GUID)
    {
        return items.ContainsKey(GUID);
    }

    public bool TryGetAmount(int GUID, int amount = 1)
    {
        int itemCount;
        bool found = items.TryGetValue(GUID, out itemCount);
        if (found == true)
        {
            found = (itemCount >= amount);
            if(found==true)
            {
                itemCount -= amount;
            }
        }
        return found;
    }

    public bool TryGetPatialAmount(int GUID, out int amountFound, int amountRequested)
    {
        int itemCount;
        bool found = items.TryGetValue(GUID, out itemCount);
        if (found == true)
        {
            if(itemCount<amountRequested)
            {
                amountFound = itemCount;
                found = false;
                items.Remove(GUID);
            }
            else if(itemCount == amountRequested)
            {
                amountFound = itemCount;
                found = true;
                items.Remove(GUID);
            }
            else
            {
                itemCount -= amountRequested;
                amountFound = amountRequested;
                found = true;
            }
        }
        else
        {
            amountFound = 0;
        }
        return found;
    }
}

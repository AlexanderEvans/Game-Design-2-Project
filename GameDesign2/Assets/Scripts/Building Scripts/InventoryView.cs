using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView
{
    InventoryData inventoryData;
    public InventoryView(InventoryData inventoryData) 
    {
        this.inventoryData = inventoryData;
    }
    private InventoryView() { }//disallow the normal constructor


}
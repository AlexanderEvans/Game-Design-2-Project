using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{

    InventoryData inventoryData;

    private void Awake()
    {
        //initialise out slot mappings and items
        setUpInventoryData(inventoryData);

        //load items here?
    }

    public bool setUpInventoryData(InventoryData inventoryData)
    {
        bool success = true;

        //set up item slots layout/in-out lists
        //this replaces the dumb startup

        return success;
    }

    //make a list of item slots
    //keep track of what page of items you are on
    //don't do any of this in update
    //do bounds checking on inventory sizes
    //make sure input slots map to inputs and outputs to outpusts 
    //when talking to the data container
    //via their indexes
}

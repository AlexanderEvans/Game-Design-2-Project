using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ResourceBuildings : MonoBehaviour, IOutputResource
{
    [System.Serializable]
    struct ItemsTemplate
    {
        [HideInInspector]
        public int itemGUID;
#pragma warning disable 0649
        public int count;
        [SerializeField]
        private Item prefab;
#pragma warning restore 0649
        public void InitializeGUID()
        {
            Debug.Assert(prefab != null, "Error: prefab is null in " + this);
            itemGUID = prefab.getItemGUID();
        }
        
    }


    private void Awake()
    {
        foreach (ItemsTemplate item in itemsRequireds)
        {
            item.InitializeGUID();
        }
    }

    public bool TakeOutput(int itemGUID, int amount)
    {
        foreach (Item.ItemsSlot itemSlot in outputItems)
        {
            if (itemGUID == itemSlot.itemGUID)
            {
                if (itemSlot.count >= amount)
                {
                    itemSlot.count -= amount;
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckOutput(int itemGUID, int amount)
    {
        foreach (Item.ItemsSlot itemSlot in outputItems)
        {
            if (itemGUID == itemSlot.itemGUID)
            {
                if (itemSlot.count >= amount)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public int TakePartialOutput(int itemGUID, int amount)
    {
        foreach (Item.ItemsSlot itemSlot in outputItems)
        {
            if (itemGUID == itemSlot.itemGUID)
            {
                if (itemSlot.count >= amount)
                {
                    itemSlot.count -= amount;
                    return amount;
                }
                else
                {
                    int tempInt = itemSlot.count;
                    itemSlot.count = 0;
                    return tempInt;
                }
            }
        }
        return 0;
    }
    public int CheckPartialOutput(int itemGUID, int amount)
    {
        foreach (Item.ItemsSlot itemSlot in outputItems)
        {
            if (itemGUID == itemSlot.itemGUID)
            {
                if (itemSlot.count >= amount)
                {
                    return amount;
                }
                else
                {
                    return itemSlot.count;
                }
            }
        }
        return 0;
    }

    //set in inspector
    [SerializeField]
    List<ItemsTemplate> itemsRequireds = new List<ItemsTemplate>();
    [SerializeField]
    List<ItemsTemplate> outputProducts = new List<ItemsTemplate>();

    //set in code
    List<ResourceConnection> inputConnections = new List<ResourceConnection>();
    List<Item.ItemsSlot> outputItems = new List<Item.ItemsSlot>();
    List<ResourceConnection> outputConnection = new List<ResourceConnection>();

    //max item stack size
    //do stuff

    void craftRecipe()
    {
        bool failed = false;
        //grabbed inputs
        foreach (ItemsTemplate itemsSlot in itemsRequireds.TakeWhile(itemSlot =>failed==false))
        {
            int amount = itemsSlot.count;
            foreach (ResourceConnection resourceConnection in inputConnections.TakeWhile(resourceConnection => amount>0))
            {
                IOutputResource iOutput = resourceConnection.input;
                amount -= iOutput.CheckPartialOutput(itemsSlot.itemGUID, amount);
            }
            if (amount > 0)
                failed = true;
        }

        //generate outputs
        if(failed!=true)
        {
            while(outputItems.Count < outputProducts.Count)
            {
                Item.ItemsSlot itemsSlot = new Item.ItemsSlot();
                outputItems.Add(itemsSlot);
            }
            for(int i = 0; i< outputProducts.Count; i++)
            {
                if (outputItems[i].itemGUID == outputProducts[i].itemGUID)
                {
                    outputItems[i].count += outputProducts[i].count;
                }
                else if(outputItems[i].itemGUID == -1)
                {
                    outputItems[i].count = outputProducts[i].count;
                    outputItems[i].itemGUID = outputProducts[i].itemGUID;
                }
                else
                {
                    failed = true;
                }

            }
            if(failed!=true)
            {
                foreach (ItemsTemplate itemsSlot in itemsRequireds.TakeWhile(itemSlot => failed == false))
                {
                    int amount = itemsSlot.count;
                    foreach (ResourceConnection resourceConnection in inputConnections.TakeWhile(resourceConnection => amount > 0))
                    {
                        IOutputResource iOutput = resourceConnection.input;
                        amount -= iOutput.TakePartialOutput(itemsSlot.itemGUID, amount);
                    }
                }
            }
        }

    }
    
}

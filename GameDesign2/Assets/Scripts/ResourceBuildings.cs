using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceBuildings : MonoBehaviour, IOutput
{
    [System.Serializable]
    struct ItemsRequired
    {
        public int count;
        [HideInInspector]
        public int itemGUID;
        [SerializeField]
        private Item prefab;
        public void InitializeGUID()
        {
            itemGUID = prefab.getItemGUID();
        }
        
    }
    [System.Serializable]
    struct ItemsProduced
    {
        public int count;
        [HideInInspector]
        public int itemGUID;
        [SerializeField]
        private Item prefab;
        public void InitializeGUID()
        {
            itemGUID = prefab.getItemGUID();
        }
    }
    private void Awake()
    {
        foreach (ItemsRequired item in itemsRequireds)
        {
            item.InitializeGUID();
        }
        foreach (ItemsProduced item in outputProducts)
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
    List<ItemsRequired> itemsRequireds = new List<ItemsRequired>();
    [SerializeField]
    List<ItemsProduced> outputProducts = new List<ItemsProduced>();

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
        foreach (ItemsRequired itemsSlot in itemsRequireds.TakeWhile(itemSlot =>failed==false))
        {
            int amount = itemsSlot.count;
            foreach (ResourceConnection resourceConnection in inputConnections.TakeWhile(resourceConnection => amount>0))
            {
                IOutput iOutput = resourceConnection.input;
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
                else if(outputItems[i].itemGUID== -1)
                {
                    outputItems[i].count += outputProducts[i].count;
                    outputItems[i].itemGUID += outputProducts[i].itemGUID;
                }
                else
                {
                    failed = true;
                }

            }
            if(failed!=true)
            {
                foreach (ItemsRequired itemsSlot in itemsRequireds.TakeWhile(itemSlot => failed == false))
                {
                    int amount = itemsSlot.count;
                    foreach (ResourceConnection resourceConnection in inputConnections.TakeWhile(resourceConnection => amount > 0))
                    {
                        IOutput iOutput = resourceConnection.input;
                        amount -= iOutput.TakePartialOutput(itemsSlot.itemGUID, amount);
                    }
                }
            }
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ResourceBuildings : MonoBehaviour, IOutputResource
{
    [SerializeField]
    public RecipeTemplate[] recipeTemplates { get; private set; }
    RecipeTemplate activeRecipeTemplate;

    void SetActiveRecipeTemplate(int index)
    {
        Debug.Assert(index < recipeTemplates.Count() && index>=0, "Error: "+this+" Recieved an invalid recipeTemplateIndex!");
        activeRecipeTemplate = recipeTemplates[index];
    }

    private void Awake()
    {
        //initialization workaround
        if(activeRecipeTemplate.initialized == false)
        {
            activeRecipeTemplate.InitializeGUIDS();
            activeRecipeTemplate.initialized = true;
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
        foreach (RecipeTemplate.ItemsTemplate itemsSlot in activeRecipeTemplate.ItemsRequired.TakeWhile(itemSlot =>failed==false))
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
            while(outputItems.Count < activeRecipeTemplate.OutputProducts.Count)
            {
                Item.ItemsSlot itemsSlot = new Item.ItemsSlot();
                outputItems.Add(itemsSlot);
            }
            for(int i = 0; i< activeRecipeTemplate.OutputProducts.Count; i++)
            {
                if (outputItems[i].itemGUID == activeRecipeTemplate.OutputProducts[i].itemGUID)
                {
                    outputItems[i].count += activeRecipeTemplate.OutputProducts[i].count;
                }
                else if(outputItems[i].itemGUID == -1)
                {
                    outputItems[i].count = activeRecipeTemplate.OutputProducts[i].count;
                    outputItems[i].itemGUID = activeRecipeTemplate.OutputProducts[i].itemGUID;
                }
                else
                {
                    failed = true;
                }

            }
            if(failed!=true)
            {
                foreach (RecipeTemplate.ItemsTemplate itemsSlot in activeRecipeTemplate.ItemsRequired.TakeWhile(itemSlot => failed == false))
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

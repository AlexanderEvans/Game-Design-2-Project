using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ResourceBuildings : MonoBehaviour, IOutputResource
{
    [SerializeField]
    public List<RecipeTemplate> RecipeTemplates = new List<RecipeTemplate>();
    RecipeTemplate activeRecipeTemplate;

    void SetActiveRecipeTemplate(int index)
    {
        Debug.Assert(index < RecipeTemplates.Count() && index>=0, "Error: "+this+" Recieved an invalid recipeTemplateIndex!");
        activeRecipeTemplate = RecipeTemplates[index];
    }

    private void Awake()
    {
        //initialization workaround
        if(activeRecipeTemplate!=null&&activeRecipeTemplate.initialized == false)
        {
            activeRecipeTemplate.InitializeGUIDS();
            activeRecipeTemplate.initialized = true;
        }
    }

    public bool TakeOutput(string itemGUID, int amount)
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
    public bool CheckOutput(string itemGUID, int amount)
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

    public int TakePartialOutput(string itemGUID, int amount)
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
    public int CheckPartialOutput(string itemGUID, int amount)
    {
        foreach(int outputIndex in outputItemSlots)
        {
            if(allItems[outputIndex].)
        }

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
    List<ResourceConnection> outputConnection = new List<ResourceConnection>();
    

    //new
    List<ItemStack> allItems = new List<ItemStack>();
    List<int> inputItemSlots = new List<int>();
    List<int> outputItemSlots = new List<int>();

    //max item stack size
    //do stuff

    public void CraftRecipe()
    {
        bool failed = false;
        //grabbed inputs
        foreach (RecipeTemplate.ItemsTemplate itemsSlot in activeRecipeTemplate.ItemsRequired.TakeWhile(itemSlot =>failed==false))
        {
            int amount = itemsSlot.count;
            foreach (ResourceConnection resourceConnection in inputConnections.TakeWhile(resourceConnection => amount>0))
            {
                IOutputResource iOutput = resourceConnection.input;
                amount -= iOutput.CheckPartialOutput(itemsSlot.ItemGUID, amount);
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
                if (outputItems[i].itemGUID == activeRecipeTemplate.OutputProducts[i].ItemGUID)
                {
                    outputItems[i].count += activeRecipeTemplate.OutputProducts[i].count;
                }
                else if(outputItems[i].itemGUID == "")
                {
                    outputItems[i].count = activeRecipeTemplate.OutputProducts[i].count;
                    outputItems[i].itemGUID = activeRecipeTemplate.OutputProducts[i].ItemGUID;
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
                        amount -= iOutput.TakePartialOutput(itemsSlot.ItemGUID, amount);
                    }
                }
            }
        }

    }
    
}

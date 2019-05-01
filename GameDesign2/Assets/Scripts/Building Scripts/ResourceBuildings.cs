using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ResourceBuildings : MonoBehaviour//, IOutputResource
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

    bool TryTakeOutput(string itemGUID, int amount)
    {
        return true;
    }
    bool CheckOutput(string itemGUID, int amount)
    {
        //foreach(int index in )
        return true;
    }
    int CheckOutput(string itemGUID)
    {
        return 1;
    }

    int TakePartialOutput(string itemGUID, int amount)
    {
        return 1;
    }


    //set in code
    List<ResourceConnection> inputConnections = new List<ResourceConnection>();
    List<ResourceConnection> outputConnection = new List<ResourceConnection>();
    

    //new
    InventoryData inventoryData;

    //max item stack size
    //do stuff

    public void CraftRecipe()
    {

    }
    
}

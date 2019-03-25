using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;

[RequireComponent(typeof(Item))]
public class ItemPooler : PrefabPooler
{
    Item item = null;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    public override void Activate(string objectProperties = "")
    {
        item.LoadPropertiesFromJSONString(objectProperties);
    }

    public override IPoolableObject CreateInstance(string objectProperties = "")
    {
        Debug.Assert(PrefabStageUtility.GetPrefabStage(gameObject) != null, "Error, not a prefab");
        GameObject newGameObject = Instantiate(gameObject);
        newGameObject.GetComponent<Item>().LoadPropertiesFromJSONString(objectProperties);
        return newGameObject.GetComponent<ItemPooler>();
    }

}

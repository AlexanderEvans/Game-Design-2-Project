using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "I've come up with a new Recipay~", menuName = "Scriptable Object/New Recipe")]
public class RecipeTemplate : ScriptableObject
{
    [System.Serializable]
    public struct ItemsTemplate
    {
        public int ItemGUID { get; private set; }
#pragma warning disable 0649
        public int count;
        [SerializeField]
        private Item prefab;
#pragma warning restore 0649
        public void InitializeGUID()
        {
            Debug.Assert(prefab != null, "Error: prefab is null in " + this);
                ItemGUID = prefab.GUID;
            if(prefab==null)
                Debug.LogWarning(prefab + " is null!");
        }
    }

    //initialization workaround for not having OnValidate();
    [HideInInspector]
    public bool initialized = false;

    //set in inspector
    [SerializeField]
    public List<ItemsTemplate> ItemsRequired = new List<ItemsTemplate>();
    [SerializeField]
    public List<ItemsTemplate> OutputProducts = new List<ItemsTemplate>();

    private void OnValidate()
    {
        InitializeGUIDS();
    }

    public void InitializeGUIDS()
    {
        foreach (ItemsTemplate item in ItemsRequired)
        {
            item.InitializeGUID();
        }
        foreach (ItemsTemplate item in OutputProducts)
        {
            item.InitializeGUID();
        }
    }
}


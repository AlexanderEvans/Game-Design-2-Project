using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiSlotController : MonoBehaviour
{
    [SerializeField] Button leftButton = null;
    [SerializeField] Button rightButton = null;
    [SerializeField] List<Image> images = new List<Image>();
    [SerializeField] CharachterInventory charachterInventory = null;
    List<Item> items = new List<Item>();
    int maxPages = 5;

    public int MaxPages
    {
        get
        {
            return maxPages;
        }
        set
        {
            if(value>maxPages)
            {
                maxPages = value;
            }
            else if(maxPages==value)
            {
                Debug.LogWarning("maxPages is already "+value);
            }
            else
            {
                Debug.LogError("Cannot shrink maxPages!");
            }
        }
    }

    void OverrideMaxPages(int maxPagesOverride)
    {
        maxPages = maxPagesOverride;
    }

    public int page;

    void UpdateItemsList()
    {
        foreach (string str in charachterInventory.items.Keys)
        {
            Item prefabRef = Item.GetPrefabComponent(str);
            items.Add(prefabRef);
        }
    }

    void NextSet()
    {
        if(page < maxPages)
        {
            page++;
        }
        enforceRightButton();
    }

    void enforceRightButton()
    {
        if (page < maxPages)
        {
            rightButton.enabled = true;
        }
        else
        {
            rightButton.enabled = false;
        }
    }

    void prevSet()
    {
        if (page > 0)
        {
            page--;
        }
        enforceLeftButton();
    }

    void enforceLeftButton()
    {
        if (page > 0)
        {
            leftButton.enabled = true;
        }
        else
        {
            leftButton.enabled = false;
        }
    }

    void UpdateImages()
    {
        for(int itemIndex = page*images.Count, imageIndex = 0; itemIndex < images.Count; itemIndex++, imageIndex++)
        {
            if(itemIndex<items.Count)
                images[imageIndex].sprite = items[itemIndex].icon;
        }
    }
}

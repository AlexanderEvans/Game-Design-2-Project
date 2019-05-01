using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryCollector : MonoBehaviour, ICollectorEntity
{
    [SerializeField]
    InventoryData inventoryData;
    [SerializeField]
    ObjectPool objectPool;

    public void collect(ItemStack itemStack)
    {
        inventoryData.insert(itemStack);
        itemStack.gameObject.SetActive(false);
        objectPool.PushObject(itemStack);

    }

    private void Awake()
    {
        if(inventoryData==null)
        {
            Debug.LogError("inventoryData should not be null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

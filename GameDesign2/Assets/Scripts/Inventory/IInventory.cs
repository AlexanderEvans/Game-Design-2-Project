

internal interface IInventory
{
    void tryGetItem(int requested, out int available, out InventoryData.GenericItemSlot itemInfo);
}
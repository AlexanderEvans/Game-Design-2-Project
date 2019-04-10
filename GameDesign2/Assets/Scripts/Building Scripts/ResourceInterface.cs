using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IOutputResource
{
    bool TryTakeOutput(string itemGUID, int amount);
    bool CheckOutput(string itemGUID, int amount);
    int CheckOutput(string itemGUID);
    int TakePartialOutput(string itemGUID, int amount);
}

interface IInputResource
{
    bool TryInsertInput(string itemGUID, int amount);
    bool CheckInput(string itemGUID, int amount);
    int CheckPartialInput(string itemGUID, int amount);
    int PlacePartialInput(string itemGUID, int amount);
}
interface IGUIResource
{
    InventoryView GetInventoryView();
}

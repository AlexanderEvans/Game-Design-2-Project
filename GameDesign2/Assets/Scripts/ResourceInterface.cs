using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IOutputResource
{
    bool TakeOutput(int itemGUID, int amount);
    bool CheckOutput(int itemGUID, int amount);
    int TakePartialOutput(int itemGUID, int amount);
    int CheckPartialOutput(int itemGUID, int amount);
}

interface IInputResource
{
    bool PlaceInput(int itemGUID, int amount);
    bool CheckInput(int itemGUID, int amount);
    int CheckPartialInput(int itemGUID, int amount);
    int PlacePartialInput(int itemGUID, int amount);
}

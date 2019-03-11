using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IOutputResource
{
    bool TakeOutput(string itemGUID, int amount);
    bool CheckOutput(string itemGUID, int amount);
    int TakePartialOutput(string itemGUID, int amount);
    int CheckPartialOutput(string itemGUID, int amount);
}

interface IInputResource
{
    bool PlaceInput(string itemGUID, int amount);
    bool CheckInput(string itemGUID, int amount);
    int CheckPartialInput(string itemGUID, int amount);
    int PlacePartialInput(string itemGUID, int amount);
}

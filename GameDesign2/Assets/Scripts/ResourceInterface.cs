using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IOutput
{
    bool TakeOutput(int itemGUID, int amount);
    bool CheckOutput(int itemGUID, int amount);
    int TakePartialOutput(int itemGUID, int amount);
    int CheckPartialOutput(int itemGUID, int amount);
}

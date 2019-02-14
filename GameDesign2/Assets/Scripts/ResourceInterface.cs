using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInput
{
    bool TakeInput(Item item);
}

interface IOutput
{
    Item[] GiveOutput();
}
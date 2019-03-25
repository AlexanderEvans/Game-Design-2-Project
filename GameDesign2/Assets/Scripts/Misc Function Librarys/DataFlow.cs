using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataFlow
{
    public static void Swap<T>(T one, T two)
    {
        T temp = one;
        one = two;
        two = temp;
    }
}

interface ISaveable
{
    string SavePropertiesToJSONString();
    void LoadPropertiesFromJSONString(string str);
}

using UnityEngine;
using System;
using System.Collections.Generic;


public class SerializeableItemDictionary : MonoBehaviour, ISerializationCallbackReceiver
{
    public List<string> _keys = new List<string>();
    public List<GameObject> _values = new List<GameObject>();

    //Unity doesn't know how to serialize a Dictionary
    public Dictionary<string, GameObject> _myDictionary = new Dictionary<string, GameObject>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in _myDictionary)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        _myDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            _myDictionary.Add(_keys[i], _values[i]);
    }

    void OnGUI()
    {
        foreach (var kvp in _myDictionary)
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }
}
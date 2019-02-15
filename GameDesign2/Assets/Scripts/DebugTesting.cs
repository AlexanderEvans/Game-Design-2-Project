using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTesting : MonoBehaviour
{
    [SerializeField]
    MeleeWeapon meleeWeapon;
    // Start is called before the first frame update
    void Start()
    {
        Item item = meleeWeapon;
        GenericFunction(gameObject);
        GenericFunction(item);
        GenericFunction(meleeWeapon);

        GenericFunction(4);
        GenericFunction(84.48);
    }

    void GenericFunction<T>(T obj)
    {
        Debug.Log(obj + " ; " + obj.GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

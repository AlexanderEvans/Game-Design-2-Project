using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGUIObj : MonoBehaviour
{
    [SerializeField] GameObject gameObjectToToggle = null;


    public void ToggleGUIObject()
    {
        if(gameObjectToToggle.activeSelf==true)
        {
            gameObjectToToggle.SetActive(false);
        }
        else
        {
            gameObjectToToggle.SetActive(true);
        }
    }
}

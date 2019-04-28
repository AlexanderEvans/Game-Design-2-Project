using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    GameObject player;
    Canvas canvas;
    Text[] text;
    PlayerCombatController playercombatcontroler;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        player = GameObject.Find("Player");
        text = canvas.GetComponentsInChildren<Text>();
        playercombatcontroler = player.GetComponentInChildren<PlayerCombatController>();
    }

    // Update is called once per frame
    void Update()
    {
        text[3].text = playercombatcontroler.HP.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    Canvas mainmenu;
    [SerializeField]
    WorldManager worldmanager;
    InputField input;
    public int seed;
    // Start is called before the first frame update
    void Start()
    {
        mainmenu = GetComponentInChildren<Canvas>();
        mainmenu.enabled = true;
        input = mainmenu.GetComponentInChildren<InputField>();
        input.text = worldmanager.seed.ToString();
        

        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        ScanForKeyStoke();
    }

    public void ToggleMainMenu()
    {
        if(mainmenu.enabled)
        {
            mainmenu.enabled = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            mainmenu.enabled = true;
            Time.timeScale = 0f;
        }
    }

    public void ScanForKeyStoke()
    {
        if (Input.GetKeyDown("escape"))
        {
            ToggleMainMenu();
        }
    }

    public void NewGame()
    {
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
        worldmanager.Awake();
    }

    public void SetSeed(string newSeed)
    {
        if(worldmanager == null)
        {
            Debug.Log("world manager null");
        }
        else
        {
           worldmanager.seed = int.Parse(newSeed);
        }
        
    }

}

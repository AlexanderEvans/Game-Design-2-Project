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
    GameObject player;
    public int seed;
    // Start is called before the first frame update
    void Start()
    {
        mainmenu = GetComponentInChildren<Canvas>();
        mainmenu.enabled = true;
        player = GameObject.Find("Player");
        
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
        if (player != null)
        {
            player.transform.SetPositionAndRotation(new Vector3(0f, 0f,-1f), Quaternion.identity);
            
        }
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

    public void SetHight(string newhight)
    {
        if (worldmanager == null)
        {
            Debug.Log("world manager null");
        }
        else
        {
            worldmanager.rows = int.Parse(newhight);
        }

    }

    public void SetWidth(string newWidth)
    {
        if (worldmanager == null)
        {
            Debug.Log("world manager null");
        }
        else
        {
            worldmanager.columns = int.Parse(newWidth);
        }

    }

}

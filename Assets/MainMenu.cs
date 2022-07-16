using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject secondary;
    public void startgame()
    {
        SceneManager.LoadScene(1);
    }

    public void help()
    {
        secondary.SetActive(true);
        mainmenu.SetActive(false);
    }

    public void back()
    {
        mainmenu.SetActive(true);
        secondary.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
    }
    
}

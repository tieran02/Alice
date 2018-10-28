using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    //The mainmenu panel for the Mainmenu
    public GameObject MainMenuPanel;
    //The credits panel for the Mainmenu
    public GameObject CreidtsPanel;

    //Loads the first game scene
    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    //Quits the game 
    public void QuitGame()
    {
        Application.Quit();
    }

    //Toggle the credits panel
    public void ToggleCredits()
    {
        if (MainMenuPanel.activeInHierarchy)
        {
            //disable mainmenu
            MainMenuPanel.SetActive(false);
            //enable credits
            CreidtsPanel.SetActive(true);
        }
        else
        {
            //enable mainmenu
            MainMenuPanel.SetActive(true);
            //disable credits
            CreidtsPanel.SetActive(false);
        }
    }
}

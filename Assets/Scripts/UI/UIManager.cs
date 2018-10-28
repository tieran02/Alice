using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class UIManager : MonoBehaviour
{
    //InventoryUI component
    public InventoryUI InventoryUI;
    //Death menu and pause menus
    public CanvasGroup DeathMenu;
    public CanvasGroup PauseMenu;

    void Awake()
    {
        //Add the OnPlayerDie method to the players death event to notify the UI when the player dies
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterData>().OnDeathEvent += OnPlayerDie;
    }

	// Update is called once per frame
	void Update ()
    {
        //if the player presses TAB show then toggle the inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryUI.Toggle();
        }
        //check if the play has pressed escape and if so toggle pause game
        else if (Input.GetKeyDown(KeyCode.Escape))
            TogglePauseMenu();
	}

    void OnPlayerDie()
    {
        //When the player dies show the death menu
        DeathMenu.alpha = 1;
        DeathMenu.interactable = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideDeathMenu()
    {
        //Hides the death menu (used when restarting from checkpoint)
        DeathMenu.alpha = 0;
        DeathMenu.interactable = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TogglePauseMenu()
    {
        //Get the FPS controller attached to the player
        FirstPersonController fps = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();

        //if pause menu is hidden then show it
        if (PauseMenu.alpha == 0)
        {
            //Show the pause menu
            PauseMenu.alpha = 1;
            PauseMenu.interactable = true;
            PauseMenu.blocksRaycasts = true;
            //Set the timescale to 0 to pause the game
            Time.timeScale = 0;
            //Set the look sensitivity to 0 and unlock the cursor
            fps.m_MouseLook.XSensitivity = 0;
            fps.m_MouseLook.YSensitivity = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else // pause menu is visable so unpause the game
        {
            // hide the menu
            PauseMenu.alpha = 0;
            PauseMenu.interactable = false;
            PauseMenu.blocksRaycasts = false;
            //Set the time scale back to one
            Time.timeScale = 1;
            //Set the sensitivity back to default and lock the cursor
            fps.m_MouseLook.XSensitivity = 2;
            fps.m_MouseLook.YSensitivity = 2;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

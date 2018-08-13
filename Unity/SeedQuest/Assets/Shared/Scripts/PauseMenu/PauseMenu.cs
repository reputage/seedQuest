using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject menuObjects;
    public GameObject optionsMenu;

    public GameStateData gameState;
    private bool optionsMenuOpen;

	
    private void Start()
	{
        gameState.isPaused = false;
        optionsMenuOpen = false;
	}


	void Update () {
        if (gameState.isPaused && gameState.isStarted && !optionsMenuOpen)
            activatePause();
        else if (gameState.isPaused && gameState.isStarted && optionsMenuOpen)
            deactivatePauseMenuOnly();
        else
            deactivatePause();
	}

    // Activate the pause state and pause menu
    public void activatePause()
    {
        //Debug.Log("Pausing the game, activating pause menu");
        menuObjects.SetActive(true);
    }

    // Deactivate the pause state and decativate the pause menu
    public void deactivatePause()
    { 
        //Debug.Log("Decativating pause menu");
        gameState.isPaused = false;
        menuObjects.SetActive(false);
        optionsMenu.SetActive(false);
        optionsMenuOpen = false;
    }

    // Deactivate the pause menu without unpausing the pause state
    public void deactivatePauseMenuOnly()
    {
        //Debug.Log("Decativating pause menu");
        menuObjects.SetActive(false);
    }

    // Function to quit the game
    public void quitGame()
    {
        Application.Quit();
    }

    // Function for the continue button in the pause menu
    public void continueButton()
    {
        //Debug.Log("Continue from the continue button");
        deactivatePause();
    }

    // Function for the restart button in the pause menu
    public void restartButton()
    {
        deactivatePause();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // This works, might need to change later
    }

    // Function for the option button
    public void optionsButton()
    {
        //Debug.Log("options button pressed");
        optionsMenuOpen = true;
        menuObjects.SetActive(false);
        optionsMenu.SetActive(true);
    }

    // Function for the back button in the options menu
    public void optionsBackButton()
    {
        //Debug.Log("options back button pressed");
        optionsMenuOpen = false;
        optionsMenu.SetActive(false);
        menuObjects.SetActive(true);
    }

}

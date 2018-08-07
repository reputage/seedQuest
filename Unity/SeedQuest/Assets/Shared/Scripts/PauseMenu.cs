using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject menuObjects;
    public GameStateData gameState;

	
    private void Start()
	{
        gameState.isPaused = false;
	}


	void Update () {
        if (gameState.isPaused && gameState.isStarted)
            activatePause();
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

}

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

    public void activatePause()
    {
        //Debug.Log("Pausing the game, activating pause menu");
        menuObjects.SetActive(true);
    }

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

    public void continueButton()
    {
        Debug.Log("Continue from the continue button");
        deactivatePause();
    }

    public void restartButton()
    {
        deactivatePause();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // This works, might need to change later
    }

}

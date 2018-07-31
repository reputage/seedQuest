using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject menuObjects;
    public GameStateData gameState;

    private bool menuActive = false;

	private void Start()
	{
		
	}

	void Update () {
        if (gameState.isPaused && gameState.isStarted)
            activatePause();
        else
            deactivatePause();
	}

    public void activatePause()
    {
        if (menuActive)
            return;
        else
        {
            Debug.Log("Pausing the game, activating pause menu");
            menuObjects.SetActive(true);
            menuActive = true;
        }
    }

    public void deactivatePause()
    {
        if (!menuActive)
            return;
        else
        {
            Debug.Log("Decativating pause menu");
            gameState.isPaused = false;
            menuObjects.SetActive(false);
            menuActive = false;
        }
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject menuObjects;
    public GameStateData gameState;

    private bool menuActive = false;

	private void Start()
	{
		
	}

	void Update () {
        if (gameState.isPaused)
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
            menuObjects.SetActive(false);
            menuActive = false;
        }
        
    }

    // Function to quit the game
    public void quitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newStartMenu : MonoBehaviour {

    public GameStateData gameState;
    public GameObject startMenu;

    public string seedInput;

	private void Start()
	{
        seedInput = "";
	}

    // This function should be called once the game enters rehearsal mode
	public void inRehearsalMode()
    {
        if(seedInput != "")
        {
            setStateDataSeed();
        }
        gameState.startPathSearch = true;
        gameState.inRehersalMode = true;
        gameState.isStarted = true;
        startMenu.SetActive(false);
    }

    // This function should be called once the game enters recall mode
    public void inRecallMode()
    {
        gameState.startPathSearch = true;
        gameState.inRehersalMode = false;
        gameState.isStarted = true;
        startMenu.SetActive(false);
    }

    // Function for the quit button on the start menu
    public void quitApplication()
    {
        Application.Quit();
    }

    // Function for the input field on the start screen for inputing a string
    public void getSeedFromInput(string seedFromInput)
    {
        seedInput = seedFromInput;
        Debug.Log(seedInput);
    }

    // Pass the input seed to the GameState data, so it can find the right Path
    public void setStateDataSeed()
    {
        gameState.SeedString = seedInput;
    }

}



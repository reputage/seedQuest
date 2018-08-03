using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newStartMenu : MonoBehaviour {

    public GameStateData gameState;
    public GameObject startMenu;

    public string seedInput;

    public void inRehearsalMode()
    {
        gameState.startPathSearch = true;
        gameState.inRehersalMode = true;
        gameState.isStarted = true;
        startMenu.SetActive(false);
    }

    public void inRecallMode()
    {
        gameState.startPathSearch = true;
        gameState.inRehersalMode = false;
        gameState.isStarted = true;
        startMenu.SetActive(false);
    }

    public void quitApplication()
    {
        Application.Quit();
    }

    public void getSeedFromInput(string seedFromInput)
    {
        seedInput = seedFromInput;
    }

}



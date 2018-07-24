using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newStartMenu : MonoBehaviour {

    public GameStateData gameState;
    public GameObject startMenu;

    public void inRehearsalMode()
    {
        gameState.startPathSearch = true;
        gameState.inRehersalMode = true;
        startMenu.SetActive(false);
    }

    public void inRecallMode()
    {
        gameState.startPathSearch = true;
        gameState.inRehersalMode = false;
        startMenu.SetActive(false);
    }

    public void quitApplication()
    {
        Application.Quit();
    }

}



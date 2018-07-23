using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newStartMenu : MonoBehaviour {

    public GameStateData gameState;
    public GameObject startMenu;
    //public ClipboardCopy clipboardCopier;

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

    public void copyToClip(string testerString)
    {
        testerString = "testing a copy function!";
        testerString.CopyToClipboard();
        Debug.Log("Hello from copyToClip()");
    }

}



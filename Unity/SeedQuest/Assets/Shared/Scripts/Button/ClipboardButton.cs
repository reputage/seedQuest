using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardButton : MonoBehaviour {

    public GameStateData gameState;
    public GameObject clipboardButton;

    public void copyToClip(string seed)
    {
        seed = gameState.recoveredSeed;
        if (seed == null)
            return;
        seed.CopyToClipboard();
        //Debug.Log("Hello from copyToClip()");
    }

    public void inactivate()
    {
        clipboardButton.SetActive(false);
    }

    public void activate()
    {
        clipboardButton.SetActive(true);
    }

}

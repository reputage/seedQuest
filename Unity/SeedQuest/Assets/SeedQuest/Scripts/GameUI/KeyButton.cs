using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MonoBehaviour {

    public int keyIndex;
    public string keyName;
    public string menuMode;

    void Start () 
    {
        menuMode = "";
    }

    // finds the key name for this button
    void getKeyName(Dictionary<string, string> userDids)
    {
        int i = 1;

        // This causes an object reference error for some reason
        if (userDids.Count == 0)
        {
            keyName = "";
            return;
        }
        foreach (KeyValuePair<string, string> entry in userDids)
        {
            if (i == keyIndex)
            {
                keyName = entry.Key;
                return;
            }
            i++;
        }
        keyName = "";
    }

    // Return true if no key name is set, otherwise return false
    public bool isEmpty(Dictionary<string, string> userDids)
    {
        getKeyName(userDids);
        if (keyName == "")
            return true;
        else
            return false;
    }

    // Set the seed that the user will practice, and begin rehearse mode
    public void rehearsalStart()
    {
        if (keyName != "" && menuMode == "learn")
        {
            Debug.Log("Key name: " + keyName + " returns user seed value: " + DideryDemoManager.UserSeeds[keyName] + " Input seed in SeedManager: " + SeedManager.InputSeed);
            SeedManager.InputSeed = DideryDemoManager.UserSeeds[keyName]; 
            GameManager.State = GameState.Rehearsal; 
        }
    }

    // Set the did that will be retrieved from didery, and begin recall mode
    public void recallStart()
    {
        if (keyName != "" && menuMode == "recover")
        {
            Debug.Log("Key name: " + keyName + " returns user did value: " + DideryDemoManager.UserDids[keyName] + " Demo blob in DideryDemoManager: " + DideryDemoManager.DemoBlob);
            DideryDemoManager.DemoDid = DideryDemoManager.UserDids[keyName];
            GameManager.State = GameState.Recall; 
        }
    }

    // set which menu the button is acting for
    public void setMenuMode(string menuType)
    {
        menuMode = menuType;
    }

    // start the game differently depending on which menu the user is in
    public void startGame()
    {
        if(menuMode == "learn")
        {
            rehearsalStart();
        }
        else if (menuMode == "recover")
        {
            recallStart();
        }
    }

}

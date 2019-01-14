using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MonoBehaviour {

    public int keyIndex;
    public string keyName;

    void Start () 
    {

    }

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
        if (keyName != "")
        {
            Debug.Log("Key name: " + keyName + " returns user seed value: " + DideryDemoManager.UserSeeds[keyName] + " Input seed in SeedManager: " + SeedManager.InputSeed);
            SeedManager.InputSeed = DideryDemoManager.UserSeeds[keyName]; 
        }
        GameManager.State = GameState.Rehearsal; 
    }

    // Set the did that will be retrieved from didery, and begin recall mode
    public void recallStart()
    {
        if (keyName != "")
        {
            Debug.Log("Key name: " + keyName + " returns user seed value: " + DideryDemoManager.UserSeeds[keyName] + " Input seed in SeedManager: " + SeedManager.InputSeed);
            DideryDemoManager.DemoDid = DideryDemoManager.UserSeeds[keyName];
        }
        GameManager.State = GameState.Recall; 
    }

}

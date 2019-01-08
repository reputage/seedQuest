using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MonoBehaviour {

    public int keyIndex;
    public string keyName;
    //public DideryDemoManager dideryDemoManager;

    void Start () 
    {
        //getKeyName();
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
        if(keyName != "")
            SeedManager.InputSeed = DideryDemoManager.UserSeeds[keyName];
        GameManager.State = GameState.Rehearsal;
    }

    // Set the did that will be retrieved from didery, and begin recall mode
    public void recallStart()
    {
        if (keyName != "")
        {
            DideryDemoManager.DemoDid = DideryDemoManager.UserSeeds[keyName];
        }
        GameManager.State = GameState.Recall;
    }

}

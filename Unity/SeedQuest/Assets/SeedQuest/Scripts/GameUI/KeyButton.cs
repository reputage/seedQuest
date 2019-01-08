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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MonoBehaviour {

    public int keyIndex;
    public string keyName;
    public

    void Start () 
    {
        //getKeyName();
	}
	
    void getKeyName()
    {
        int i = 1;
        foreach (KeyValuePair<string, string> entry in DideryDemoManager.UserDids)
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

    public bool isEmpty()
    {
        getKeyName();
        if (keyName == "")
            return true;
        else
            return false;
    }

}

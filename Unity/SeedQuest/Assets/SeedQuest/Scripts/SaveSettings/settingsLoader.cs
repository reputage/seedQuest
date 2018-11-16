using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsLoader : MonoBehaviour {

    // This could be included in a different script, but I'm giving
    //  it it's own script for the purpose of encapsulation

	void Start () 
    {
        SaveSettings.loadSettings();
	}
	
}

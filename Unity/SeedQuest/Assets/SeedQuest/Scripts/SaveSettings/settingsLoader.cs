using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsLoader : MonoBehaviour {

    // This could be included in a different script if we want

	void Start () 
    {
        SaveSettings.loadSettings();
	}
	
}

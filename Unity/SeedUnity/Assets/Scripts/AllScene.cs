using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllScene : MonoBehaviour {

    static int seedParsed;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        seedParsed = 0;
	}

    public void seedChange(int newParsed){
        seedParsed = newParsed;
    }

    public int getSeed(){
        return seedParsed; 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllScene : MonoBehaviour {

    static int seedParsed;
    static bool scenePrimed = false;
    //private float sceneVal;

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
        
        if (Input.GetAxis("Fire1") > 0)
        {
            scenePrimed = true;
            Debug.Log("Scene primed");
        }
        //sceneVal = Input.GetAxis("SceneHack");

        if (scenePrimed == true && Input.GetAxis("SceneHack") > 0)
        {
            scenePrimed = false;
            Debug.Log("Transitioning to scene 0...");
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else if (scenePrimed == true && Input.GetAxis("SceneHack") < 0)
        {
            scenePrimed = false;
            Debug.Log("Transitioning to scene 1...");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
	}
}

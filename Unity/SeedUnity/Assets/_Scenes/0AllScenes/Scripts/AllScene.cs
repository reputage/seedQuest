using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllScene : MonoBehaviour {

    static int seedParsed;
    static bool scenePrimed = false;
    //static bool pauseEverything = false;
    //private float sceneVal;


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

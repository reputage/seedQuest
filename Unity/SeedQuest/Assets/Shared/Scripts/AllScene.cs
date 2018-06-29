using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllScene : MonoBehaviour {

    /* 
       This script and it's associated object are mean to be in every scene.
       This is meant to be used for things that need to stay constant across 
       all scenes, but don't belong in another object or script.
    */
      
     
    static int seedParsed;
    //static bool scenePrimed = false;
    public static int locationID = 0;
    //public static Vector3 outdoorLocation;

	void Start () {
        //DontDestroyOnLoad(gameObject);
        seedParsed = 0;
	}

    public void seedChange(int newParsed){
        seedParsed = newParsed;
    }

    public int getSeed(){
        return seedParsed; 
    }
	
	void Update () {

        /*
        if (Input.GetAxis("Fire1") > 0)
        {
            scenePrimed = true;
            Debug.Log("Scene primed");
        }

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
        */
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllScene : MonoBehaviour {

    static int seedParsed;
    static bool scenePrimed = false;
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
	}

    /*
    public void setLocation(Vector3 playerLoc)
    {
        outdoorLocation = playerLoc;
    }
    */
}

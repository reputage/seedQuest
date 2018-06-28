using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionOperator : MonoBehaviour {

    /* 
       This script is meant to be used in handling some menus 
       and other UI elements in the game. 
    */


    public GameObject PauseMenu;
    public GameObject player;
    public GameObject actionSpot;
    public GameObject itemPrompt;
    public GameObject entrancePrompt;
    public GameObject camera;

	// Use this for initialization
	void Start () {
        PauseMenu.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Activates UI elements for item prompts
    public void activateSpot()
    {
        itemPrompt.gameObject.SetActive(true);
    }

    // Deactivates UI elements for item prompts
    public void deactivateSpot()
    {
        itemPrompt.gameObject.SetActive(false);
    }

    // Activates pause menu, stops camera from moving
    public void activatePause()
    {
        PauseMenu.gameObject.SetActive(true);
        camera.GetComponent<ThirdPersonCamera>().activatePause();
    }

    // Deactivates pause menu, lets camera move again
    public void deactivatePause()
    {
        PauseMenu.gameObject.SetActive(false);
        camera.GetComponent<ThirdPersonCamera>().deactivatePause();
    }

    // Activates UI elements for entrance prompts
    public void activateEntrance()
    {
        entrancePrompt.gameObject.SetActive(true);
    }

    // Deactivates UI elements for entrance prompts
    public void deactivateEntrance()
    {
        entrancePrompt.gameObject.SetActive(false);
    }

    public void quitButton()
    {
        // Quit the game
        Application.Quit();
    }

}

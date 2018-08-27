using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapObjects : MonoBehaviour {

    public GameObject childObjects;
    public GameStateData gameState;

	// Use this for initialization
	void Start () 
    {
        deactivate();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (gameState.inRehersalMode && gameState.isStarted)
        {
            activate();
        }
        else
        {
            deactivate();    
        }
	}

    void deactivate()
    {
        childObjects.SetActive(false);
        Debug.Log("Deactivating minimap");
    }

    void activate()
    {
        childObjects.SetActive(true);
        Debug.Log("Activating minimap");
    }

}

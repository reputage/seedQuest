using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapObjects : MonoBehaviour {

    public GameObject childObjects;
    public GameStateData gameState;

	void Start () 
    {
        deactivate();
    }
	
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
    }

    void activate()
    {
        childObjects.SetActive(true);
    }

}

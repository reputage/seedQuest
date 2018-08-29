using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compassObjects : MonoBehaviour {

    public GameObject childObjects;
    public GameStateData gameState;

    private bool checking;

	// Use this for initialization
	void Start () 
    {
        deactivate();
        checking = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (checking)
        {
            if (gameState.inRehersalMode && gameState.isStarted)
            {
                activate();
                checking = false;
            }
            else
            {
                deactivate();
            }
        }
    }


    void activate()
    {
        childObjects.SetActive(true);
    }

    void deactivate()
    {
        childObjects.SetActive(false);
    }

}

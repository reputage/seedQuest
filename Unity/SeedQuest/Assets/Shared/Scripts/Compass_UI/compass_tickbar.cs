using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass_tickbar : MonoBehaviour {

    public GameObject player;

    Vector3 forward;

    public int pixelLoop;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    private void initialize()
    { 
        forward = transform.forward;

    }

}

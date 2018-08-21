using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudGenerator : MonoBehaviour {

    public int cloudIter;

	// Use this for initialization
	void Start () {
        cloudIter = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        cloudIter += 1;
		
	}
}

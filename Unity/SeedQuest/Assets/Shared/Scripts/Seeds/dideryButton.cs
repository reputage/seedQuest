using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dideryButton : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void dideryGetTest() 
    {
        StartCoroutine(DideryInterface.GetRequest("http://localhost:8080/blob/"));
    }

}

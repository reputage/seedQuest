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
        StartCoroutine(DideryInterface.GetRequest("http://localhost:8080/blob"));
        StartCoroutine(DideryInterface.GetRequest("http://localhost:8080/blob/did:dad:fKymS-dKgO3YlfwF5XdCXfx79UN1X22bsM3u9KRxXhY="));
    }


    public void parseBlob()
    {


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class dideryButton : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void dideryGetTest() 
    {
        //StartCoroutine(DideryInterface.GetRequest("http://localhost:8080/blob"));
        StartCoroutine(DideryInterface.GetRequest("http://localhost:8080/blob/did:dad:fKymS-dKgO3YlfwF5XdCXfx79UN1X22bsM3u9KRxXhY="));
    }


    public void dideryPostTest()
    {
        string json = "json";
        string signer = "signer=\"lMAxMfKQFKthpHjTxSP4EBOwPE3nhylt_-wZAJqEWfNNa3i7LiudIFQt9LS_G6W_14aGaGWVFxY3zQPu_pO1AQ==\"";
        StartCoroutine(DideryInterface.PostRequest("http://localhost:8080/blob", json, signer));

    }

    public void dideryPostTest2()
    {
        string[] data = new string[3];
        string key = "aj;skljfjasisfjweoijfaiajfo;iavjvjncowrnoiarejnfoj;csacivnfgo;afiewvajdfvo;hnafddjio;ahjfgoia;ehroi;hs";
        string did = "";
        string signature = "";
        string postBody = "";
        string url = "http://localhost:8080/blob";

        byte[] byteKey = System.Text.Encoding.ASCII.GetBytes(key);

        data = DideryInterface.makePost(byteKey);

        did = data[0];
        signature = data[1];
        postBody = data[2];

        Debug.Log("url: " + url);
        Debug.Log("post body: " + postBody);
        Debug.Log("signature: " + signature);

        StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    public void parseBlob()
    {
        string nonParsed = DideryInterface.getResult;

    }

}

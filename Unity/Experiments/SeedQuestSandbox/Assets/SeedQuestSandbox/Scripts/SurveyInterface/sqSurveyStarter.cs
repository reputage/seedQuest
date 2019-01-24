using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sqSurveyStarter : MonoBehaviour {


	private void Start()
	{
        sendRequestData();
	}

	public void sendRequestData()
    {
        string textResponse = "Hello from unity!";

        Debug.Log("Starting Request.");
        StartCoroutine(sqSurveyInterface.postRequest(textResponse));
        Debug.Log("Request Finished.");
    }

}

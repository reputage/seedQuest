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
        Debug.Log("Starting Request.");
        StartCoroutine(sqSurveyInterface.postRequest());
        Debug.Log("Request Finished.");
    }

}

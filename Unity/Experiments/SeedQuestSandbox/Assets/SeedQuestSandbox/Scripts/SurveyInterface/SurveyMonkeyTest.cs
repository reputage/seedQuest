using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyMonkeyTest : MonoBehaviour {


	private void Start()
	{
        sendRequestData();
	}

	public void sendRequestData()
    {
        Debug.Log("Starting Request.");
        StartCoroutine(HttpTest.postRequest());
        Debug.Log("Request Finished.");
    }

}

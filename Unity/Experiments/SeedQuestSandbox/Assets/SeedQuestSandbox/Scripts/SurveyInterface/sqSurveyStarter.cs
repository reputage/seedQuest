using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sqSurveyStarter : MonoBehaviour {


	private void Start()
	{
        testRequestData();
	}

    public void postTestList()
    {
        List<string> questions = new List<string>();
        List<string> responses = new List<string>();

        questions.Add("q1");
        questions.Add("q2");
        questions.Add("q3");

        responses.Add("r1");
        responses.Add("r2");
        responses.Add("r3");

        sendRequestData(questions, responses);
    }

    // Send survey data to the server
    public void testRequestData()
    {
        //string textResponse = "Hello from unity!";

        Debug.Log("Starting Request.");
        StartCoroutine(sqSurveyInterface.testPostRequest());
        Debug.Log("Request Finished.");
    }

    // Send survey data to the server
    public void sendRequestData(List<string> questions, List<string> responses)
    {
        //string textResponse = "Hello from unity!";
        string serverUrl = "http://178.128.0.208:8080/";

        Debug.Log("Starting Request.");
        StartCoroutine(sqSurveyInterface.postRequest(questions, responses, serverUrl));
        Debug.Log("Request Finished.");
    }

}

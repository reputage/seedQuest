using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyDataHandler : MonoBehaviour 
{

    // Send POST request to survey backend
    public void postRequest(string url, string apiHeader, string apiToken, string surveyId, byte[]fileBytes)
    {
        StartCoroutine(SurveyInterface.PostRequest(url, apiHeader, apiToken, surveyId, fileBytes));
    }

}

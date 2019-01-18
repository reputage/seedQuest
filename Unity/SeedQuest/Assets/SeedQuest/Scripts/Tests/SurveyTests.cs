using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyTests : MonoBehaviour {

    private string apiHeader = "X-API-TOKEN";
    private string apiToken = "1234";
    private string expectsHeader = "expects:";
    private string expectsData = "1234";
    private string url = "http/stuff";
    private string surveyId = "surveyID12345";
    private byte[] fileBytes;

    SurveyDataHandler surveyDataHandler = new SurveyDataHandler();

    public void runTests()
    {
        testPostRequest();
    }

    private void testPostRequest()
    {
        surveyDataHandler.postRequest(url, apiHeader, apiToken, surveyId, fileBytes);
    }

}

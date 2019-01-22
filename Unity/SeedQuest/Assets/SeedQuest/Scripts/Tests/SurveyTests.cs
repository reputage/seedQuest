using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyTests : MonoBehaviour {

    public string apiHeader = "X-API-TOKEN";
    public string apiToken = "...";
    public string expectsHeader = "Expects:";
    public string expectsData = "1234";
    public string url = "https://...";
    public string surveyId = "...";
    public string testCsv = "Question1,answer1";
    public byte[] fileBytes;

    public void runTests()
    {
        testPostRequest();
    }

    private void testPostRequest()
    {
        //fileBytes = Encoding.UTF8.GetBytes(testCsv);
        fileBytes = new System.Text.UTF8Encoding().GetBytes(testCsv);
        StartCoroutine(SurveyInterface.PostRequest(url, apiHeader, apiToken, surveyId, fileBytes));
    }

}

using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyTests : MonoBehaviour {

    public string apiHeader = "X-API-TOKEN";
    public string apiToken = "2dDzDzOcKy5muXvFITA6dvyz9nvtmeefks8fgKpu";
    public string expectsHeader = "Expects:";
    public string expectsData = "1234";
    public string url = "https://co1.qualtrics.com/API/v3/responseimports";
    public string surveyId = "SV_eWZOGc8YXsdaqTb";
    public string testCsv = "Question1,answer1";
    public byte[] fileBytes;

    //'X-API-TOKEN: 2dDzDzOcKy5muXvFITA6dvyz9nvtmeefks8fgKpu' -H "Expects:" -F 'surveyId=SV_eWZOGc8YXsdaqTb' 
    // -F 'file=@testSurveyResponse.csv;type=text/csv' 'https://co1.qualtrics.com/API/v3/responseimports'

    //SurveyDataHandler surveyDataHandler = new SurveyDataHandler();

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

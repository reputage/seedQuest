using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class SurveyInterface 
{
    // Send Post request to survey API
    public static IEnumerator PostRequest(string url, string apiHeader, string apiToken, string surveyId, byte[] csvFile)
    {

        byte[] surveyBytes = new System.Text.UTF8Encoding().GetBytes(surveyId);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("surveyId", surveyBytes));
        formData.Add(new MultipartFormFileSection("file", csvFile));

        UnityWebRequest uwr = UnityWebRequest.Post(url, formData);

        uwr.chunkedTransfer = false;

        uwr.SetRequestHeader(apiHeader, apiToken);
        uwr.SetRequestHeader("Expects", "");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

}

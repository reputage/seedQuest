using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class SurveyInterface 
{
    // Send Post request to survey API
    public static IEnumerator PostRequest(string url, string apiHeader, string apiToken, string surveyId, byte[] csvFile)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("surveyId", surveyId));
        formData.Add(new MultipartFormFileSection("file:", csvFile));
        formData.Add(new MultipartFormDataSection("content-type", "text/csv"));

        UnityWebRequest uwr = UnityWebRequest.Post(url, formData);
        uwr.SetRequestHeader(apiHeader, apiToken);

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

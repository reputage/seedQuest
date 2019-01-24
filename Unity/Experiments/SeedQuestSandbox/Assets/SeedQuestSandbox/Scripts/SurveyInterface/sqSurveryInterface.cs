using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class sqSurveyInterface
{

    public static IEnumerator postRequest(string url=null, string textResponse=null)
    {
        if (url==null)
            url = "http://localhost:8080/surveys";

        string json = jsonBodyBuilder("hello from unity");

        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }


    public static string jsonBodyBuilder(string textResponseOne)
    {
        string dateTime = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
        string testName = "xyz";
        string testEmail = "xyz@domain.com";

        Debug.Log("Date: " + dateTime);

        string body;

        body = "{";

        body += "\"Name\": \"" + testName + "\",";
        body += "\"Email\": \"" + testEmail + "\",";
        body += "\"Response\": \"" + textResponseOne + "\"";

        body += "}";
        Debug.Log("Json body: " + body);

        return body;
    }


    public static string responseFormatter(string questionId, string userResponse)
    {
        string json = "\"" + questionId + "\": \"" + userResponse + "\"";
        return json;
    }

    public static string groupResponses(List<string> questions, List<string> responses)
    {
        if (questions.Count != responses.Count)
        {
            Debug.Log("Error: insufficient responses for number of questions");
            return "";
        }

        string json = "{";

        for (int i = 0; i < questions.Count; i++)
        {
            json += responseFormatter(questions[i], responses[i]);
            if (i + 1 != questions.Count)
                json += ",";
        }

        Debug.Log("Json group formatted: " + json);

        return json;
    }


}
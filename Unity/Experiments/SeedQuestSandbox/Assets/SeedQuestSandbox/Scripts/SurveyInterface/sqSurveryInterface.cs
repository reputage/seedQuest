using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class sqSurveyInterface
{

    // POSTs the survey response data to the server. Will need to have the URL changed eventually
    //  used for testing purposes
    public static IEnumerator testPostRequest(string url = null, string textResponse = null)
    {
        if (url == null)
            url = "http://localhost:8000/surveys";

        Debug.Log("url: " + url);

        List<string> questions = new List<string>();
        List<string> responses = new List<string>();

        questions.Add("q1");
        questions.Add("q2");
        responses.Add("r1");
        responses.Add("r2");

        string json = jsonBodyBuilder(questions, responses);

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

    // POSTs the survey response data to the server. Will need to have the URL changed eventually
    public static IEnumerator postRequest(List<string> questions, List<string> responses, string url = null)
    {
        if (url == null)
            url = "http://178.128.0.208:8000/surveys";

        string json = jsonBodyBuilder(questions, responses);

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

    // Sends a GET request to the survey server - probably not needed within Unity
    public static IEnumerator getRequest(string url = null)
    {
        string getResult;
        if (url == null)
            url = "http://localhost:8080/surveys";

        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        getResult = uwr.downloadHandler.text;

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        string[] getData = getResult.Split(':');

    }

    // I'm not 100% sure what the final survey will look like, but here's a preliminary 
    //  function for formatting the JSON for the POST request
    public static string jsonBodyBuilder(List<string> questions, List<string> responses, string name = null, string email = null)
    {
        string dateTime = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
        string testName;
        string testEmail;

        if (name == null)
            name = "xyz";
        if (email == null)
            email = "xyz@domain.com";

        Debug.Log("Date: " + dateTime);

        string body;
        string textResponseOne;

        textResponseOne = groupResponses(questions, responses);

        body = "{";

        body += "\"Name\": \"" + name + "\",";
        body += "\"Email\": \"" + email + "\",";
        body += "\"Response\": " + textResponseOne;

        body += "}";
        Debug.Log("Json body: " + body);

        return body;
    }

    // Another JSON formatter function. Again, don't know what the survey will look like yet
    public static string responseFormatter(string questionId, string userResponse)
    {
        string json = "\"" + questionId + "\": \"" + userResponse + "\"";
        return json;
    }

    // Another JSON formatter function. Again, don't know what the survey will look like yet
    public static string groupResponses(List<string> questions, List<string> responses)
    {
        if (questions.Count != responses.Count)
        {
            Debug.Log("Warning: insufficient responses for number of questions");
        }

        int maxResponses = Math.Min(questions.Count, responses.Count);
        string json = "{";

        for (int i = 0; i < maxResponses; i++)
        {
            json += responseFormatter(questions[i], responses[i]);
            if (i + 1 != maxResponses)
                json += ",";
        }

        json += "}";
        Debug.Log("Json group formatted: " + json);

        return json;
    }

    // I'm not sure what format the responses will be in - this may be helpful
    public static void addToLists(List<string> questions, List<string> responses, string quetsionToAdd, string responseToAdd)
    {
        questions.Add(quetsionToAdd);
        responses.Add(responseToAdd);
    }


}
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class sqSurveyInterface
{

    public static IEnumerator postRequest(string url=null)
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
        string ipAddress = "127.0.0.1";
        string testName = "xyz";
        string testEmail = "xyz@domain.com";

        Debug.Log("Date: " + dateTime);
        Debug.Log("IP adress: " + ipAddress);

        string body;

        body = "{";

        body += "\"Name\": \"" + testName + "\",";
        body += "\"Email\": \"" + testEmail + "\",";
        body += "\"ipaddress\": \"" + ipAddress + "\",";
        body += "\"Response\": \"" + textResponseOne + "\"";

        body += "}";
        Debug.Log("Json body: " + body);

        return body;
    }

}
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpTest
{

    public static IEnumerator postRequest()
    {
        string surveyHash = "ZBSG26H";
        string surveyId = "165469305";
        string collectorId = "224504125";
        string url = "https://api.surveymonkey.net/v3/collectors/";
        string json = "{}";

        json = jsonBodyBuilder("hello from unity");

        string finalUrl = url + "/" + collectorId + "/responses";

        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", "bearer FEk40QGlfQuAYxU5PtnZhi4XCzwG7ixK18xmRpr4MLQy5cAuR70KnV0DX27glqgHs9j-vd.x8d3qQwwqk-rDz60vYoulBCnrMlW3pShO6tHC7uXMlSoavyTJjdN0DOVT");

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
        dateTime += "+00:00";
        string ipAddress = "127.0.0.1";
        string recipientId = "564728340";
        string pageOneId = "1";
        string rowOneId = "1";
        string questionId = "1";

        Debug.Log("Date: " + dateTime);
        Debug.Log("IP adress: " + ipAddress);

        string body;

        body = "{";
        //body += "\"date_created\": \"" + dateTime + "\", \"ip_address\": \"" + ipAddress + "\",";
        //body += "\"recipient_id\": \"" + recipientId + "\",";
        body += "\"pages\": [{\"id\": \"" + pageOneId +"\",";
        body += "\"questions\": [{\"answers\": [{";
        body += "\"text\": \"" + textResponseOne + "\"}],";
        body += "\"id\": \"" + questionId + "\"}]}]}";

        Debug.Log("Json body: " + body);

        return body;
    }

    // 1 date created
    // 2 ip address
    // 3 recipient id
    // 4 pages
    // 5 id
    // 6 answers - row id, choice id, id or text + id

}
    /*

    request.Content = new StringContent("" +
                                        "{\"custom_variables\":" +
                                        "{\"custvar_1\": \"one\", \"custvar_2\": \"two\"}," +

                                        "\"response_status\": \"overquota\"," +
                                        "\"custom_value\": " + "\"custom identifier for the response\"," +
                                        "\"date_created\": " + "\"2015-10-06T12:56:55+00:00\"," +
                                        "\"ip_address\": \"127.0.0.1\"," +

                                        "\"recipient_id\": \"564728340\", " +
                                        "\"pages\": " +
                                        "[{\"id\": \"12345678\"," +
                                        "\"questions\": " +
                                        " [{\"answers\": " +
                                        "[{\"choice_id\": \"12345678\"}], " +
                                        "\"id\": \"12345678\"}," +
                                        " {\"answers\": " +
                                        "[{\"row_id\": \"12345678\", " +
                                        "\"choice_id\": \"12345678\"}], " +
                                        "\"id\": \"12345678\"}, " +
                                        "{\"answers\": " +
                                        "[{\"row_id\": \"12345678\", " +
                                        "\"col_id\": \"12345678\", " +
                                        "\"choice_id\": \"12345678\"}], " +
                                        "\"id\": \"12345678\"}," +
                                        "{\"answers\": " +
                                        "[{\"text\": \"Sample Text Response\"}], " +
                                        "\"id\": \"12345678\"}, " +
                                        "{\"answers\": " +
                                        "[{\"row_id\": \"12345678\", " +
                                        "\"text\": \"Sample Text Response\"}]," +
                                        " \"id\": \"12345678\"}]}]}", Encoding.UTF8, "application/json");
*/


/*
using (var httpClient = new HttpClient())
{
    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.surveymonkey.com/v3/surveys/{survey_id}/responses"))
    {
        request.Headers.TryAddWithoutValidation("Authorization", "bearer YOUR_ACCESS_TOKEN"); 

        request.Content = new StringContent("{\"custom_variables\":{\"custvar_1\": \"one\", \"custvar_2\": \"two\"},\"response_status\": \"overquota\",\"custom_value\": \"custom identifier for the response\",\"date_created\": \"2015-10-06T12:56:55+00:00\",\"ip_address\": \"127.0.0.1\",\"recipient_id\": \"564728340\", \"pages\": [{\"id\": \"12345678\",\"questions\": [{\"answers\": [{\"choice_id\": \"12345678\"}], \"id\": \"12345678\"}, {\"answers\": [{\"row_id\": \"12345678\", \"choice_id\": \"12345678\"}], \"id\": \"12345678\"}, {\"answers\": [{\"row_id\": \"12345678\", \"col_id\": \"12345678\", \"choice_id\": \"12345678\"}], \"id\": \"12345678\"}, {\"answers\": [{\"text\": \"Sample Text Response\"}], \"id\": \"12345678\"}, {\"answers\": [{\"row_id\": \"12345678\", \"text\": \"Sample Text Response\"}], \"id\": \"12345678\"}]}]}", Encoding.UTF8, "application/json");

var response = await httpClient.SendAsync(request);
    }
}


request.Content = new StringContent("{\"custom_variables\":{\"custvar_1\": \"one\", 
\"custvar_2\": \"two\"},\"response_status\": \"overquota\",\"custom_value\": 
\"custom identifier for the response\",\"date_created\": \"2015-10-06T12:56:55+00:00\",
\"ip_address\": \"127.0.0.1\",\"recipient_id\": \"564728340\", \"pages\": [{\"id\": 
\"12345678\",\"questions\": [{\"answers\": [{\"choice_id\": \"12345678\"}], \"id\": 
\"12345678\"}, {\"answers\": [{\"row_id\": \"12345678\", \"choice_id\": \"12345678\"}], 
\"id\": \"12345678\"}, {\"answers\": [{\"row_id\": \"12345678\", \"col_id\": \"12345678\", 
\"choice_id\": \"12345678\"}], \"id\": \"12345678\"}, {\"answers\": [{\"text\": 
\"Sample Text Response\"}], \"id\": \"12345678\"}, {\"answers\": [{\"row_id\": \"12345678\", \"text\": \"Sample Text Response\"}], 
\"id\": \"12345678\"}]}]}", Encoding.UTF8, "application/json");


*/
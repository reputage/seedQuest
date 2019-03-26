using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sqSurveyStarter : MonoBehaviour
{

    public SurveyData surveyData;

    private void Start()
    {
        //testRequestData();
    }

    // test post request with the actual server
    public void postTestList()
    {
        List<string> questions = new List<string>();
        List<string> responses = new List<string>();

        questions.Add("q1");
        questions.Add("q2");
        questions.Add("q3");

        responses.Add("r1");
        responses.Add("r2");
        responses.Add("r3");

        sendRequestData(questions, responses);
    }

    // test the get questions function
    public void testGetSurveyData()
    {
        List<string> questions = getQuestionsFromSurvey(surveyData);
        List<string> responses = getAnswersFromSurvey(surveyData);

        for (int i = responses.Count - 1; i < questions.Count - 1; i++)
        {
            responses.Add(" ");
        }

        for (int i = 0; i < questions.Count; i++)
            Debug.Log("Question " + i + ": " + questions[i]);

        for (int i = 0; i < responses.Count; i++)
            Debug.Log("Answer " + i + ": " + responses[i]);

        sendRequestData(questions, responses);
    }

    // Send survey data to the server
    public void testRequestData()
    {
        StartCoroutine(sqSurveyInterface.testPostRequest());
    }

    // Send survey data to the server
    public void sendRequestData(List<string> questions, List<string> responses)
    {
        //string textResponse = "Hello from unity!";
        string serverUrl = "http://178.128.0.208:8080/surveys";

        StartCoroutine(sqSurveyInterface.postRequest(questions, responses, serverUrl));
    }

    // Get the questions from the scriptable object
    public List<string> getQuestionsFromSurvey(SurveyData data)
    {
        List<string> questions = new List<string>();
        for (int i = 0; i < data.surveyData.Count; i++)
        {
            if (data.surveyData[i].questions.Length > 0)
            {
                // add all questions
                for (int j = 0; j < data.surveyData[i].questions.Length; j++)
                {
                    string comboQuestion = data.surveyData[i].question + "-" + data.surveyData[i].questions[j];
                    questions.Add(comboQuestion);
                }
            }
            else
            {
                questions.Add(data.surveyData[i].question);
            }
        }
        return questions;
    }

    // to do - front end is still in progress?
    public List<string> getAnswersFromSurvey(SurveyData data)
    {
        List<string> responses = new List<string>();

        for (int i = 0; i < data.surveyData.Count; i++)
        {
            if (data.surveyData[i].answers.Length > 0)
            {
                // add all questions
                for (int j = 0; j < data.surveyData[i].answers.Length; j++)
                {
                    string answer = data.surveyData[i].answers[j];
                    responses.Add(answer);
                }
            }
            else
            {
                responses.Add(data.surveyData[i].answer);
            }
        }
        return responses;
    }

    public void debugBodyBuilder()
    {
        List<string> questions = new List<string>();
        List<string> responses = new List<string>();

        questions.Add("q1");
        questions.Add("q2");

        responses.Add("r1");
        responses.Add("r2");

        string json = sqSurveyInterface.jsonBodyBuilder(questions, responses);
    }
}

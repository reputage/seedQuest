using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SurveyDataItem {
    public enum QuestionType { Open, Scale } //, Rank }
    public QuestionType type = QuestionType.Open;
    public string question;
    public string[] headers;
    public string[] questions;
    /*public int scaleStart = 1;
    public int scaleDefault = 3;
    public int scaleStop = 5;
    public List<string> ranks;*/
    public string answer;
    public string[] answers;
}

[CreateAssetMenu(menuName = "Survey/SurveyData")]
public class SurveyData : ScriptableObject {
    public List<SurveyDataItem> surveyData;

}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class SurveyDataItem {
    public enum QuestionType { Open, Scale, Rank }
    public QuestionType type = QuestionType.Open;
    public string question;
    public int scaleStart = 1;
    public int scaleDefault = 3;
    public int scaleStop = 5;
    public List<string> ranks;
    public string answer;
}

[CreateAssetMenu(menuName = "Survey/SurveyData")]
public class SurveyData : ScriptableObject {
    public List<SurveyDataItem> surveyData;

}
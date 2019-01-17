using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SurveyDataItem {
    public string question;
    public string answer;
}

[CreateAssetMenu(menuName = "Survey/SurveyData")]
public class SurveyData : ScriptableObject {

    public List<SurveyDataItem> surveyData;

} 
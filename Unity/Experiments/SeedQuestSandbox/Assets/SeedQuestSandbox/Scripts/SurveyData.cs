using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SurveyDataItem {
    public string question;
    public string questionType;
    public string answer;
}

[CreateAssetMenu(menuName = "Survey/SurveyData")]
public class SurveyData : ScriptableObject {

    public List<SurveyDataItem> surveyData;

}

/*[CustomEditor(typeof(SurveyData)), CanEditMultipleObjects]
public class SurveyEditor : Editor
{
    public override void OnInspectorGUI()
    {

    }
}*/
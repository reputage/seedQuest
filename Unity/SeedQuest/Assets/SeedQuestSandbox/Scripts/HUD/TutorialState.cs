using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialState : MonoBehaviour
{
    private static TutorialState instance = null;
    public static TutorialState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TutorialState>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        instance = null;
    }

    public static bool skip = false;
    public static bool Skip
    {
        get { return skip; }
        set { if (value == skip) return; skip = value; }
    }
}

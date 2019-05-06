using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Level;

public class LevelClearUI : MonoBehaviour {
    
    static private LevelClearUI instance = null;
    static private LevelClearUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<LevelClearUI>(true); return instance; }
    static public LevelClearUI Instance { get { return instance == null ? setInstance() : instance; } }

    static public void ToggleOn() {
        if (Instance.gameObject.activeSelf)
            return;

        Instance.gameObject.SetActive(true);
    }

    public void GoToSceneSelect() {
        //LevelManager.GoToSceneSelect();
        MenuScreenManager.ActivateSceneLineUp();
        gameObject.SetActive(false);
    }
}
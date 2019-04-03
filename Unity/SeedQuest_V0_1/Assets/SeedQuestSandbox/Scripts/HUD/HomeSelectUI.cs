using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using SeedQuest.Level;

public class HomeSelectUI : MonoBehaviour {
    
    public void GoToSceneSelect() {
        LoadingScreenUI.LoadScene(LevelManager.LevelSelectScene, false);
    }
}
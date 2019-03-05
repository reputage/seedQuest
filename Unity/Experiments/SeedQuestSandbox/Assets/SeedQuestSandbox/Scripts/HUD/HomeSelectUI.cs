using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSelectUI : MonoBehaviour {

    public string sceneName = "SceneSelect";

    private void Update()
    {
        ListenForKeyDown();
    }

    public void ListenForKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void GoToSceneSelect()
    {
        SceneManager.LoadScene(sceneName);
    }
}
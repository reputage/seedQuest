using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    static private SceneLoader __instance = null;
    static public SceneLoader instance
    {
        get
        {
            if (__instance == null)
                __instance = SceneLoader.FindObjectOfType<SceneLoader>();
            return __instance;
        }
    }


    static public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    static public IEnumerator LoadGame() {
        //SceneManager.LoadScene("Demo_v05");
        AsyncOperation async = SceneManager.LoadSceneAsync("Demo_v05");
        while(!async.isDone) {
            yield return null;
        }   
    }

    static public IEnumerator LoadMainMenu() {
        //SceneManager.LoadScene("MainMenu_v02");
        AsyncOperation async = SceneManager.LoadSceneAsync("MainMenu_v02");
        while(!async.isDone) {
            yield return null;
        }  
    }
}

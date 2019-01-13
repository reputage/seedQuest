using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour {
    
    public void GoToSceneSelect() {
        SceneManager.LoadScene("SceneSelect");
    }

}

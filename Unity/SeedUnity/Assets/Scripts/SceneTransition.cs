using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    void OnMouseUp()
    {
        SceneManager.LoadScene("procedural");
    }
}

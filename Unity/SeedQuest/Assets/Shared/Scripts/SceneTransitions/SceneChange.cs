using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour
{

    //public Fade fade;
    public GameObject fadePanelPrefab;
    public GameObject fadePanel;
    private bool one = true;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        GameObject fadePanel123 = (GameObject)Instantiate(fadePanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        fadePanel = fadePanel123;
        fadePanel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
        fadePanel.SetActive(true);

    }

    // These fade transitions need to be executed as coroutines, due to the way 
    //  Unity is built. Fade transitions could be programmed differently
    //  using the update() function, but I also wanted an excuse to 
    //  learn how coroutines work.
    private void Update()
    {
        if (one)
        {
            StartCoroutine(fadeIn(fadePanel));
        }
        one = false;
	}

    // Change the scene, but fade out firts
	public void sceneChange(int sceneNum)
    {
        StartCoroutine(fadeOut(fadePanel, sceneNum));
    }

    // Actually execute the scene change
    public void loadScene(int sceneNum)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNum);
    }

    // Coroutine for the fade in animation
    IEnumerator fadeIn(GameObject panel)
    {
        Debug.Log("Fading in...");
        panel.GetComponent<CanvasRenderer>().SetAlpha(255);
        float alphaVal = 255;

        while (alphaVal > 0) 
        {
            //Debug.Log("Alpha : " + alphaVal);
            alphaVal -= (Time.deltaTime + 12);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaVal / 255);
            yield return null;
        }

        panel.GetComponent<CanvasRenderer>().SetAlpha(0);
        panel.SetActive(false);
        yield return null;
    }

    // coroutine for the fade out animation
    IEnumerator fadeOut(GameObject panel, int sceneNum)
    {
        panel.SetActive(true);
        Debug.Log("Fading out...");
        panel.GetComponent<CanvasRenderer>().SetAlpha(0);
        float alphaVal = 0;
        while (alphaVal < 255)
        {
            //Debug.Log("Alpha : " + alphaVal);
            alphaVal += (Time.deltaTime + 17);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaVal / 255);
            yield return null;
        }
        Debug.Log(panel.GetComponent<CanvasRenderer>().GetAlpha());
        panel.GetComponent<CanvasRenderer>().SetAlpha(1);
        loadScene(sceneNum);
    }

}
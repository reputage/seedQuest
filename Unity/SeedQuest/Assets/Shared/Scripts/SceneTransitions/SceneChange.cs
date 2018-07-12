using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneChange : MonoBehaviour
{

    //public Fade fade;
    public GameObject fadePanelPrefab;
    public GameObject fadePanel;
    private bool one = true;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameObject fadePanel123 = (GameObject)Instantiate(fadePanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        fadePanel = fadePanel123;
        fadePanel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
        fadePanel.SetActive(true);

    }

    private void Update()
    {
        if (one)
        {
            StartCoroutine(fadeIn(fadePanel));
        }
        one = false;

	}

	void sceneChange(int sceneNum)
    {
        fadeOut(fadePanel);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNum);
        fadeIn(fadePanel);
    }

    IEnumerator fadeIn(GameObject panel)
    {
        Debug.Log("Fading in...");
        panel.GetComponent<CanvasRenderer>().SetAlpha(255);
        float alphaVal = 255;

        while (alphaVal > 0) 
        {
            Debug.Log("Alpha : " + alphaVal);
            alphaVal -= (Time.deltaTime + 7);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaVal / 255);
            yield return null;
        }

        panel.GetComponent<CanvasRenderer>().SetAlpha(0);
        panel.SetActive(false);
        yield return null;
    }

    public void fadeOut(GameObject panel)
    {
        panel.SetActive(true);
        Debug.Log("Fading out...");
        panel.GetComponent<CanvasRenderer>().SetAlpha(0);
        for (int i = 1; i < 31; i++)
        {
            float alphaVal = 255 / i;
            alphaVal = 255 - alphaVal;
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaVal);
        }
        panel.GetComponent<CanvasRenderer>().SetAlpha(255);

    }

}
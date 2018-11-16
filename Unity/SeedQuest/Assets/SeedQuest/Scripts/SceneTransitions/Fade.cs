using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
    
    //public GameObject fadePanel;


    public void fadeIn(GameObject panel)
    {
        panel.GetComponent<CanvasRenderer>().SetAlpha(255);
        for (int i = 1; i < 31; i ++)
        {
            float alphaVal = 255 / i;
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaVal);
        }
        panel.GetComponent<CanvasRenderer>().SetAlpha(0);
    }

    public void fadeOut(GameObject panel)
    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsTracker : MonoBehaviour
{
    public Text text;
    public float updateInterval = 0.5f;
    public float fps;
    public float deltaFps;
    public float timeLeft;
    public float fpsAverage;
    public float unityEstimate;
    public int frameCounter;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        timeLeft = updateInterval;
    }

    void Update()
    {
        setText(AccumulateFps());
    }

    public void setText(string input)
    {
        text.text = input;
    }

    public string realFps()
    {
        fps = 1.0f / Time.deltaTime;
        return "Actual FPS: " + fps;
    }

    public string avgFps()
    {
        timeLeft -= Time.deltaTime;
        deltaFps += Time.timeScale / Time.deltaTime;
        frameCounter += 1;
        string returnString = "";

        if (timeLeft <= 0f)
        {
            fpsAverage = (deltaFps / frameCounter);
            unityEstimate = (1.29f * deltaFps / frameCounter);
            if ((deltaFps / frameCounter) < 1)
            {
                text.text = "";
            }
            timeLeft = updateInterval;
            deltaFps = 0f;
            frameCounter = 0;
        }

        returnString = "Average FPS: " + fpsAverage.ToString("f2");
        returnString += "\nEditor FPS: " + unityEstimate.ToString("f2");

        return returnString;

    }

    public string AccumulateFps()
    {
        string total = realFps() + "\n" + avgFps();
        return total;
    }

}

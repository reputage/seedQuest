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
    public int frameCounter;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        timeLeft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //realFPS();
        avgFps();
    }

    public void realFPS()
    {
        fps = 1.0f / Time.deltaTime;
        text.text = "FPS: " + fps;
    }

    public void avgFps()
    {
        timeLeft -= Time.deltaTime;
        deltaFps += Time.timeScale / Time.deltaTime;
        frameCounter += 1;

        if (timeLeft <= 0f)
        {
            text.text = "FPS: " + (deltaFps / frameCounter).ToString("f2");
            if ((deltaFps / frameCounter) < 1)
            {
                text.text = "";
            }
            timeLeft = updateInterval;
            deltaFps = 0f;
            frameCounter = 0;
        }
    }

}

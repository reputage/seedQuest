using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectedIndicator : MonoBehaviour
{
    Image[] indicators;
    public bool[] activeNums;

    int counter;

    void Awake()
    {
        indicators = GetComponentsInChildren<Image>();
        foreach (Image img in indicators)
            img.gameObject.SetActive(false);
        activeNums = new bool[indicators.Length];
    }

    // Unfortunately, the scene selected indicators are FUBAR, and for some reason
    //  setting them to be active on every update is necessary to deal with visual bugs
    void Update()
    {
        if (counter > 0)
        {
            setActive();
            counter -= 1;
        }
    }

    public void Activate(int i) {
        activeNums[i] = true;
        counter = 5;
    }

    public void Deactivate(int i ) {
        activeNums[i] = false;
        indicators[i].gameObject.SetActive(false);
        counter = 5;
    }

    public void setActive()
    {
        for (int i = 0; i < activeNums.Length; i++)
        {
            if (activeNums[i])
                indicators[i].gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        foreach (Image img in indicators)
            img.gameObject.SetActive(false);
        for (int i = 0; i < activeNums.Length; i++)
            activeNums[i] = false;
    }
}
    

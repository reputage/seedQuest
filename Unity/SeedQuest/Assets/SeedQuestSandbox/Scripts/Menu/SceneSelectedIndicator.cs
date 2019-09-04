using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectedIndicator : MonoBehaviour
{
    Image[] indicators;

    // Start is called before the first frame update
    void Awake() {
        indicators = GetComponentsInChildren<Image>();
        foreach (Image img in indicators)
            img.gameObject.SetActive(false); 
    }

    public void Activate(int i) {
        indicators[i].gameObject.SetActive(true);
    }

    public void Deactivate(int i ) {
        indicators[i].gameObject.SetActive(false);
    }

    public void Reset()
    {
        foreach (Image img in indicators)
            img.gameObject.SetActive(false);
    }
}
    

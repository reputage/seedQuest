using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapScaling : MonoBehaviour {

	void Start () 
    {
        scale();
	}

    void scale()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x * Screen.width / 1024f, rt.sizeDelta.y * Screen.height / 768f);
        rt.anchoredPosition = new Vector3(rt.anchoredPosition.x * Screen.width / 1024f, rt.anchoredPosition.y * Screen.height / 768f, 1);
    }
}

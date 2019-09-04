using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{

    RectTransform rect;


    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
    }


    public void setScale(float scale)
    {
        rect.localScale *= scale;
    }

    public void resetScale()
    {
        rect.localScale = new Vector3(1, 1, 1);
    }
}

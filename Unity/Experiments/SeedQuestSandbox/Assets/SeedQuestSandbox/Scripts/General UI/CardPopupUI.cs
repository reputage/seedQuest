using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPopupUI : MonoBehaviour
{
    public Color32 primaryColor = new Color32(0x21, 0x96, 0xf3, 0xff);
    public Color32 backgroundColor = new Color32(0xff, 0xff, 0xff, 0xff);
    public Color backdropColor = new Color(0, 0, 0, 0.4f);

    private Image[] images;

    public void Start()
    {
        gameObject.SetActive(false);         // Default popup inactive

        images = GetComponentsInChildren<Image>();
    }

    public void Update()
    {
        images[0].color = backdropColor;
        images[1].color = backgroundColor;
        images[2].color = primaryColor;
    }

    public void toggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
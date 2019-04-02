using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class ActionItem : MonoBehaviour {

    public Image image;
    public TextMeshProUGUI text;
    	
    public void SetItem(int index, string _text, Sprite _image) {

        GameObject textObj = new GameObject("Text");
        GameObject imageObj = new GameObject("Image");
        textObj.transform.parent = this.transform;
        imageObj.transform.parent = this.transform;

        image = imageObj.AddComponent<Image>();
        text = textObj.AddComponent<TextMeshProUGUI>();

        // Set ActionItem Position 
        //this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100.0F - index * 75.0F, 0);
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -225.0F - index * 75.0F, 0);

        // Set image properties
        image.transform.localScale = Vector3.one;
        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(-229, 23, 0);
        image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
        image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
        image.sprite = _image;

        // Set text properties
        text.transform.localScale = Vector3.one;
        text.GetComponent<RectTransform>().anchoredPosition = new Vector3(225, 0, 0);
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 800);
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        text.GetComponent<TextMeshProUGUI>().font = GameManager.GameUI.font;
        text.GetComponent<TextMeshProUGUI>().fontMaterial = GameManager.GameUI.fontMaterial;
        text.fontSize = 55;
        text.text = _text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Text))]
public class ActionItem : MonoBehaviour {

    public Image image;
    public Text text;

	// Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        text = GetComponent<Text>();
    }
	
    public void SetItem(string _text, Sprite _image) {
        text.text = _text;
        image.sprite = _image;
    }
}

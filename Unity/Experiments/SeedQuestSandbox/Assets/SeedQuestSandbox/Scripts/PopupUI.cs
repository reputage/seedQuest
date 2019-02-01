using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupUI : MonoBehaviour {

    private static PopupUI instance = null;
    public static PopupUI Instance {
        get {
            if (instance == null)
                instance = Resources.FindObjectsOfTypeAll<PopupUI>()[0];
            return instance;
        }
    }

    [TextArea]
    public string popupText;

    public void Start() {
        // Default popup inactive
        gameObject.SetActive(false);

        // Setup Close Button
        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(toggleShow);

        // Setup Toggle Button
        GameObject toggle = GameObject.FindGameObjectWithTag("PopupToggle");
        toggle.GetComponent<Button>().onClick.AddListener(toggleShow);
    }

    public void toggleShow() {
        gameObject.SetActive(!gameObject.activeSelf);
        TextMeshProUGUI text = GameObject.FindGameObjectWithTag("PopupText").GetComponent<TextMeshProUGUI>();
        text.text = popupText;
    }
}
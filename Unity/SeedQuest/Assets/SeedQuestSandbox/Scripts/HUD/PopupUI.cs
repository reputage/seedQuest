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

    public void toggleShow() {
        gameObject.SetActive(!gameObject.activeSelf);

        TextMeshProUGUI text = GetComponentsInChildren<TextMeshProUGUI>()[1];
        if(text != null)
            text.text = popupText;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour {

    private static PopupUI instance = null;
    public static PopupUI Instance {
        get {
            if (instance == null)  instance = GameObject.FindObjectOfType<PopupUI>();
            return instance;
        }
    }

    public void Start()
    {
        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(toggleShow);
    }

    public void toggleShow() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CursorActionUI : MonoBehaviour {

    static private CursorActionUI instance = null;
    static private CursorActionUI setInstance() { instance = Resources.FindObjectsOfTypeAll<CursorActionUI>()[0]; return instance; }
    static public CursorActionUI Instance { get => instance == null ? setInstance() : instance; }

    public Vector3 offset = new Vector3(40, -20, 0);
    private RectTransform rootTransform;
    private RectTransform rectTransform;

    public bool show = false;
    static public bool Show
    {
        get { return Instance.show; }
        set { Instance.show = value; Instance.gameObject.SetActive(value); }
    }

    void Awake() {
        rootTransform = GetComponentsInChildren<RectTransform>()[0];
        rectTransform = GetComponentsInChildren<RectTransform>()[1];
    }

    void Update() {
        if(Show) {
            rectTransform.localPosition = Input.mousePosition - new Vector3(rootTransform.rect.width / 2.0f, rootTransform.rect.height / 2.0f, 0) + offset;
        }
    }
}
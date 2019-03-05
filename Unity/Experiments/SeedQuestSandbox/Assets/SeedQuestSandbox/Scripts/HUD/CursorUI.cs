using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUI : MonoBehaviour {

    static GameObject cursor;
    static private bool showCursor = true;

    static public bool ShowCursor
    {
        get { return showCursor; }
        set { showCursor = value; cursor.SetActive(showCursor); }
    }

    private void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        if (cursor != null)
            cursor.SetActive(showCursor);
    }

}
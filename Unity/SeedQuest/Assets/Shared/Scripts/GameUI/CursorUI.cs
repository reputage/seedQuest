using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUI : MonoBehaviour {

	void Update () {
        if (GameManager.State == GameState.GameStart || GameManager.State == GameState.Pause )
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }
}

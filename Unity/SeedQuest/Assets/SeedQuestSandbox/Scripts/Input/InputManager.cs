using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    static public bool GetKeyDown(KeyCode key) {
        if (CommandLineManager.isInUse)
            return false;
        else
            return Input.GetKeyDown(key);
    }
}
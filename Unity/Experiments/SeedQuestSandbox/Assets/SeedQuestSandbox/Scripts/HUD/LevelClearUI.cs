using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClearUI : MonoBehaviour {

    static private LevelClearUI instance = null;

    static public LevelClearUI Instance {
        get {
            if (instance == null)
                instance = Resources.FindObjectsOfTypeAll<LevelClearUI>()[0];
            return instance;
        }
    }

    static public void ToggleOn() {
        if (Instance.gameObject.activeSelf)
            return;

        Instance.gameObject.SetActive(true);
    }
}

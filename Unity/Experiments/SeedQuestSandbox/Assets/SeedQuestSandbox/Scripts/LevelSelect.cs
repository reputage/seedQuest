using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInfo {
    public string name;
    public Color32 backgroundColor;
    public Texture levelImage;
}

public class LevelSelect : MonoBehaviour {

    public LevelInfo[] levelList;

}

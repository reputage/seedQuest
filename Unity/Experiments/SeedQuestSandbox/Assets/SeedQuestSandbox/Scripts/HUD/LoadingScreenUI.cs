using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : MonoBehaviour
{
    static private LoadingScreenUI instance = null;
    static private LoadingScreenUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<LoadingScreenUI>(true); return instance; }
    static public LoadingScreenUI Instance { get { return instance == null ? setInstance() : instance; } }

    public float loadProgress = 0;
    static public float LoadProgress { get => Instance.loadProgress; set => Instance.loadProgress = value; }

    private Slider progressSlider;

    private void Awake() {
        progressSlider = GetComponentInChildren<Slider>();
    }

    private void Update() {
        progressSlider.value = loadProgress;
    }
}
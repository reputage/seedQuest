using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControlsUI : MonoBehaviour
{
    private Slider volSlider;

    void Start() {
        volSlider = GetComponentInChildren<Slider>();
        volSlider.value = SettingsManager.MasterVolume;
    }

    void Update() {
        SettingsManager.MasterVolume = volSlider.value;
    }
}

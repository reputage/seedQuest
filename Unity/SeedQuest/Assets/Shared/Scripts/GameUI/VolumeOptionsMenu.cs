using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOptionsMenu : MonoBehaviour {

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    private bool muted; 

    void Start () {
        masterSlider.value = SettingsManager.MasterVolume;
        musicSlider.value = SettingsManager.MusicVolume;
        sfxSlider.value = SettingsManager.SoundEffectVolume;
        muted = SettingsManager.IsVolumeMuted;
    }
	
    void Update () {
        masterVolChange();
        musicVolChange();
        sfxVolChange();
    }

    public void masterVolChange()
    {
       SettingsManager.MasterVolume = masterSlider.value;
    }

    public void musicVolChange()
    {
        SettingsManager.MusicVolume = musicSlider.value;
    }

    public void sfxVolChange()
    {
        SettingsManager.SoundEffectVolume = sfxSlider.value;
    }

    public void muteToggle()
    {
        SettingsManager.IsVolumeMuted = !SettingsManager.IsVolumeMuted;
        muted = SettingsManager.IsVolumeMuted;
    }

    public void LeaveVolumeSettingsMenu()
    {
        PauseMenuCanvas.State = MenuState.PauseMenu;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOptionsMenu : MonoBehaviour {

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    void Start () {
        masterSlider.value = SettingsManager.MasterVolume;
        musicSlider.value = SettingsManager.MusicVolume;
        sfxSlider.value = SettingsManager.SoundEffectVolume;
        muteToggle.isOn = SettingsManager.IsVolumeMuted;
    }
	
    public void MasterVolumeChanged(float value) 
    {
       SettingsManager.MasterVolume = value;
    }

    public void MusicVolumeChanged(float value)
    {
        SettingsManager.MusicVolume = value;
    }

    public void sfxVolumeChanged(float value)
    {
        SettingsManager.SoundEffectVolume = value;
    }

    public void MuteToggleChanged(bool value)
    {
        SettingsManager.IsVolumeMuted = value;
    }

    public void LeaveVolumeSettingsMenu()
    {
        PauseMenuCanvas.State = MenuState.PauseMenu;
    }
}

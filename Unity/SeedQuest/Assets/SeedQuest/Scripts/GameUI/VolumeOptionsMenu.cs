using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOptionsMenu : MonoBehaviour {

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;
    public Slider cameraSensitivity;

    void Start () {
        masterSlider.value = SettingsManager.MasterVolume;
        musicSlider.value = SettingsManager.MusicVolume;
        sfxSlider.value = SettingsManager.SoundEffectVolume;
        muteToggle.isOn = SettingsManager.IsVolumeMuted;
        cameraSensitivity.value = SettingsManager.CameraSensitivity;
    }

    public void updateSettings()
    {
        masterSlider.value = SettingsManager.MasterVolume;
        musicSlider.value = SettingsManager.MusicVolume;
        sfxSlider.value = SettingsManager.SoundEffectVolume;
        muteToggle.isOn = SettingsManager.IsVolumeMuted;
        cameraSensitivity.value = SettingsManager.CameraSensitivity;
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
        value = muteToggle.isOn;
        SettingsManager.IsVolumeMuted = value;
    }

    public void CameraSensitivityChanged(float value)
    {
        SettingsManager.CameraSensitivity = value;
    }

    public void LeaveVolumeSettingsMenu()
    {
        SaveSettings.saveSettings();
        PauseMenuCanvas.State = MenuState.PauseMenu;
    }
}

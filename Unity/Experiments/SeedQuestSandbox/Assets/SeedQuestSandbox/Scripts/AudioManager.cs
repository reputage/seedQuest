using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds = null;

    static private AudioManager instance = null;

    private void Awake() {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitAudioSettings();
        //Play("ThemeMusic");
    }

    public void InitAudioSettings()
    {
        sounds = GameManager.GameSound.Sounds;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    static public void UpdateAudioSettings() {

        foreach (Sound s in instance.sounds)
        {
            float volumeFactor;
            if (s.name == "ThemeMusic")
                volumeFactor = SettingsManager.MusicVolume;
            else
                volumeFactor = SettingsManager.SoundEffectVolume;

            if (SettingsManager.IsVolumeMuted)
                volumeFactor = 0f;

            s.source.volume = s.volume * SettingsManager.MasterVolume * volumeFactor;
            //Debug.Log("Updating Audio Settings to " + s.source.volume);
        }
    }

    static public void Play (string name) {
        Sound s = Array.Find(instance.sounds, Sound => Sound.name == name);
        if (s == null)
            Debug.LogWarning("Sounds : " + name + " was not found.");
        else
            s.source.Play();
    }
}

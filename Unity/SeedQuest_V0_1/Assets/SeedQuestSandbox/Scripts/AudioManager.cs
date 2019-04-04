using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    static private AudioManager instance = null;

    public List<Sound> sounds = new List<Sound>();
    public GameSoundData UISounds;
    public GameSoundData DefaultSounds;
    public GameSoundData GameSounds;

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

    public void InitAudioSettings() {
        if(UISounds != null)
            sounds.AddRange(UISounds.Sounds);
        if(DefaultSounds != null)
            sounds.AddRange(DefaultSounds.Sounds);
        if(GameSounds != null) 
            sounds.AddRange(GameSounds.Sounds);

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
        }
    }

    static public void Play (string name) {
        if (instance == null) {
            Debug.LogWarning("Warning: Can't find AudioManager instance.");
            return;
        }
        
        Sound s = Array.Find(instance.sounds.ToArray(), Sound => Sound.name == name);
        if (s == null)
            Debug.LogWarning("Sounds : " + name + " was not found.");
        else
            s.source.Play();
    }
}
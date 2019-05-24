using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    
    static public bool useDebug = false;
    public float masterVolume = 1.0f;
    public float musicVolume = 1.0f;
    public float soundEffectVolume = 1.0f;
    public float cameraSensitivity = 1.0f;
    public bool muteVolume = false;

    static private SettingsManager __instance = null;
    static public SettingsManager Instance
    {
        get
        {
            if (__instance == null)
                __instance = GameObject.FindObjectOfType<SettingsManager>();
            return __instance;
        }
    }

    static public float MasterVolume {
        get { return Instance.masterVolume; }
        set { Instance.masterVolume = value; AudioManager.UpdateAudioSettings(); }
    }

    static public float MusicVolume {
        get { return Instance.musicVolume; }
        set { Instance.musicVolume = value; AudioManager.UpdateAudioSettings(); }
    }

    static public float SoundEffectVolume {
        get { return Instance.soundEffectVolume; }
        set { Instance.soundEffectVolume = value; AudioManager.UpdateAudioSettings(); }
    }

    static public bool IsVolumeMuted {
        get { return Instance.muteVolume; }
        set { Instance.muteVolume = value; AudioManager.UpdateAudioSettings(); }
    }

    static public float CameraSensitivity
    {
        get { return Instance.cameraSensitivity; }
        set { Instance.cameraSensitivity = value; }
    }
}

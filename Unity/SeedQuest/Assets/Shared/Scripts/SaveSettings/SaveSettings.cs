using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSettings
{
    public static void saveSettings()
    {
        primeSettings();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedSettings.gd");
        bf.Serialize(file, Settings.settingsHere);
        file.Close();

    }

    public static void loadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/savedSettings.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedSettings.gd", FileMode.Open);
            Settings.settingsHere = (Settings)bf.Deserialize(file);
            file.Close();
            setSettings();
        }
        else
        {
            Settings.MasterVolume = 1f;
            Settings.MusicVolume = 1f;
            Settings.SoundEffectVolume = 1f;
            Settings.IsVolumeMuted = false;
        }

        setSettings();
    }

    public static void primeSettings()
    {
        Settings.MasterVolume = SettingsManager.MasterVolume;
        Settings.MusicVolume = SettingsManager.MusicVolume;
        Settings.SoundEffectVolume = SettingsManager.SoundEffectVolume;
        Settings.IsVolumeMuted = SettingsManager.IsVolumeMuted;
    }

    public static void setSettings()
    {
        SettingsManager.MasterVolume = Settings.MasterVolume;
        SettingsManager.MusicVolume = Settings.MusicVolume;
        SettingsManager.SoundEffectVolume = Settings.SoundEffectVolume;
        SettingsManager.IsVolumeMuted = Settings.IsVolumeMuted;
    }

}

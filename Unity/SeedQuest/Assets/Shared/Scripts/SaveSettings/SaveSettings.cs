using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSettings
{

    public static void saveSettings()
    {
        Settings settings = new Settings();
        primeSettings(settings);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedSettings.gd");
        bf.Serialize(file, settings);
        file.Close();

    }

    public static void loadSettings()
    {
        Settings settings = new Settings();

        if (File.Exists(Application.persistentDataPath + "/savedSettings.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedSettings.gd", FileMode.Open);
            settings = (Settings)bf.Deserialize(file);
            file.Close();
            setSettings(settings);
        }
        else
        {
            settings.MasterVolume = 1f;
            settings.MusicVolume = 1f;
            settings.SoundEffectVolume = 1f;
            settings.IsVolumeMuted = false;
        }

        setSettings(settings);
    }

    public static void primeSettings(Settings settings)
    {
        settings.MasterVolume = SettingsManager.MasterVolume;
        settings.MusicVolume = SettingsManager.MusicVolume;
        settings.SoundEffectVolume = SettingsManager.SoundEffectVolume;
        settings.IsVolumeMuted = SettingsManager.IsVolumeMuted;
    }

    public static void setSettings(Settings settings)
    {
        SettingsManager.MasterVolume = settings.MasterVolume;
        SettingsManager.MusicVolume = settings.MusicVolume;
        SettingsManager.SoundEffectVolume = settings.SoundEffectVolume;
        SettingsManager.IsVolumeMuted = settings.IsVolumeMuted;
    }

}

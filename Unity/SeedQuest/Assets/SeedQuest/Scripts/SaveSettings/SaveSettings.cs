using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSettings
{

    // Save the settings data to a file
    public static void saveSettings()
    {
        Settings settings = new Settings();
        retrieveSettings(settings);
        retrieveDids(settings);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedSettings.gd");
        bf.Serialize(file, settings);
        file.Close();

    }

    // Loads the settings from a previously saved settings file, if one exists
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
            setDids(settings);
        }
        else
        {
            settings.MasterVolume = 1f;
            settings.MusicVolume = 1f;
            settings.SoundEffectVolume = 1f;
            settings.CameraSensitivity = 0.5f;
            settings.IsVolumeMuted = false;
        }
        if (settings.CameraSensitivity == 0f)
            settings.CameraSensitivity = 0.1f;

        setSettings(settings);
    }

    // Retrieves the settings from the settings manager, and saves them in a setting object
    public static void retrieveSettings(Settings settings)
    {
        settings.MasterVolume = SettingsManager.MasterVolume;
        settings.MusicVolume = SettingsManager.MusicVolume;
        settings.SoundEffectVolume = SettingsManager.SoundEffectVolume;
        settings.CameraSensitivity = SettingsManager.CameraSensitivity;
        settings.IsVolumeMuted = SettingsManager.IsVolumeMuted;
    }

    // Sets the settings in the settings manager equal to the settings retrieved from the saved file
    public static void setSettings(Settings settings)
    {
        SettingsManager.MasterVolume = settings.MasterVolume;
        SettingsManager.MusicVolume = settings.MusicVolume;
        SettingsManager.SoundEffectVolume = settings.SoundEffectVolume;
        SettingsManager.CameraSensitivity = settings.CameraSensitivity;
        SettingsManager.IsVolumeMuted = settings.IsVolumeMuted;
    }

    public static void retrieveDids(Settings settings)
    {
        settings.userDids = DideryDemoManager.UserDids;
    }

    public static void setDids(Settings settings)
    {
        DideryDemoManager.UserDids = settings.userDids;
    }

}

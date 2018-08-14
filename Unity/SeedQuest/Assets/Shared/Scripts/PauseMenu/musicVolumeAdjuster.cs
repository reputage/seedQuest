using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicVolumeAdjuster : MonoBehaviour
{

    // This script should be attached to any object with an audiosource used for music
    // This script will find the audiosource of the object, and adjust the volume.

    public GameStateData gameState;
    public AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameState.musicMute)
        {
            //mute volume
            audioSource.volume = 0;
        }
        else
        {
            if (gameState.masterVolume == 0 || gameState.musicVolume == 0)
            {
                audioSource.volume = 0;
                //Debug.Log("setting music volume to 0");
            }
            else
            {
                audioSource.volume = gameState.masterVolume * gameState.musicVolume;
            }
        }
    }
}

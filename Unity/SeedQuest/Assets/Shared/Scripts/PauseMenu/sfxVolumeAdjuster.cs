using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxVolumeAdjuster : MonoBehaviour {
    
    // This script should be attached to any object with an audiosource used for sound effect
    // This script will find the audiosource of the object, and adjust the volume.

    public GameStateData gameState;
    public AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameState.masterVolume == 0 || gameState.sfxVolume == 0)
        {
            audioSource.volume = 0;
            //Debug.Log("Setting sfx volume to 0...");
        }
        else
            audioSource.volume = gameState.masterVolume * gameState.sfxVolume;    
    }
}

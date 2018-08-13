using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxVolumeAdjuster : MonoBehaviour {

    public GameStateData gameState;

    public AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.volume = 1;
    }

    // Update is called once per frame
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

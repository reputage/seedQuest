using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicVolumeAdjuster : MonoBehaviour {

    public GameStateData gameState;

    public AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState.masterVolume == 0 || gameState.musicVolume == 0 )
        {
            audioSource.volume = 0;
            //Debug.Log("setting music volume to 0");
            //Debug.Log(gameState.masterVolume + " " + gameState.musicVolume);
        }
        else
            audioSource.volume = gameState.masterVolume * gameState.musicVolume;
    }
}

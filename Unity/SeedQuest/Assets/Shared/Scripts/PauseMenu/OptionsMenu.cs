using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public GameStateData gameState;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;


	// Use this for initialization
	void Start () 
    {
        masterSlider.value = 1f;
        musicSlider.value = 1f;
        sfxSlider.value = 1f;
        gameState.masterVolume = 1f;
        gameState.musicVolume = 1f;
        gameState.sfxVolume = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        masterVolChange();
        musicVolChange();
        sfxVolChange();
        //Debug.Log("options menu updating...");
	}

    // Function to change the master volume
    public void masterVolChange()
    {
        gameState.masterVolume = masterSlider.value;
    }

    // Function to change the music volume
    public void musicVolChange()
    {
        gameState.musicVolume = musicSlider.value;
    }

    // Function to change the sound effects (sfx) volume
    public void sfxVolChange()
    {
        gameState.sfxVolume = sfxSlider.value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepSound : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip footstep1;

	void Start () 
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		
	}

    void playAudio(AudioClip sound)
    {
        Debug.Log("Footstep sound playing...");
        audioSource.PlayOneShot(footstep1);
    }
}

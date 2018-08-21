using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepSound : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip footstep1;

    //public GameObject dustCloud;

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

    void runStep(AudioClip sound)
    {
        Debug.Log("Running footstep sound playing...");
        audioSource.PlayOneShot(footstep1);
        //Vector3 position = gameObject.transform.position;
        //GameObject dustObj = Instantiate(dustCloud, this.transform.position, this.transform.rotation);
    }
}

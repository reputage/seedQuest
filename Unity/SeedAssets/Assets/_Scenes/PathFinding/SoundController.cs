using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public Transform player;
    public float soundRange = 20.0F;

	// Update is called once per frame
	void Update () {
        AudioSource audio = GetComponent<AudioSource>();
        Vector3 dist = player.position - transform.position;

        if (dist.sqrMagnitude < soundRange && audio.isPlaying == false) {
            audio.Play();
        } 

        if (dist.sqrMagnitude > soundRange && audio.isPlaying == true) {
            audio.Stop();
        }
	}
}

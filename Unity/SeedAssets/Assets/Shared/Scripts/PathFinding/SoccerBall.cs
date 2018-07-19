using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour {

    public Transform target;
    public float speed = 15.0F;
    private bool isKicked = false;

	// Use this for initialization
	void Start () {
		
	}
	
    public void KickBall() {
        isKicked = true;
        AudioSource _audio = GetComponent<AudioSource>();
        _audio.Play();
    }

	// Update is called once per frame
	void Update () {
        if(isKicked) {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
	}
}

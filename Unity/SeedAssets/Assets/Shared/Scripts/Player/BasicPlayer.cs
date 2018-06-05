using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour {

    public float moveSpeed = 10;
    public float rotateSpeed = 60;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Rotate(0, rotate, 0);
        transform.Translate(0, 0, move);
	}
}

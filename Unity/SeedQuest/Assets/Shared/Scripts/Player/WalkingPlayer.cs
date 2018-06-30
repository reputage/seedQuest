using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPlayer : MonoBehaviour {

    Animator animator;
    public float moveSpeed = 10;
    public float rotateSpeed = 100;
    // public float gravity = 8.9;

	// Use this for initialization
	void Start () {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        //Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        //float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //transform.Rotate(0, rotate, 0);
        transform.Translate(moveHorizontal, 0, 0);
        transform.Translate(0, 0, moveVertical);

        if(moveVertical != 0) {
            animator.SetBool("Walk", true);
        }
        else {
            animator.SetBool("Walk", false);
        }

	}
}

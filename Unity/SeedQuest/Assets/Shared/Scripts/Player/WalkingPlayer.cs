using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPlayer : MonoBehaviour {

    Animator animator;

    public GameStateData gameState;

    public float moveSpeed = 11;
    public float runMultiplier = 2.5f;
    public float rotateSpeed = 100;
    public float runSpeed = 1f;

    private float moveHorizontal = 0f;
    private float moveVertical = 0f;

	// Use this for initialization
	void Start () {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        //Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        //float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;

        // runSpeed = animator.GetBool("Run") ? runMultiplier : 1;

        if (animator.GetBool("Run") && runSpeed < runMultiplier)
            runSpeed += 0.09f;
        else if (animator.GetBool("Run"))
            runSpeed = runMultiplier;
        else
            runSpeed = 1f;

        if (!gameState.isPaused)
        {
            moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * runSpeed;
        }
        else
        {
            moveHorizontal = 0f;
            moveVertical = 0f;
        }


        //transform.Rotate(0, rotate, 0);
        transform.Translate(moveHorizontal, 0, 0);
        transform.Translate(0, 0, moveVertical);

        if(moveVertical != 0) {
            animator.SetBool("Walk", true);
        }
        else {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown("r") && !gameState.isPaused)
            if (animator.GetBool("Run"))
                animator.SetBool("Run", false);
            else
                animator.SetBool("Run", true);
    }
}

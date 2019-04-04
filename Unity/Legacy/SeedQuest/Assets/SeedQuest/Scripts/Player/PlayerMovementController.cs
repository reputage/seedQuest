using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    private Animator animator; 
    public GameObject cloudGenerator;

    public float moveSpeed = 11;
    public float runMultiplier = 2.5f;
    public float rotateSpeed = 100;
    public float runSpeed = 1f;

    private float moveHorizontal = 0f;
    private float moveVertical = 0f;

	void Start () {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
	}
	
	void Update () {

        // runSpeed = animator.GetBool("Run") ? runMultiplier : 1;
        if (animator.GetBool("Run") && runSpeed < runMultiplier)
            runSpeed += 0.09f;
        else if (animator.GetBool("Run"))
            runSpeed = runMultiplier;
        else
            runSpeed = 1f;

        if (!PauseManager.isPaused) {
            moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * runSpeed;
        }
        else {
            moveHorizontal = 0f;
            moveVertical = 0f;
        }

        transform.Translate(moveHorizontal, 0, 0);
        transform.Translate(0, 0, moveVertical);

        if(moveVertical != 0) {
            animator.SetBool("Walk", true);
        }
        else {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            //cloudGenerator.GetComponent<cloudGenerator>().stopGenerate();
        }

        if (Input.GetKeyDown("r") && !PauseManager.isPaused)
        {
            if (animator.GetBool("Run"))
            {
                animator.SetBool("Run", false);
                //cloudGenerator.GetComponent<cloudGenerator>().stopGenerate();
            }
            else
            {
                animator.SetBool("Run", true);
                //cloudGenerator.GetComponent<cloudGenerator>().startGenerate();
            }
        }
    }
}

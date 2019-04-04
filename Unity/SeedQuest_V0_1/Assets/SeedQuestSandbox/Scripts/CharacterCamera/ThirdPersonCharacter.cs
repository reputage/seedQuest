using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacter : MonoBehaviour {

    public float rotateSpeed = 2f;
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public bool isRunning = false;
    public float jumpVelocity = 4;

    private void Update() {
        if(!PauseManager.isPaused) {
            MoveCharacter();
            RotateCharacter();
            CheckForJump();
        }
    }

    /// <summary> Move Character based on Inputs with (WASD), and Shift </summary>
    public void MoveCharacter() {
        // Get move quantity based on delta time and speed
        float moveSpeed = isRunning ? runSpeed : walkSpeed;
        moveSpeed = PauseManager.isPaused ? 0 : moveSpeed;
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Translate character
        transform.Translate(moveHorizontal, 0, 0);
        transform.Translate(0, 0, moveVertical);

        // Use Left Shift to Toggle Running
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            isRunning = !isRunning;
        }
    }

    /// <summary> Rotate Character horizontally with Mouse </summary>
    public void RotateCharacter() {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed * SettingsManager.CameraSensitivity;
        transform.localRotation *= Quaternion.Euler(0f, horizontal, 0f);
    }

    public void CheckForJump() {
        if (Input.GetButtonDown("Jump")) {
            GetComponent<Rigidbody>().velocity = Vector2.up * jumpVelocity;
            AudioManager.Play("Jump");
        }
    }
}
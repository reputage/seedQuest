using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {

    public float mouseSpeed = 2f;
    public float moveSpeed = 5f;

    private Transform characterTransform;
    private Transform cameraTransform;

    public void Update() {
        UpdatePosition();
        UpdateRotation();
    }

    public void UpdatePosition() {

        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveHorizontal, 0, 0);
        transform.Translate(0, 0, moveVertical);
    }

    public void UpdateRotation() {
        float horizontal = Input.GetAxis("Mouse X") * mouseSpeed * SettingsManager.CameraSensitivity;
        float vertical = Input.GetAxis("Mouse Y") * mouseSpeed * SettingsManager.CameraSensitivity;
        vertical = Mathf.Clamp(vertical, -90f, 90f);

        characterTransform = transform;
        cameraTransform = GetComponentInChildren<Camera>().transform;
        characterTransform.localRotation *= Quaternion.Euler(0f, horizontal, 0f);
        cameraTransform.localRotation *= Quaternion.Euler(-vertical, 0f, 0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 
}

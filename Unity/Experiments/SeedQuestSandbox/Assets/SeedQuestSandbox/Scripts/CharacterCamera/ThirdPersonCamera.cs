using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public float mouseSpeed = 2f;
    public bool lockCursor = true;

    private void LateUpdate() {
        if(!PauseManager.isPaused) { 
            CameraRotate();
        }

        UpdateCursorLock();
    }

    /// <summary> Rotate Camera with Mouse (along vertical direction) </summary>
    public void CameraRotate() {
        float verticalRotate = Input.GetAxis("Mouse Y") * mouseSpeed * SettingsManager.CameraSensitivity;
        verticalRotate = Mathf.Clamp(verticalRotate, -80, 80);
        Transform cameraTransform = GetComponentInChildren<Camera>().transform;
        cameraTransform.localRotation *= Quaternion.Euler(-verticalRotate, 0f, 0f);
    }

    public void UpdateCursorLock() {
        if(lockCursor && !PauseManager.isPaused) { 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CursorUI.ShowCursor = true;
        } 
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CursorUI.ShowCursor = false;
        }
    }
}
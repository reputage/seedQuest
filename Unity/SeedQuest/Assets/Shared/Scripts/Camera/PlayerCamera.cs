using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPos { Near, Far }

public class PlayerCamera : MonoBehaviour {

    // Sensitivity Properties
    public float moveSpeed = 4;
    public float mouseXSpeed = 200f;
    public float mouseYSpeed = 1f;

    // Camera Position and LookAt Properties
    public float cameraDistance = 2f;
    public float cameraFarDistance = 8f;
    public float cameraRotateXOffset = -15;
    public float cameraFarRotateXOffset = 0;
    public Vector2 cameraRotateYBounds = new Vector2(-25f, 15f);
    public Vector3 cameraPositionOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 cameraFarPositionOffset = new Vector3(0.0f, 8.0f, 0.0f);
    public Vector3 cameraTargetOffset = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 cameraFarTargetOffset = new Vector3(0.0f, 4.0f, 0.0f);

    // Accumulators for MousePosition
    private float cameraRotateYOffset = 0f;

    // Camera Position Interpolation 
    private CameraPos cameraPosState = CameraPos.Near;
    private bool goCameraMove = false;
    public float lerpStopTime = 1.0f;
    private float lerpTime = 1.0f;
    float Percentage { get { return lerpTime / lerpStopTime; } }

    // Camera Position and LookAt
    Vector3 NearCameraPosition {  get { return transform.position + (transform.forward * -cameraDistance) + cameraPositionOffset; } }
    Vector3 FarCameraPosition { get { return transform.position + (transform.forward * -cameraFarDistance) + cameraFarPositionOffset; } }
    Vector3 NearCameraLookAt { get { return transform.position + cameraTargetOffset; } }
    Vector3 FarCameraLookAt { get { return transform.position + cameraFarTargetOffset; } }

    private void Start() {
        PlayerManager.SetPlayer(transform);
        lerpTime = lerpStopTime;
    }

    void LateUpdate () {
        
        if(!PauseManager.isPaused) {
            //Cursor.visible = false; 
            GoLerp();
            MovePlayer();
            MoveCamera();
            CameraLookAt();
            CameraRotate();
            PlayerLookAt();            
        }
        else {
            Cursor.visible = true;
        }
    }

    void GoLerp() {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Move Camera: " + cameraPosState);

            lerpTime = 0.0f;
            goCameraMove = true;
            if (cameraPosState == CameraPos.Near)
                cameraPosState = CameraPos.Far;
            else
                cameraPosState = CameraPos.Near;
        }

        if (goCameraMove)
        {
            lerpTime += Time.deltaTime;
            if (lerpTime > lerpStopTime)
            {
                lerpTime = lerpStopTime;
            }
        }
    }

    void MovePlayer() {
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveHorizontal, 0, 0);
        transform.Translate(0, 0, moveVertical);
    }

    void MoveCamera() {
        if (cameraPosState == CameraPos.Far)
            Camera.main.transform.position = Vector3.Lerp(NearCameraPosition, FarCameraPosition, Percentage);
        else
            Camera.main.transform.position = Vector3.Lerp(FarCameraPosition, NearCameraPosition, Percentage);
    }

    void CameraLookAt() {
        // Look at Player 
        Vector3 lookAt;
        if (cameraPosState == CameraPos.Far)
            lookAt = Vector3.Lerp(NearCameraLookAt, FarCameraLookAt, Percentage);
        else
            lookAt = Vector3.Lerp(FarCameraLookAt, NearCameraLookAt, Percentage);
        Camera.main.transform.LookAt(lookAt);
    }
        
    void CameraRotate() {
        // Rotate Camera in Horizontal Plane 
        Vector3 xRotateOffset;
        if(cameraPosState == CameraPos.Far)
            xRotateOffset = Vector3.Lerp(new Vector3(0, cameraRotateXOffset, 0), new Vector3(0, cameraFarRotateXOffset, 0), Percentage);
        else
            xRotateOffset = Vector3.Lerp(new Vector3(0, cameraFarRotateXOffset, 0), new Vector3(0, cameraRotateXOffset, 0), Percentage);
        
        Camera.main.transform.Rotate(xRotateOffset);

        // Roate Camera in Vertical Plane
        cameraRotateYOffset += Input.GetAxis("Mouse Y") * mouseYSpeed;
        cameraRotateYOffset = Mathf.Clamp(cameraRotateYOffset, cameraRotateYBounds[0], cameraRotateYBounds[1]);
        Camera.main.transform.Rotate(new Vector3(-cameraRotateYOffset, 0, 0));
    }

    void PlayerLookAt() {
        float rotateX = Input.GetAxis("Mouse X") * mouseXSpeed * Time.deltaTime;
        transform.Rotate(0, rotateX, 0);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class IsometricCamera : MonoBehaviour
{
    static public IsometricCamera instance;
    static public Camera Camera = null;                         // Static reference to Camera 

    private Transform playerTransform;                          // Reference to player transform
    public float smoothSpeed = 2f;                              // Camera lerp smoothing speed parameter
    public Vector3 cameraDirection = new Vector3(1, 1, -1);     // Camera direction vector
    public float distance = 14;                                 // Default camera distance from player
    public float startingDistance = 28;                         // Starting scene camera distance from player
    public float lookAtPeek = 4f;                               // Look Ahead peak distance 

    private float nearInteractableDistance = 8.0f;
    private float nearDistance = 5.0f;
    private float farDistance = 40.0f;
    private float currentDistance;                              // Current Distance from player
    public static float StaticDistance 
    {
        get { return IsometricCamera.instance.currentDistance; }
        set { IsometricCamera.instance.currentDistance = value; }
    }

    private static bool playerIsNearInteractable = true;
    public static void setZoomNear(bool isNear) {
        if(playerIsNearInteractable) {
            StaticDistance = instance.nearInteractableDistance;
            playerIsNearInteractable = false;
        }
    }

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        IsometricCamera.instance = this;
        IsometricCamera.Camera = gameObject.GetComponent<Camera>();
        IsometricCamera.StaticDistance = distance;
    }

    private void Start() {
        //transform.position = cameraDirection.normalized * startingDistance;
        CameraZoom.nearDistance = nearDistance;
        CameraZoom.farDistance = farDistance;
    }

    private void LateUpdate() {
        SmoothFollowCamera();
        //CameraLookAt();
    }

    /* Simple follow camera with Smoothing */
    public void SmoothFollowCamera() {
        if (playerTransform.position == Vector3.zero)
            return;
        
        Vector3 currentOffset = CameraZoom.GetCurrentZoomDistance(cameraDirection, StaticDistance, startingDistance);
        Vector3 desiredPosition = playerTransform.position + playerTransform.forward * lookAtPeek + currentOffset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = currentPosition;
    }

    public void CameraLookAt() {
        Vector3 lookAt = playerTransform.position + playerTransform.forward * lookAtPeek;
        transform.LookAt(lookAt);
    }
}
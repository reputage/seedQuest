using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

using SeedQuest.Interactables;

[ExecuteInEditMode]
public class IsometricCamera : MonoBehaviour
{
    static public IsometricCamera instance;
    static public Camera Camera = null;                         // Static reference to Camera 

    public Transform playerTransform;                           // Reference to player transform
    public float smoothSpeed = 2f;                              // Camera lerp smoothing speed parameter
    public float lookAtSpeed = 2f;
    public Vector3 cameraDirection = new Vector3(1, 1, -1);     // Camera direction vector
    public float distance = 14;                                 // Default camera distance from player
    public float startingDistance = 28;                         // Starting scene camera distance from player
    public float lookAtPeek = 4f;                               // Look Ahead peak distance 

    private bool lookAtInteractable;
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

    private void Start()
    {
        //transform.position = cameraDirection.normalized * startingDistance;
        CameraZoom.nearDistance = nearDistance;
        CameraZoom.farDistance = farDistance;

        #if UNITY_WEBGL
            PostProcessLayer layer = GetComponent<PostProcessLayer>();
            layer.enabled = false;
        #else
            PostProcessLayer layer = GetComponent<PostProcessLayer>();
            if(layer != null)
                layer.enabled = true;
        #endif
    }

    private void LateUpdate() {
        if (lookAtInteractable) {
            CameraFollowInteractable();
            CameraLookAtInteractable();
        }
        else {
            CameraFollowPlayer();
            CameraLookAtPlayer();
        }
    }

    /// <summary> Follow player with camera with smoothing </summary>
    public void CameraFollowPlayer() {
        if (playerTransform.position == Vector3.zero) return;

        Vector3 currentOffset = CameraZoom.GetCurrentZoomDistance(cameraDirection, StaticDistance, startingDistance);
        Vector3 desiredPosition = playerTransform.position + playerTransform.forward * lookAtPeek + currentOffset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = currentPosition;
    }

    /// <summary> Follow interactable with camera with smoothing </summary>
    public void CameraFollowInteractable() {
        if (playerTransform.position == Vector3.zero) return;

        Interactable interactable = InteractableManager.ActiveInteractable;
        Vector3 iOffset = interactable.GetComponent<BoxCollider>().center;
        Vector3 currentOffset = CameraZoom.GetCurrentZoomDistance(cameraDirection, interactable.interactableCamera.zoomDistance, distance);
        Vector3 desiredPosition = interactable.transform.position + interactable.interactableCamera.positionOffset + currentOffset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = currentPosition;
    }

    /// <summary> LookAt player with camera with smoothing </summary>
    public void CameraLookAtPlayer() {
        Vector3 lookAt = playerTransform.position + playerTransform.forward * lookAtPeek;
        LookAt(lookAt); 
    }

    /// <summary> LookAt interactable with camera with smoothing </summary>
    public void CameraLookAtInteractable() {
        Interactable interactable = InteractableManager.ActiveInteractable;
        Vector3 iOffset = interactable.GetComponent<BoxCollider>().center + interactable.interactableCamera.lookAtOffset;
        Vector3 lookAt = interactable.transform.position;
        LookAt(lookAt + iOffset);
    }

    public void ToggleLookAtInteractable(bool active) {
        lookAtInteractable = active;
    }

    public void LookAt(Vector3 target) {
        Vector3 relativePos = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        Quaternion rotation = Quaternion.Lerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

}
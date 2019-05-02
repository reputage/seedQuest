using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour {

    static public Camera Camera = null;              // Static reference to Camera 

    public float smoothSpeed = 10f;                  // Camera lerp smoothing speed parameter
    public Vector3 offset = new Vector3(0, 1, -10);  // Camera position offset

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        IsometricCamera.Camera = gameObject.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        SmoothFollowCamera();
    }

    /* A simple follow camera */
    public void FollowCamera()
    {
        if (playerTransform.position == Vector3.zero)
            return;

        transform.position = playerTransform.position + offset;
    }

    /* A simple follow camera with Smoothing */
    public void SmoothFollowCamera()
    {
        if (playerTransform.position == Vector3.zero)
            return;

        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = currentPosition;
    }
}


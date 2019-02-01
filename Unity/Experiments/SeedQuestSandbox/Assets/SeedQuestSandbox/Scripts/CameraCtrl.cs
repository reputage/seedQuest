using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    static public Camera Camera = null;              // Static reference to Camera 

    public float smoothSpeed = 10f;                  // Camera lerp smoothing speed parameter
    public Vector3 offset = new Vector3(0, 1, -10);  // Camera position offset

    private void Awake() {
        CameraCtrl.Camera = gameObject.GetComponent<Camera>();    
    }

    private void LateUpdate() {
        SmoothFollowCamera();
    }

    /* A simple follow camera */
    public void FollowCamera() {
        if (PlayerCtrl.PlayerTransform.position == Vector3.zero)
            return;

        transform.position = PlayerCtrl.PlayerTransform.position + offset; 
    }

    /* A simple follow camera with Smoothing */
    public void SmoothFollowCamera()
    {
        if (PlayerCtrl.PlayerTransform.position == Vector3.zero)
            return;

        Vector3 desiredPosition = PlayerCtrl.PlayerTransform.position + offset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = currentPosition;

        //transform.LookAt(PlayerCtrl.PlayerTransform);
    }
}

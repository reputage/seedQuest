using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraZoom
{

    static public bool usePlayMode = false; // If false, camera is busy
    static public bool useStartingZoomIn = true; // Status of starting zoom in

    static private float zoomInStartTime = 0;
    static private float zoomInstartTimeOffset = 0.5f;
    static private float zoomInStopTime = 2.0f;

    static public float currentDistance; // Current camera distance
    static public float nearDistance; // Min distance for camera
    static public float farDistance; // Max distance for camera

    static public void StartZoomIn() {
        useStartingZoomIn = true;
        usePlayMode = false;
        zoomInStartTime = Time.time;
    }

    static public void ResetZoom() {
        useStartingZoomIn = false;
        usePlayMode = false;
        zoomInStartTime = 0;
    }

    static public Vector3 GetCurrentZoomDistance(Vector3 cameraDirection, float targetdDistance, float startingDistance) {
        Vector3 targetOffset = cameraDirection.normalized * targetdDistance;
        Vector3 startingOffset = cameraDirection.normalized * startingDistance;

        if (useStartingZoomIn)
        {
            float fraction = CameraDistanceFraction();
            if (CameraReady() && !usePlayMode)
            {
                usePlayMode = true;
                GameManager.State = GameState.Play;
            }

            return Vector3.Lerp(startingOffset, targetOffset, fraction);
        }
        else
            return startingOffset;
    }

    static public bool CameraReady() {
        return CameraDistanceFraction() >= 1.0 ? true : false;
    }

    static public float CameraDistanceFraction() {
        return Mathf.Clamp01((Time.time - zoomInStartTime - zoomInstartTimeOffset) / zoomInStopTime);
    }
}

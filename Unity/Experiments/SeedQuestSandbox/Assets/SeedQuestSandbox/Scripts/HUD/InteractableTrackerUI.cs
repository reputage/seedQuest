using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

[System.Serializable]
public class InteractableTrackerProps {
    public Vector3 positionOffset = Vector3.zero;
    public Vector2 screenPositionOffset = Vector2.zero;
    public Vector2 screenWobbleDirection = Vector2.up;
    public float screenRotation = 0f;
}

public class InteractableTrackerUI : MonoBehaviour
{
    [Header("Position/Angle Properties")]
    public bool isFixedToCenterEdge = true;
    public Vector3 positionOffset = Vector3.zero;
    public float angleOffset = 25;

    [Header("Wobble Properties")]
    public float wobbleStrength = 10;
    public float wobbleSpeed = 5;

    [Header("Near Properties")]
    public float nearDistance = 4;
    public float nearOpacity = 0.5f;
    public Vector3 nearPositionOffset = new Vector3(0, 5.0f, 0);

    [Header("Sandbox Options")]
    public bool showInSandbox = true;
    public int showIndex = 0;

    private Transform player;
    private new Camera camera;
    private RectTransform tracker;
    private RectTransform arrow;
    private CanvasGroup canvasGroup;
    private Interactable target;

    private float angle;
    private Vector3 screenPosition;
    private Vector3 adjustedScreenPosition;
    private Vector3 unclampedScreenPosition;
    private bool isClampedTop = false;
    private bool isClampedBottom = false;
    private bool isClampedRight = false;
    private bool isClampedLeft = false;

    private Vector2 padding = new Vector2(150, 150);
    private Vector2 arrowOffset = new Vector2(75, 75);
    private float behindCameraOffset = 10000; 

    private float MidScreenX { get => camera.scaledPixelWidth / 2.0f; }
    private float MidScreenY { get => camera.scaledPixelHeight / 2.0f; }

    void Start() {
        showIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        tracker = GetComponentsInChildren<RectTransform>()[1];
        arrow = GetComponentsInChildren<RectTransform>()[5];
        canvasGroup = GetComponentInChildren<CanvasGroup>();

        if (GameManager.Mode == GameMode.Rehearsal)
            gameObject.SetActive(true);
        else if (GameManager.Mode == GameMode.Sandbox && showInSandbox)
            gameObject.SetActive(true);
        else 
            gameObject.SetActive(false);
    }

    void Update() {
        if (IsReady) {
            SetTrackedInteractable();
            SetActive();
            SetScreenPosition();
            SetPositionClamp();
            SetTrackerIconPosition();
            SetArrowIconPosition();
            SetOpacity();
        }
    }

    /// <summary> Checks if InteractableTrackerUI is ready </summary>
    private bool IsReady { get => (GameManager.Mode == GameMode.Rehearsal && InteractablePath.NextInteractable != null) || (GameManager.Mode == GameMode.Sandbox && showInSandbox); }

    /// <summary> Sets the interactactable to be tracked based off GameMode </summary>
    private void SetTrackedInteractable() {
        if (GameManager.Mode == GameMode.Rehearsal)
            target = InteractablePath.NextInteractable;
        else if (GameManager.Mode == GameMode.Sandbox) {
            if (showIndex < InteractableManager.InteractableList.Length)
                target = InteractableManager.InteractableList[showIndex];
            else
                target = null;
        }
        else
            target = null;
    }

    /// <summary> Activates/Deactivates the tracker and arrow gameobjects </summary>
    private void SetActive() {
        if (target == null) {
            tracker.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
        }
        else {
            tracker.gameObject.SetActive(true);
            arrow.gameObject.SetActive(true);
        }
    }

    /// <summary> Calculates and Sets InScreen Position </summary>
    private void SetScreenPosition() {
        if (target == null)
            return;

        // Set Unclamped ScreenPosition
        unclampedScreenPosition = camera.WorldToScreenPoint(target.transform.position);

        // Set Adjusted ScreenPosition
        adjustedScreenPosition = camera.WorldToScreenPoint(target.transform.position + positionOffset + target.interactableTracker.positionOffset + NearPositionOffset());
        Vector2 screenPositionOffset = target.interactableTracker.screenPositionOffset;
        adjustedScreenPosition += new Vector3(screenPositionOffset.x, screenPositionOffset.y);
    }

    /// <summary> Sets the position clamp status for TrackerIcon i.e. when outside of the bounds of screen it sets which edges of the screen it is out of bounds </summary>
    private void SetPositionClamp() {
        isClampedRight = unclampedScreenPosition.x > camera.scaledPixelWidth - padding.x;
        isClampedLeft = unclampedScreenPosition.x < padding.x;
        isClampedTop = unclampedScreenPosition.y > camera.scaledPixelHeight - padding.y;
        isClampedBottom = unclampedScreenPosition.y < padding.y;
    }

    /// <summary> Gets Interpolated NearPositionOffset Vector </summary>
    public Vector3 NearPositionOffset()
    {
        Vector3 mag = player.position - target.transform.position;
        if (mag.magnitude < nearDistance) {
            return Vector3.Lerp(nearPositionOffset, Vector3.zero, mag.magnitude / nearDistance);
        }
        else
            return Vector3.zero;
    }

    /// <summary> Set Tracker Position. Follows next interactable, unless offscreen then appears in direction of next interactable. </summary>
    private void SetTrackerIconPosition() {
        if(target == null)
            return;
        
        Vector3 wobble = Vector3.zero;
        // Set screen position when interactables is in the FOV
        if(InBounds(unclampedScreenPosition) && unclampedScreenPosition.z > 0) {
            screenPosition = adjustedScreenPosition;

            float rotate = target.interactableTracker.screenRotation;
            tracker.rotation = Quaternion.Euler(new Vector3(0, 0, rotate));

            Vector2 wobbleDir = target.interactableTracker.screenWobbleDirection;
            wobble = wobbleStrength * new Vector3(wobbleDir.x, wobbleDir.y, 0) * Mathf.Sin(wobbleSpeed * Time.time);
        }   
        // Set screen position when interactables behind the camera FOV
        else if(InBounds(unclampedScreenPosition) && unclampedScreenPosition.z < 0) {
            screenPosition = unclampedScreenPosition;

            tracker.rotation = Quaternion.Euler(Vector3.zero);

            // Clamp TrackerIcon when Next Interactable when object is behind camera
            var x = Mathf.Clamp(screenPosition.x + behindCameraOffset, 0.0f + padding.x, camera.scaledPixelWidth - padding.x);
            var y = Mathf.Clamp(screenPosition.y, 0.0f + padding.y, camera.scaledPixelHeight - padding.y);
            screenPosition = new Vector3(x, y, screenPosition.z);

            if (isFixedToCenterEdge) {
                screenPosition = new Vector3(camera.scaledPixelWidth - padding.x, MidScreenY, screenPosition.z);    
            }
        }
        // Set screen position when interactables is out of FOV
        else {
            screenPosition = unclampedScreenPosition;

            tracker.rotation = Quaternion.Euler(Vector3.zero);

            // Clamp TrackerIcon when Next Interactable if off screen
            var x = Mathf.Clamp(screenPosition.x, 0.0f + padding.x, camera.scaledPixelWidth - padding.x);
            var y = Mathf.Clamp(screenPosition.y, 0.0f + padding.y, camera.scaledPixelHeight - padding.y);
            screenPosition = new Vector3(x, y, screenPosition.z);

            // Clamp TrackerIcon to Center Edge of Screen based on position off screen
            if(isFixedToCenterEdge) {
                if (screenPosition.z < 0)
                    screenPosition = new Vector3(camera.scaledPixelWidth - padding.x, MidScreenY, screenPosition.z);    
                else if (isClampedRight)
                    screenPosition = new Vector3(camera.scaledPixelWidth - padding.x, MidScreenY, screenPosition.z);    
                else if (isClampedLeft)
                    screenPosition = new Vector3(padding.x, MidScreenY, screenPosition.z);    
                else if (isClampedTop && !isClampedLeft && !isClampedRight)
                    screenPosition = new Vector3(MidScreenX, camera.scaledPixelHeight - padding.y, screenPosition.z);
                else if(isClampedBottom && !isClampedLeft && !isClampedRight)
                    screenPosition = new Vector3(MidScreenX, padding.y, screenPosition.z);
            }
        }

        // Set TrackerIcon Postion
        Vector3 camOffset = new Vector3(MidScreenX, MidScreenY, 0.0f);
        tracker.localPosition = screenPosition - camOffset + wobble;    
    }

    /// <summary> Set ArrowIcon over player point in the direction of the Next interactable </summary>
    private void SetArrowIconPositionOnPlayer() {
        Vector3 playerPosition = camera.WorldToScreenPoint(player.position);
        Vector2 v1 = new Vector2(MidScreenX, MidScreenY);
        Vector2 v2 = new Vector2(unclampedScreenPosition.x, unclampedScreenPosition.y);
        angle = Vector2.SignedAngle(playerPosition, v2-v1);
        arrow.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + angleOffset));
    }

    /// <summary> Sets arrow icon when Next interactable is off screen. Arrow faces the Next interactable. </summary>
    private void SetArrowIconPosition() {
        // Show Arrow if Iteractable is out of bounds of screen
        if (InBounds(unclampedScreenPosition) && unclampedScreenPosition.z > 0)
            arrow.gameObject.SetActive(false);
        else
            arrow.gameObject.SetActive(true);

        // Set ArrowIcon facing direction
        Vector3 playerPosition = camera.WorldToScreenPoint(player.position);
        Vector2 v1 = new Vector2(MidScreenX, MidScreenY);
        Vector2 v2 = new Vector2(unclampedScreenPosition.x, unclampedScreenPosition.y);
        angle = Vector2.SignedAngle(playerPosition, v2 - v1);
        arrow.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + angleOffset));

        // Set ArrowIcon facing direction if UI is fixedToEdge
        if(isFixedToCenterEdge) {
            if (unclampedScreenPosition.z < 0)
                angle = 0;
            else if (isClampedRight)
                angle = 0;
            else if (isClampedLeft)
                angle = 180;
            else if (isClampedTop)
                angle = 90;
            else if (isClampedBottom)
                angle = -90;
            arrow.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
        }

        // Set ArrowIcon position offset from TrackerIcon position
        Vector3 clampOffset = Vector3.zero;
        if (isClampedRight)
            clampOffset = new Vector3(arrowOffset.x, 0, 0);
        else if (isClampedLeft)
            clampOffset = new Vector3(-arrowOffset.x, 0, 0);
        else if (isClampedTop)
            clampOffset = new Vector3(0, arrowOffset.y, 0);
        else if (isClampedBottom)
            clampOffset = new Vector3(0, -arrowOffset.y, 0);

        // Set ArrowIcon position
        Vector3 camOffset = new Vector3(MidScreenX, MidScreenY, 0.0f);
        arrow.localPosition = screenPosition - camOffset + clampOffset;
    }

    /// <summary> Checks if screen position is in Camera frame bounds </summary>
    private bool InBounds(Vector3 pos) {
        float x0 = 0;
        float x1 = camera.scaledPixelWidth;
        float y0 = 0; 
        float y1 = camera.scaledPixelHeight;
        return x0 <= pos.x && pos.x <= x1 && y0 <= pos.y && pos.y <= y1;
    }

    /// <summary> Set Near Opacity </summary>
    public void SetOpacity() {
        if (target == null)
            return;
        
        Vector3 mag = player.position - target.transform.position;
        if (mag.magnitude < nearDistance) {
            canvasGroup.alpha = nearOpacity;
            float scale = (mag.magnitude / nearDistance) * 0.5f + 0.5f;
            canvasGroup.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
        }
        else {
            canvasGroup.alpha = 1.0f;
            canvasGroup.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
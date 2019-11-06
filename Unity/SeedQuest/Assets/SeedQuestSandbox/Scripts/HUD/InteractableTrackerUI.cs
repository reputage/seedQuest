using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

public class InteractableTrackerUI : MonoBehaviour
{
    [Header("Position/Angle Properties")]
    public bool isFixedToCenterEdge = false;
    public bool isArrowAngleFixedToEdge = true;
    public float angleOffset = 25;

    [Header("Sandbox Options")]
    public bool showInSandbox = true;

    private Transform player;
    private new Camera camera;
    private RectTransform tracker;
    private RectTransform arrow;
    private CanvasGroup canvasGroup;
    private Interactable target;

    private float angle;
    private Vector3 screenPosition;
    private Vector3 unclampedScreenPosition;
    private bool isClampedTop = false;
    private bool isClampedBottom = false;
    private bool isClampedRight = false;
    private bool isClampedLeft = false;

    public Vector2 paddingX = new Vector2(150, 150);
    public Vector2 paddingY = new Vector2(150, 225);
    public Vector2 arrowOffset = new Vector2(75, 75);

    private float MidScreenX { get => camera.scaledPixelWidth / 2.0f; }
    private float MidScreenY { get => camera.scaledPixelHeight / 2.0f; }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        tracker = GetComponentsInChildren<RectTransform>()[1];
        arrow = GetComponentsInChildren<RectTransform>()[3];
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
        }
        else {
            target = null;
            SetActive(); 
        }
    }

    /// <summary> Checks if InteractableTrackerUI is ready </summary>
    private bool IsReady { get => (GameManager.Mode == GameMode.Rehearsal && InteractablePath.NextInteractable != null) || (GameManager.Mode == GameMode.Sandbox && showInSandbox); }

    /// <summary> Sets the interactactable to be tracked based off GameMode </summary>
    private void SetTrackedInteractable() {
        if (GameManager.Mode == GameMode.Rehearsal)
            target = InteractablePath.NextInteractable;
        else if (GameManager.Mode == GameMode.Sandbox) {
            target = InteractableManager.ActiveInteractable;
        }
        else
            target = null;
    }

    /// <summary> Activates/Deactivates the tracker and arrow gameobjects </summary>
    private void SetActive() {
        if (InteractableLog.CurrentLevelIndex < GameManager.GraduatedFlags.Length && GameManager.GraduatedFlags[InteractableLog.CurrentLevelIndex] == true) {
            tracker.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
            return;
        }

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
        unclampedScreenPosition = camera.WorldToScreenPoint(target.LabelPosition);
    }

    /// <summary> Sets the position clamp status for TrackerIcon i.e. when outside of the bounds of screen it sets which edges of the screen it is out of bounds </summary>
    private void SetPositionClamp() {
        isClampedRight = unclampedScreenPosition.x > camera.scaledPixelWidth - paddingX.x;
        isClampedLeft = unclampedScreenPosition.x < paddingX.y;
        isClampedTop = unclampedScreenPosition.y > camera.scaledPixelHeight - paddingY.x;
        isClampedBottom = unclampedScreenPosition.y < paddingY.y;
    }

    /// <summary> Checks if screen position is in Camera frame bounds </summary>
    private bool InBounds(Vector3 pos) {
        float screenPadding = 100; 

        float x0 = 0 + screenPadding;
        float x1 = camera.scaledPixelWidth - screenPadding;
        float y0 = 0 + screenPadding;
        float y1 = camera.scaledPixelHeight - screenPadding;
        return x0 <= pos.x && pos.x <= x1 && y0 <= pos.y && pos.y <= y1;
    }

    /// <summary> Set Tracker Position. Follows next interactable, unless offscreen then appears in direction of next interactable. </summary>
    private void SetTrackerIconPosition() {
        if(target == null)
            return;
        
        Vector3 wobble = Vector3.zero;
        // Set screen position when interactables is in the FOV
        if(InBounds(unclampedScreenPosition) && unclampedScreenPosition.z > 0) {
            screenPosition = unclampedScreenPosition;
            canvasGroup.alpha = 0.0f;
        }   
        // Set screen position when interactables behind the camera FOV
        else if(InBounds(unclampedScreenPosition) && unclampedScreenPosition.z < 0) {
            screenPosition = unclampedScreenPosition;
            canvasGroup.alpha = 0.0f; 
        }
        // Set screen position when interactables is out of FOV
        else {
            screenPosition = unclampedScreenPosition;
            tracker.rotation = Quaternion.Euler(Vector3.zero);

            // Clamp TrackerIcon when Next Interactable if off screen
            var x = Mathf.Clamp(screenPosition.x, 0.0f + paddingX.x, camera.scaledPixelWidth - paddingX.y);
            var y = Mathf.Clamp(screenPosition.y, 0.0f + paddingY.x, camera.scaledPixelHeight - paddingY.y); 

            // Correction if interactable is behind the camera 
            if (unclampedScreenPosition.z < 0f) {
                x = MidScreenX - (x - MidScreenX);
                y = MidScreenY - (y - MidScreenY);
            }

            screenPosition = new Vector3(x, y, screenPosition.z);

            // Clamp TrackerIcon to Center Edge of Screen based on position off screen
            float xMax = camera.scaledPixelWidth;
            float yMax = camera.scaledPixelHeight;
            if (x > camera.scaledPixelWidth * 0.7f && y < MidScreenY) {
                if( (x / xMax) > (1 - (y / yMax)) )
                    screenPosition = new Vector3(camera.scaledPixelWidth * 0.7f, 0 + paddingY.x);
                else 
                    screenPosition = new Vector3(camera.scaledPixelWidth - paddingX.y, MidScreenY);
            }

            if(isFixedToCenterEdge) {
                if (screenPosition.z < 0)
                    screenPosition = new Vector3(camera.scaledPixelWidth - paddingX.x, MidScreenY, screenPosition.z);    
                else if (isClampedRight)
                    screenPosition = new Vector3(camera.scaledPixelWidth - paddingX.x, MidScreenY, screenPosition.z);    
                else if (isClampedLeft)
                    screenPosition = new Vector3(paddingX.y, MidScreenY, screenPosition.z);    
                else if (isClampedTop && !isClampedLeft && !isClampedRight)
                    screenPosition = new Vector3(MidScreenX, camera.scaledPixelHeight - paddingY.y, screenPosition.z);
                else if(isClampedBottom && !isClampedLeft && !isClampedRight)
                    screenPosition = new Vector3(MidScreenX, paddingY.y, screenPosition.z); 
            }
            canvasGroup.alpha = 1.0f;
        }

        // Set TrackerIcon Postion
        Vector3 camOffset = new Vector3(MidScreenX, MidScreenY, 0.0f);
        tracker.localPosition = screenPosition - camOffset + wobble;    
    }

    /// <summary> Sets arrow icon when Next interactable is off screen. Arrow faces the Next interactable. </summary>
    private void SetArrowIconPosition() {
        // Show Arrow if Iteractable is out of bounds of screen
        if (InBounds(unclampedScreenPosition) && unclampedScreenPosition.z > 0)
            arrow.gameObject.SetActive(false);
        else {
            if (GameManager.Mode == GameMode.Rehearsal && GameManager.GraduatedFlags[InteractableLog.CurrentLevelIndex] == true)
                return;
            else
                arrow.gameObject.SetActive(true);
        }

        // Set ArrowIcon facing direction
        Vector3 playerPosition = camera.WorldToScreenPoint(player.position);
        Vector2 v1 = new Vector2(MidScreenX, MidScreenY);
        Vector2 v2 = new Vector2(unclampedScreenPosition.x, unclampedScreenPosition.y);
        angle = Vector2.SignedAngle(playerPosition, v2 - v1);
        arrow.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + angleOffset));

        // Set ArrowIcon facing direction if UI is fixedToEdge
        if(isArrowAngleFixedToEdge) {
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
}
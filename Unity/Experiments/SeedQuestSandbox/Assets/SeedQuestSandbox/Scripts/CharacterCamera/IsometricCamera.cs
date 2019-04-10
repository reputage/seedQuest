using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IsometricCamera : MonoBehaviour
{

    public enum ScreenSpaceDirection { left, right, up, down };

    static public Camera Camera = null;              // Static reference to Camera 

    public float smoothSpeed = 10f;                  // Camera lerp smoothing speed parameter
    public Vector3 offset = new Vector3(1, 1, -1);  // Camera position offset
    public Vector3 cameraDirection = new Vector3(1, 1, -1);
    public float distance = 14;
    public float startingDistance = 28;

    private Transform playerTransform;
    private Vector3 currentOffset;
    private float time = 0f;
    private float startTime = 1.0f;
    private float stopTime = 3.0f;
    private bool useCameraMove = false;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        IsometricCamera.Camera = gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        CheckIfClickOnScreenEdge();
    }

    public void CheckIfClickOnScreenEdge()
    {
        if (PauseManager.isPaused || PauseManager.isInteracting)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Vector2 screen = new Vector2(Screen.width, Screen.height);

            float minX = Screen.width * 0.2f;
            float maxX = Screen.width * 0.8f;
            float minY = Screen.height * 0.2f;
            float maxY = Screen.width * 0.8f;
            bool isOnEdge = (mouse.x < minX || maxX < mouse.x || mouse.y < minY || maxY < mouse.y);

            useCameraMove = isOnEdge;
            return;

            if (isOnEdge)
            {
                if (mouse.x < minX)
                    MoveCamera(ScreenSpaceDirection.left);
                else if (maxX < mouse.x)
                    MoveCamera(ScreenSpaceDirection.right);
                else if (maxY < mouse.y)
                    MoveCamera(ScreenSpaceDirection.up);
                else if (mouse.y < minY)
                    MoveCamera(ScreenSpaceDirection.down);
            }
        }
    }

    public void MoveCamera(ScreenSpaceDirection direction)
    {
        float moveSpeed = 10f;

        Vector3 moveDirection;
        if (direction == ScreenSpaceDirection.left)
            moveDirection = Camera.transform.rotation * Vector3.left * Time.deltaTime * moveSpeed;
        else if (direction == ScreenSpaceDirection.right)
            moveDirection = Camera.transform.rotation * Vector3.right * Time.deltaTime * moveSpeed;
        else if (direction == ScreenSpaceDirection.up)
            moveDirection = Camera.transform.rotation * Vector3.up * Time.deltaTime * moveSpeed;
        else
            moveDirection = Camera.transform.rotation * Vector3.down * Time.deltaTime * moveSpeed;

        transform.position = playerTransform.position + currentOffset + moveDirection;
    }

    private void LateUpdate()
    {
        SetOffset();
        if (useCameraMove)
            SmoothFollowCamera();

        //SmoothMoveCamera()
    }

    public void SetOffset()
    {
        //currentOffset = Quaternion.Euler(rotateAngles) * Vector3.right * distance;

        Vector3 targetOffset = cameraDirection.normalized * distance;
        Vector3 startingOffset = cameraDirection.normalized * startingDistance;
        currentOffset = Vector3.Lerp(startingOffset, targetOffset, Mathf.Clamp01((Time.time - startTime) / stopTime));
    }

    /* A simple follow camera */
    public void FollowCamera()
    {
        if (playerTransform.position == Vector3.zero)
            return;

        transform.position = playerTransform.position + currentOffset;
    }

    /* A simple follow camera with Smoothing */
    public void SmoothFollowCamera()
    {
        if (playerTransform.position == Vector3.zero)
            return;

        Vector3 desiredPosition = playerTransform.position + currentOffset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = currentPosition;
    }

    private float lerp_time = 0;
    public void SmoothMoveCamera(Vector3 start, Vector3 target, float move_time)
    {
        if (playerTransform.position == Vector3.zero)
            return;

        float lerp_fraction = Mathf.Clamp01((Time.time - lerp_time) / move_time);
        transform.position = Vector3.Lerp(target, target, lerp_fraction);
        lerp_time += Time.deltaTime;
    }
}


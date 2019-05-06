using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class IsometricCamera : MonoBehaviour
{

    public enum ScreenSpaceDirection { left, right, up, down };

    static public Camera Camera = null;              // Static reference to Camera 
    static private bool useLevelZoomIn = true;
    static private bool usePlayMode = false;
    static private float zoomInTime = 0;
    static public void StartLevelZoomIn() {
        useLevelZoomIn = true;
        usePlayMode = false;

        zoomInTime = Time.time;
        Debug.Log("StartLevelZoomIn");
    }
    static public void ResetLevelZoomIn() {
        useLevelZoomIn = false;
        usePlayMode = false;

        zoomInTime = 0;
        Debug.Log("ResetLevelZoomIn");
    }

    public float smoothSpeed = 2f;                  // Camera lerp smoothing speed parameter
    public Vector3 offset = new Vector3(1, 1, -1);  // Camera position offset
    public Vector3 cameraDirection = new Vector3(1, 1, -1);
    public float distance = 14;
    public float startingDistance = 28;

    private Transform playerTransform;
    private Vector3 currentOffset;
    private float time = 0f;
    private float startTime = 0.5f;
    private float stopTime = 2.0f;
    [SerializeField] 
    private bool useCameraMove = true;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        IsometricCamera.Camera = gameObject.GetComponent<Camera>();

    }

    private void Start()
    {
        moveOffset = Vector3.zero;
        useCameraMove = true;
        //currentOffset = cameraDirection.normalized * distance;
        //startPosition = transform.position;
        //targetPosition = playerTransform.position + currentOffset;
    }

    private void Update() {
        //CheckIfMouseOnEdge();
        //CheckForClickMove();
    }

    public void CheckForClickMove()
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
            float maxY = Screen.height * 0.8f;
            bool isOnEdge = (mouse.x < minX || maxX < mouse.x || mouse.y < minY || maxY < mouse.y);

            if (!useCameraMove)
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

    public void CheckIfMouseOnEdge()
    {
        Vector3 mouse = Input.mousePosition;
        Vector2 screen = new Vector2(Screen.width, Screen.height);

        float minX = Screen.width * 0.1f;
        float maxX = Screen.width * 0.9f;
        float minY = Screen.height * 0.1f;
        float maxY = Screen.height * 0.9f;
        bool isOnEdge = (mouse.x < minX || maxX < mouse.x || mouse.y < minY || maxY < mouse.y);

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

    public Vector3 moveOffset = Vector3.zero;
    public void MoveCamera(ScreenSpaceDirection direction)
    {
        float moveSpeed = 10f;

        Vector3 moveDirection;
        if (direction == ScreenSpaceDirection.left)
            moveDirection = Camera.transform.rotation * Vector3.left * Time.deltaTime * moveSpeed;
        else if (direction == ScreenSpaceDirection.right)
            moveDirection = Camera.transform.rotation * Vector3.right * Time.deltaTime * moveSpeed;
        else if (direction == ScreenSpaceDirection.up)
            moveDirection = Camera.transform.rotation * Vector3.forward * Time.deltaTime * moveSpeed;
        else
            moveDirection = Camera.transform.rotation * Vector3.back * Time.deltaTime * moveSpeed;

        moveOffset += moveDirection;
        //transform.position = transform.position + moveDirection;
    }

    Vector3 startPosition;
    Vector3 targetPosition;
    bool continueSmoothCamera = false;
    private void LateUpdate()
    {

        SetOffset();
        //if(useCameraMove)
        SmoothFollowCamera();


        /*
        if(useCameraMove) {
            startPosition = transform.position;
            targetPosition = playerTransform.position + currentOffset;
        }

        SmoothMoveCamera(startPosition, targetPosition, 1.0f);

        */

        /*
        if (useCameraMove) {
            SmoothFollowCamera();
        }
        */
    }

    public bool CameraReady() {
        return CameraDistanceFraction() >= 1.0 ? true : false;
    }

    public float CameraDistanceFraction() {
        //Debug.Log("Time: " + Time.time + " Start: " + startTime + " Stop:" + stopTime + " ZoomInTime:" + zoomInTime);
        return Mathf.Clamp01( (Time.time - zoomInTime - startTime) / stopTime);
    }

    public void SetOffset()
    {
        //currentOffset = Quaternion.Euler(rotateAngles) * Vector3.right * distance;

        Vector3 targetOffset = cameraDirection.normalized * distance;
        Vector3 startingOffset = cameraDirection.normalized * startingDistance;

        if (useLevelZoomIn) {
            float fraction = CameraDistanceFraction();
            if(CameraReady() && !usePlayMode) {
                usePlayMode = true;
                GameManager.State = GameState.Play;
            }

            //Debug.Log(fraction);
            currentOffset = Vector3.Lerp(startingOffset, targetOffset, fraction);
        }
        else
            currentOffset = startingOffset;
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

        Vector3 desiredPosition = playerTransform.position + currentOffset;// + moveOffset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = currentPosition;

        if ((desiredPosition - currentPosition).magnitude < 0.1f)
        {
            useCameraMove = false;
        }
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
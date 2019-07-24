using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SeedQuest.Experiment {
    
    public class IsometricCameraMove : MonoBehaviour
    {
        static public Camera Camera = null;                         // Static reference to Camera 

        public enum ScreenSpaceDirection { left, right, up, down };

        private Transform playerTransform;
        private bool useCameraMove = true;

        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Camera = IsometricCamera.Camera;
        }

        private void Start()
        {
            moveOffset = Vector3.zero;
            useCameraMove = true;
        }

        private void LateUpdate()
        {
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

        public void CheckForClickMove()
        {
            if (PauseManager.isPaused || PauseManager.isInteracting)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

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

        /* A simple follow camera with Smoothing */
        /*
        public void SmoothFollowCamera()
        {
            if (playerTransform.position == Vector3.zero)
                return;

            Vector3 desiredPosition = playerTransform.position + playerTransform.forward * lookAtPeek + currentOffset; // + moveOffset;
            Vector3 currentPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = currentPosition;

            if ((desiredPosition - currentPosition).magnitude < 0.1f)
            {
                useCameraMove = false;
            }
        } */

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
  
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class IsometricCharacter : MonoBehaviour {
    
    public float runSpeedMultiplier = 2;
    public float runClickDistance = 6;

    private NavMeshAgent agent;
    private float walkSpeed;

    public void Start() {
        agent = GetComponent<NavMeshAgent>();
        walkSpeed = agent.speed;
    }

    public void Update() {
        MoveAndRotateCharacterWithKeys();
        MoveWithClick();
        CheckIfWalkable();
    }
    
    public void SetAgentSpeed(Vector3 target) {
        float dist = GetDistance(transform.position, target);
        if (dist > runClickDistance)
            agent.speed = walkSpeed * runSpeedMultiplier;
        else
            agent.speed = walkSpeed;
    }

    public float GetDistance(Vector3 v1, Vector3 v2) {
        return (v1 - v2).magnitude;
    }

    // Basically tank controls
    public void MoveAndRotateCharacterWithKeys()
    {
        bool isRunning = false;
        float runSpeed = walkSpeed * runSpeedMultiplier;

        // Get move quantity based on delta time and speed
        float moveSpeed = 2 * (isRunning ? runSpeed : walkSpeed);
        moveSpeed = PauseManager.isPaused ? 0 : moveSpeed;
        float rotateSpeed = 200;

        float horizontal = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        if (vertical > 0.0f) {
            agent.isStopped = true;
            MarkerManager.DeleteMarker();
        }

        // Translate character
        //transform.Translate(moveHorizontal, 0, 0);
        transform.Translate(0, 0, vertical);
        transform.localRotation *= Quaternion.Euler(0f, horizontal, 0f);

        // Use Left Shift to Toggle Running
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = !isRunning;
        }
    }

    public void MoveWithClick() {
        if (PauseManager.isPaused || PauseManager.isInteracting)
            return;

        if (Input.GetMouseButtonDown(0)) {
            Camera camera = IsometricCamera.Camera;

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)) {
                    
                NavMeshHit navHit;
                int walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
                if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, walkableMask)) {
                    agent = GetComponent<NavMeshAgent>();
                    agent.isStopped = false;
                    agent.SetDestination(hit.point);
                    SetAgentSpeed(hit.point);
                    MarkerManager.GenerateMarker(hit.point + new Vector3(0, 0.1f, 0), Quaternion.identity);
                }
            }

        }
    }

    public void CheckIfWalkable() {
        Camera c = Camera.main;
        RaycastHit hit;
        Ray ray = c.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f)) {
            NavMeshHit navHit;
            int walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
            if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, walkableMask)) {
                //Cursor.SetCursor(cursorTextureWalking, Vector2.zero, CursorMode.Auto);
                //CursorActionUI.Show = true;
            }
            else {
                //Cursor.SetCursor(cursorTextureDefault, Vector2.zero, CursorMode.Auto);
                //CursorActionUI.Show = false;
            }
        }
        else {
            //Cursor.SetCursor(cursorTextureDefault, Vector2.zero, CursorMode.Auto);
            //CursorActionUI.Show = false;
        }
    }
}
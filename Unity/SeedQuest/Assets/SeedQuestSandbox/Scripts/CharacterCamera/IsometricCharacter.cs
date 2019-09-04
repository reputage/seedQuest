using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class IsometricCharacter : MonoBehaviour {
    public float baseMoveSpeed = 1.5f;
    public float runSpeedMultiplier = 2;
    public float runClickDistance = 6;
    public Animator animator;

    private NavMeshAgent agent;
    private float walkSpeed;

    public void Start() {
        agent = GetComponent<NavMeshAgent>();
        walkSpeed = agent.speed;
        animator = GetComponent<Animator>();
    }

    public void Update() {
        MoveAndRotateCharacterWithKeys();
        MoveWithClick();
        CheckIfWalkable();
        SetAnimation();
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
    public void MoveAndRotateCharacterWithKeys() {
        bool isRunning = false;
        float runSpeed = walkSpeed * runSpeedMultiplier;

        // Get move quantity based on delta time and speed
        float moveSpeed = baseMoveSpeed * (isRunning ? runSpeed : walkSpeed);
        moveSpeed = PauseManager.isPaused ? 0 : moveSpeed;

        // Get user input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Use animation if input exists 
        if (vertical != 0.0f || horizontal != 0.0f) {
            agent.isStopped = true;
            MarkerManager.DeleteMarker();
            if (animator != null)
                animator.SetBool("Walk", true);
        }
        else {
            if (animator != null)
                animator.SetBool("Walk", false);
        }

        // Translate character
        Vector3 cameraDir = IsometricCamera.instance.cameraDirection;
        Vector3 verticalMove = IsometricCamera.Camera.transform.forward * Mathf.Abs(vertical) * - Mathf.Sign(cameraDir.z);
        Vector3 hortizonalMove = IsometricCamera.Camera.transform.right * Mathf.Abs(horizontal) * Mathf.Sign(cameraDir.x);
        verticalMove.y = 0;
        verticalMove.x = 0;
        hortizonalMove.y = 0;
        hortizonalMove.x = 0;
        Vector3 moveVector = verticalMove + hortizonalMove;
      
        transform.Translate(moveVector.normalized * moveSpeed * Time.deltaTime);

        // Rotate character
        Vector3 dir = IsometricCamera.Camera.transform.forward * vertical + IsometricCamera.Camera.transform.right * horizontal;
        dir.y = 0;
        transform.LookAt(dir + transform.position);

        // Use Left Shift to Toggle Running
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            isRunning = !isRunning;
        }
    }

    public void MoveWithClick() {
        if (PauseManager.isPaused || PauseManager.isInteracting)
            return;

        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
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

    public void SetAnimation() {
        if (agent.isStopped)
            return;
        
        float velocity = agent.velocity.magnitude;
        if (velocity > 1.0f)
            animator.SetBool("Walk", true);
        else
            animator.SetBool("Walk", false);
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
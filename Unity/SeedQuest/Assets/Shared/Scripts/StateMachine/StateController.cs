using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State currentState;
    public State remainState;

    public Interactable[] pathTargets;
    public Interactable[] interactables;
    public GameStateData gameState;
    public GameObject NavAIMesh;

    [HideInInspector] public int nextWayPoint;

    private Pathfinding pathfinding;

    private void Awake() {
        pathfinding = NavAIMesh.GetComponent<Pathfinding>();
        InitializeState();
    }

    private void Update() {
        currentState.UpdateState(this);
    }

    private void InitializeState()
    {
        //pathTargets = FindInteractables();
        interactables = FindInteractables();

        Interactable[] locs = GetInteractableLocations(); // fixes the warning in the compiler
        pathTargets = GetInteractablePathTest(locs);

        gameState.startPathSearch = false; 
        gameState.pathComplete = false;
        gameState.currentAction = pathTargets[0];
        gameState.targetList = pathTargets;
    } 

    // Modified to create a list of gameobjects with interactable attached as monobehavior
    private Interactable[] GetInteractableLocations()
    {
        SeedToByte locations = NavAIMesh.GetComponent<SeedToByte>();
        int[] actions = locations.getActions(gameState.SeedString);
        List<Interactable> locationIDs = new List<Interactable>();

        int count = 0;
        while (count < actions.Length)
        {
            int siteID = actions[count];

            for (int j = 0; j < gameState.ActionCount; j++)
            {
                int spotID = actions[count + (2 * j) + 1];
                int actionID = actions[count + (2 * j) + 2];
                locationIDs.Add(new Interactable(siteID, spotID, actionID));
            }

            count += 1 + 2 * gameState.ActionCount;
        }

        return locationIDs.ToArray();
    }

    // Modified to use the gameobject list instead
    //  fixes the compiler errors
    //  other functions commented out just in case the other way was intentional
    private Interactable[] GetInteractablePathTest(Interactable[] locationIDs)
    {
        
        Interactable[] interactablePath = new Interactable[locationIDs.Length];

        int siteCount = (int)Mathf.Pow(2.0F, gameState.SiteBits);
        int spotCount = (int)Mathf.Pow(2.0F, gameState.SpotBits);

        Interactable[,] LUT = new Interactable[siteCount, spotCount];
        for (int i = 0; i < interactables.Length; i++)
        {
            int row = interactables[i].siteID;
            int col = interactables[i].spotID;
            LUT[row, col] = interactables[i];
        }

        for (int i = 0; i < locationIDs.Length; i++)
        {
            int row = locationIDs[i].siteID;
            int col = locationIDs[i].spotID;
            interactablePath[i] = LUT[row, col];
        }

        return interactablePath;
    }

    private Interactable[] FindInteractables() {
        Interactable[] actions = (Interactable[])Object.FindObjectsOfType(typeof(Interactable));
        for (int i = 0; i < actions.Length; i++)
        {
            Debug.Log(actions[i] + " site and spot: " + actions[i].siteID + " " + actions[i].spotID);
        }
        return actions;
    }

    public Vector3[] FindPath() {
        return pathfinding.FindPath(transform.position, pathTargets[nextWayPoint].transform.position);
    }

    public void DoActionAtInteractable(int actionIndex) {
        // Record action at interactable into action log
        Interactable interactable = pathTargets[nextWayPoint];
        gameState.actionLog.Add(interactable, interactable.actions[actionIndex]);

        // Go to next waypoint
        NextPath();
    }

    public void NextPath() {
        nextWayPoint++;

        if (nextWayPoint < pathTargets.Length)
            gameState.currentAction = pathTargets[nextWayPoint];
        else
            gameState.pathComplete = true;
    }

    public void DrawPath(Vector3[] path) {
        LineRenderer line = NavAIMesh.GetComponentInChildren<LineRenderer>();
        line.positionCount = path.Length;
        line.SetPositions(path);
    }

    public bool isNearTarget() {
        if (nextWayPoint >= pathTargets.Length)
            return false;

        Vector3 dist = transform.position - pathTargets[nextWayPoint].transform.position;

        if (dist.magnitude < gameState.interactionRadius)
            return true;
        else
            return false;
    }

    public bool isNearInteractable() {
        return Physics.CheckSphere(transform.position, gameState.interactionRadius, gameState.interactableMask);
    }

    public Interactable getNearestInteractable() {
        Interactable nearest = interactables[0];
        float nearDist = float.MaxValue;

        for (int i = 0; i < interactables.Length; i++) {
            float dist = (transform.position - interactables[i].transform.position).magnitude;
            if(dist < nearDist) {
                nearDist = dist;
                nearest = interactables[i];
            }
        }

        return nearest;
    }

    public void checkIsNearTarget() {
        if (isNearTarget()) {
            gameState.showPathTooltip = true;

            if(Input.GetButtonDown("Jump")) {
                DoActionAtInteractable(0);
            }
            else if(Input.anyKey){
                DoActionAtInteractable(1);
            }
        }
        else
            gameState.showPathTooltip = false;
    }

    private void OnDrawGizmos() {
        if(currentState != null) {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, gameState.interactionRadius);
        }

        if(isNearTarget()) {
            Gizmos.DrawWireSphere(gameState.currentAction.transform.position, gameState.interactionRadius);
        }
    }

    public void TransitionToState(State nextState) {
        if (nextState != remainState)
            currentState = nextState;
    }

}

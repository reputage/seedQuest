using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State currentState;
    public State remainState;

    public Interactable[] pathTargets;
    public Interactable[] pathTargets2;
    public Interactable[] interactables;
    public GameStateData playerPathData;
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
        pathTargets = FindInteractables();
        interactables = FindInteractables();

        //List<Interactable> locs = GetInteractableLocations();
        List<GameObject> locs = GetInteractableLocationsTest(); // fixes the warning in the compiler
        //pathTargets = GetInteractablePath(locs);
        pathTargets = GetInteractablePathTest(locs);

        playerPathData.startPathSearch = false; 
        playerPathData.pathComplete = false;
        playerPathData.currentAction = pathTargets[0];
        playerPathData.targetList = pathTargets;
    } 

    // Modified to create a list of gameobjects with interactable attached as monobehavior
    private List<GameObject> GetInteractableLocationsTest()
    {
        SeedToByte locations = NavAIMesh.GetComponent<SeedToByte>();
        int[] actions = locations.getActions("AAAAAAAAAAAAAAAA");
        List<GameObject> locationIDs = new List<GameObject>();

        int count = 0;
        while (count < actions.Length)
        {
            int siteID = actions[count];

            for (int j = 0; j < 4; j++)
            {
                int spotID = actions[count + (2 * j) + 1];
                int actionID = actions[count + (2 * j) + 2];

                locationIDs.Add(new GameObject());
                locationIDs[(count * 4 / 9) + j].AddComponent<Interactable>();

                locationIDs[(count * 4 / 9) + j].GetComponent<Interactable>().siteID = siteID;
                locationIDs[(count * 4 / 9) + j].GetComponent<Interactable>().spotID = spotID;
                locationIDs[(count * 4 / 9) + j].GetComponent<Interactable>().actionID = actionID;

            }

            count += 9;
        }

        return locationIDs;
    }

    // Modified to use the gameobject list instead
    //  fixes the compiler errors
    //  other functions commented out just in case the other way was intentional
    private Interactable[] GetInteractablePathTest(List<GameObject> locationIDs)
    {

        Interactable[] interactablePath = new Interactable[locationIDs.Count];

        Interactable[,] LUT = new Interactable[16, 16];
        for (int i = 0; i < interactables.Length; i++)
        {
            int row = interactables[i].siteID;
            int col = interactables[i].spotID;
            LUT[row, col] = interactables[i];
        }

        for (int i = 0; i < locationIDs.Count; i++)
        {
            int row = locationIDs[i].GetComponent<Interactable>().siteID;
            int col = locationIDs[i].GetComponent<Interactable>().spotID;
            interactablePath[i] = LUT[row, col];
            //Debug.Log(interactablePath[i]);
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

    public void NextPath() {
        nextWayPoint++;

        if (nextWayPoint < pathTargets.Length)
            playerPathData.currentAction = pathTargets[nextWayPoint];
        else
            playerPathData.pathComplete = true;
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

        if (dist.magnitude < playerPathData.interactionRadius)
            return true;
        else
            return false;
    }

    public bool isNearInteractable() {
        return Physics.CheckSphere(transform.position, playerPathData.interactionRadius, playerPathData.interactableMask);
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
            playerPathData.showPathTooltip = true;

            if(Input.GetButtonDown("Jump")) {
                NextPath();
            }
        }
        else
            playerPathData.showPathTooltip = false;
    }

    private void OnDrawGizmos() {
        if(currentState != null) {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, playerPathData.interactionRadius);
        }

        if(isNearTarget()) {
            Gizmos.DrawWireSphere(playerPathData.currentAction.transform.position, playerPathData.interactionRadius);
        }
    }

    public void TransitionToState(State nextState) {
        if (nextState != remainState)
            currentState = nextState;
    }

}

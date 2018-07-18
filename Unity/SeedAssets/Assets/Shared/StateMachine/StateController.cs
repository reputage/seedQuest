using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State currentState;
    public Interactable[] pathTargets;
    public PathData playerPathData;
    public GameObject NavAIMesh;

    [HideInInspector] public int nextWayPoint;

    private Pathfinding pathfinding;

    private void Awake() {
        pathfinding = NavAIMesh.GetComponent<Pathfinding>();
        pathTargets = FindInteractables();
    }

    private void Update() {
        currentState.UpdateState(this);
    }

    private Interactable[] FindInteractables() {
        return (Interactable[])Object.FindObjectsOfType(typeof(Interactable));
    }

    public Vector3[] FindPath() {
        return pathfinding.FindPath(transform.position, pathTargets[nextWayPoint].transform.position);
    }

    public void NextPath() {
        nextWayPoint++;
    }

    public void DrawPath(Vector3[] path) {
        LineRenderer line = NavAIMesh.GetComponentInChildren<LineRenderer>();
        line.positionCount = path.Length;
        line.SetPositions(path);
    }

    private void OnDrawGizmos() {
        if(currentState != null) {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, playerPathData.interactionRadius);
        }
    }

}

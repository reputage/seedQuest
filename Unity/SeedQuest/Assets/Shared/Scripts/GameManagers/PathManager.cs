using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

    public GameObject PathMesh = null;
    public Interactable[] path = null;
    private int pathTargetIndex = 0;
    private Vector3[] pathWaypoints = null;

    static public Interactable[] Path { get { return Instance.path; } }

    static public Interactable PathTarget {
        get {
            if (Path == null || Path.Length == 0)
                return null;
            return Path[Instance.pathTargetIndex];
        }
    }

    static public PathManager instance = null;

    static public PathManager Instance {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<PathManager>();
            return instance;
        }
    } 

    void Update () { 
        if(GameManager.State == GameState.Rehearsal) {
            CreatePath();
            FindPath();
            DrawPath(); 
        }
	}

    private void CreatePath()
    {
        if (Path == null || Path.Length == 0) {
            SeedConverter converter = new SeedConverter();
            path = converter.encodeSeed(SeedManager.InputSeed);
        }

    }

    private void FindPath() 
    {
        Pathfinding pathfinder = PathMesh.GetComponent<Pathfinding>();
        Vector3 player = PlayerManager.GetPlayer().position;
        Vector3 target = PathTarget.transform.position;
        pathWaypoints = pathfinder.FindPath(player, target);
    }

    private void nextWaypoint()
    {
        if (pathTargetIndex + 1 < Path.Length)
            pathTargetIndex++;
        else
            GameManager.State = GameState.GameEnd;
    }

    private void DrawPath()
    {
        if (Path == null)
            return;

        LineRenderer line = PathMesh.GetComponentInChildren<LineRenderer>();
        line.positionCount = Path.Length;
        line.SetPositions(pathWaypoints);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

    /// <summary> Reference to Gameobject with Pathfinding and Grid scripts </summary>
    public GameObject PathMesh = null;
    /// <summary> Path that represents the order of correct interactions to be completed </summary>
    public Interactable[] path = null;
    /// <summary> Current path target interactable index </summary>
    private int pathSegmentIndex = 0;
    /// <summary> Array of position representing a PathSegment </summary>
    private Vector3[] pathSegment = null;

    /// <summary> Path that represents the order of correct interactions to be completed </summary>
    static public Interactable[] Path { get { return Instance.path; } } 

    /// <summary> Current path target interactable </summary>
    static public Interactable PathTarget {
        get {
            if (Path == null || Path.Length == 0)
                return null;
            return Path[Instance.pathSegmentIndex];
        }
    }

    /// <summary> Reference to singleton Instance of PathManager </summary>
    static public PathManager instance = null;

    /// <summary> Instance property reference for PathManager </summary>
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
            FindPathSegment();
            DrawPathSegment(); 
        }
	}

    /// <summary> Creates an Interactable Path from encoding a seed string </summary>
    private void CreatePath()
    {
        if (Path == null || Path.Length == 0) {
            SeedConverter converter = new SeedConverter();
            path = converter.encodeSeed(SeedManager.InputSeed);
        }
    }

    /// <summary> Generates a Vector3[] for a PathSegment </summary>
    private void FindPathSegment() 
    {
        if (PathTarget == null)
            return;

        Pathfinding pathfinder = PathMesh.GetComponent<Pathfinding>();
        Vector3 player = PlayerManager.Position;
        Vector3 target = PathTarget.transform.position;
        pathSegment = pathfinder.FindPath(player, target);
    }

    /// <summary> Draws a PathSegment using a LineRenderer </summary>
    private void DrawPathSegment()
    {
        if (Path == null || PathTarget == null)
            return;

        LineRenderer line = PathMesh.GetComponentInChildren<LineRenderer>();
        line.positionCount = pathSegment.Length;
        line.SetPositions(pathSegment);
    }

    /// <summary> Goes to next PathSegement, next update will generate and draw the next PathSegment </summary>
    static public void NextPathSegment()
    {
        if (GameManager.State != GameState.Rehearsal)
            return;

        if (Instance.pathSegmentIndex + 1 < Path.Length)
            Instance.pathSegmentIndex++;
        else
            GameManager.State = GameState.GameEnd;
    }
}
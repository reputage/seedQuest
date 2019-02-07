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

    static public Interactable LastPathTarget {
        get { 
            if (Path == null || Path.Length == 0)
                return null;
            else if (Instance.pathSegmentIndex == 0)
                return null;
            else
                return Path[Instance.pathSegmentIndex - 1];
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

    private void Start()
    {
        InvokeRepeating("FindPathSegment", 0f, 0.25f);
    }

    void Update () { 
        if(GameManager.State == GameState.LoadingRehersal) {
            //CreatePath();
        }

        if(GameManager.State == GameState.Rehearsal) {
            CreatePath();
            //FindPathSegment();
            DrawPathSegment(); 
        }

        if (GameManager.State == GameState.GameStart)
            ClearPathSegments();
	}

    public void Reset()
    {
        path = null;
        pathSegmentIndex = 0;
        pathSegment = null;
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

        if (GameManager.State != GameState.Rehearsal)
            return;
        
        Pathfinding pathfinder = PathMesh.GetComponent<Pathfinding>();
        Vector3 player = PlayerManager.Position;
        Vector3 target = PathTarget.transform.position;
        pathSegment = pathfinder.FindPath(player, target);
    }

    /// <summary> Draws a PathSegment using a LineRenderer </summary>
    private void DrawPathSegment()
    {
        if (Path == null || PathTarget == null || pathSegment == null)
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
            Instance.StartCoroutine(endGame());
    }

    static private IEnumerator endGame()
    {
        yield return new WaitForSeconds(1f);

        GameManager.State = GameState.GameEnd;
    }

    static public void PreviousPathSegment()
    {
        if (GameManager.State != GameState.Rehearsal)
            return;

        if (Instance.pathSegmentIndex - 1 >= 0)
            Instance.pathSegmentIndex--;
        else
            return;
    }

    private void ClearPathSegments() {
        LineRenderer line = PathMesh.GetComponentInChildren<LineRenderer>();
        line.positionCount = 0;
    } 

    public void ResetPathManager()
    {
        PathMesh = null;
        path = null;
        pathSegmentIndex = 0;
        pathSegment = null;
    }
}


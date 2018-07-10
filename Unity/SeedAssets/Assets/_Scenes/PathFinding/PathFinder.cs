using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    public Transform target;
    public LineRenderer line;

    Pathfinding pathfinding;
    Vector3[] path;

    private void Start()
    {
        //Vector3[] waypoints = FindPath(transform.position, target.position);
        //if (waypoints.Length > 0)
        //    DrawPath();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSucessful)
    {
        if (pathSucessful)
        {
            path = newPath;
            DrawPath();
        }
    }

    void DrawPath()
    {
        line.positionCount = path.Length;
        line.SetPositions(path);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    static PathFinder instance;
    public Transform player;
    public List<Transform> targetList = new List<Transform>();
    public LineRenderer line;

    private Pathfinding pathfinding;
    private Vector3[] path;
    private int currentIndex = 0;

    private void Awake() {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    private void Start() {
        InvokeRepeating("UpdatePath", 0, 1.0F);
    }

    private void UpdatePath() {
        path = pathfinding.FindPath(player.position, targetList[currentIndex].position);
        if (path.Length > 0)
            DrawPath();
    }

    public void NextPath() {
        currentIndex++;
    }

    void DrawPath() {
        line.positionCount = path.Length;
        line.SetPositions(path);
    }
}

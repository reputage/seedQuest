using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

    Queue<PathRequest> pathQueue = new Queue<PathRequest>();
    PathRequest pathRequest;

    static PathManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    private void Awake() {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if(!isProcessingPath && pathQueue.Count > 0) {
            pathRequest = pathQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(pathRequest.pathStart, pathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success) {
        pathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}

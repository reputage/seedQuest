using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Transform target;
    public LineRenderer line;

    public float speed = 2.0F;
    public float rotateSpeed = 1.0F;
    Vector3[] path;
    int targetIndex;

    private void Start() {
        PathManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSucessful) {
        if(pathSucessful) {
            path = newPath;
            DrawPath();
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while(true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if(targetIndex >= path.Length) {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            // Move to currentWaypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            // Look at currentWaypoint (using Lerp)
            Vector3 dir = currentWaypoint - transform.position;
            if(dir.sqrMagnitude > 0) {
                Quaternion rotatation = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotatation, rotateSpeed * Time.deltaTime);
            } 

            yield return null;
        }
    }

    void DrawPath() {
        line.positionCount = path.Length;
        line.SetPositions(path);
    }
}

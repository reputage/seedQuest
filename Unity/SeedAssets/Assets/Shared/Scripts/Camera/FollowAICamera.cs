using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAICamera : MonoBehaviour {

    public Transform target;
    public float distance = 2f;
    public Vector3 offset = new Vector3(0.0f, 2.0f, 0.0f);
    public Vector3 lookAtOffset = new Vector3(0.0f, 1.0f, 0.0f);

    private void LateUpdate()
    {
        transform.position = target.position + offset + (- distance * target.forward);
        transform.LookAt(target.position + lookAtOffset);
    }
}

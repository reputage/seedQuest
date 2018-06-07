using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCamera : MonoBehaviour {
    public GameObject target;
    public float damping = 1;
    Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 position = target.transform.position + offset;
        Vector3 lerpPosition = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
        transform.position = lerpPosition;

        transform.LookAt(target.transform.position);
    }
}

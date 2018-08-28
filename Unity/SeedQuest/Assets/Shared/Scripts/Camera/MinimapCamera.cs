using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {

    public GameObject player;
    public GameObject target;

    public Vector3 cameraOffset;


	void Start () 
    {
        cameraOffset.y = 100;	
	}
	

	void LateUpdate () 
    {
        transform.position = player.transform.position;
        transform.position += cameraOffset;
	}
}

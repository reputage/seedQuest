using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public bool useRotate = false;
    public float rotateSpeed = 0.4f;
    public Vector3 r0 = new Vector3(50, -90, 0);
    public Vector3 r1 = new Vector3(50, 90, 0);

    private float time = 0;
	
	// Update is called once per frame
	void Update () {
        if(useRotate)
        { 
            time += Time.deltaTime;
            Vector3 rotate = Vector3.Lerp(r0, r1, Mathf.Abs(Mathf.Sin(rotateSpeed * time)));
            transform.localRotation = Quaternion.Euler(rotate);
        } 
	}
}

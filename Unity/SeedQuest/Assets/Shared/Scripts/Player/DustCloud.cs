using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCloud : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 start;
    public Vector3 rotate;


	void Start () 
    {
        // Temporary, for testing:
        initialize(new Vector3(1, 1, 1), new Vector3(-24, 0, 5), new Vector3(1, 1, 1));

        transform.position = start;	
	}
	

    void Update () 
    {
        transform.position += velocity * Time.fixedDeltaTime; // not sure if I need the time var here
        transform.Rotate(rotate * Time.fixedDeltaTime);
	}


    void initialize(Vector3 vel, Vector3 st, Vector3 rot)
    {
        velocity = vel;
        start = st;
        rotate = rot;
    }
}

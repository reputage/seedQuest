using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCloud : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 start;
    public Vector3 rotate;

    public int selfDestruct;


	void Start () 
    {
        // Temporary, for testing:
        initialize(new Vector3(0, 1, 1), new Vector3(-24, 0, 5), new Vector3(0, 0, 0));

        transform.position = start;
        selfDestruct = 0;
	}
	

    void Update () 
    {
        transform.position += velocity * Time.fixedDeltaTime; // not sure if I need the time var here
        transform.Rotate(rotate * Time.fixedDeltaTime);
        transform.localScale += new Vector3(0.005F, 0.005f, 0.005f);
        selfDestruct += 1;

        if (selfDestruct == 20)
        {
            Destroy(gameObject);
        }
	}


    void initialize(Vector3 vel, Vector3 st, Vector3 rot)
    {
        velocity = vel + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.0f, 0.50f), Random.Range(-0.3f, 0.3f));
        start = st - new Vector3(Random.Range(0.0f, 0.3f), Random.Range(0.0f, 0.3f), Random.Range(0.0f, 0.1f));
        rotate = rot - new Vector3(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f));
    }
}

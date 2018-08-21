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
        initialize(new Vector3(0, 1, 1), gameObject.transform.position, new Vector3(0, 0, 0));

        transform.position = start;
        selfDestruct = 0;

	}
	

    void Update () 
    {
        transform.position += velocity * Time.fixedDeltaTime; // not sure if I need the time var here
        transform.Rotate(rotate * Time.fixedDeltaTime);
        transform.localScale += new Vector3(0.004F, 0.004f, 0.004f);
        selfDestruct += 1;

        if (selfDestruct == 20)
        {
            Destroy(gameObject);
        }
	}


    void initialize(Vector3 vel, Vector3 st, Vector3 rot)
    {
        velocity = vel + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0.0f, 0.30f), Random.Range(-0.2f, 0.2f));
        start = st - new Vector3(Random.Range(0.0f, 0.3f), Random.Range(0.0f, 0.3f), Random.Range(0.0f, 0.1f));
        rotate = rot - new Vector3(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f));
    }
}

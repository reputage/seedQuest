using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compassWaypoint : MonoBehaviour {


    public GameObject player;
    public GameObject target;

    Vector3 targetDir;
    Vector3 forward;

    // To do:
    // find the angle between the player's forward facing line and th target
    // update the x position of the waypoint to reflect the angle
    // if the angle is > some amount (60 degrees?) then don't update the position


	// Use this for initialization
	void Start ()
    {
        Vector3 forward = transform.forward;
	}
	
	// Update is called once per frame
	void Update () 
    {
        targetDir = target.transform.position - player.transform.position;

        float angle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        Debug.Log("angle: " + angle + " player angle: " + player.transform.rotation);


        /*
        Vector3 cross = Vector3.Cross(player.transform.position, target.transform.position);
        float dot = Vector3.Dot(player.transform.position, target.transform.position);
        Debug.Log(cross);
        */
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compassWaypoint : MonoBehaviour {


    public GameObject player;
    public GameObject target;

    public int xRange;
    public int angleRange;
    public float yHeight;

    Vector3 targetDir;
    Vector3 forward;

    RectTransform rPosition;

    // To do:
    // update the x position of the waypoint to reflect the angle
    // if the angle is > some amount (60 degrees?) then don't update the position


	// Use this for initialization
	void Start ()
    {
        forward = transform.forward;
        xRange = 100;
        yHeight = 170;
        angleRange = 50;

        rPosition = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        targetDir = target.transform.position - player.transform.position;

        float angle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        float playerAngle = player.transform.eulerAngles.y;

        // Don't ask me why this is necessary, it just is.

        if (playerAngle > 180)
        {
            playerAngle -= 360; //  playerAngle;
        }

        // Don't ask me why this is necessary either. 
        // Just don't change it unless you know what you're doing.
        float angleDiff = angle - playerAngle;
        if (angleDiff > 180)
        {
            angleDiff -= 360;
        }
        if (angleDiff < -180)
        {
            angleDiff += 360;
        }

        Debug.Log("angleDiff: " + angleDiff + " angle: " + angle + " playerAngle: " + playerAngle);


        if (angleDiff > -50)
        {
            float newX = (angleDiff);
            Debug.Log(newX);
            reposition(newX);
        }
        else if(angleDiff < 50)
        {
            float newX = (angleDiff);
            Debug.Log(newX);
            reposition(newX);
        }


	}

    void reposition(float newX)
    {
        rPosition.anchoredPosition = new Vector3(newX, yHeight, 0);
    }
}


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


	void Start ()
    {
        initialize();
    }
	

	void Update () 
    {
        updateWaypoint();
	}

    // Set the parameters for xRange, yHeight, angleRange, and find necessary components
    void initialize()
    {
        forward = transform.forward;
        rPosition = GetComponent<RectTransform>();

        xRange = 45;
        yHeight = 170;
        angleRange = 50;
    }

    // This calculates the angles for direction the player is facing, 
    //  angle between the player and the target, and sets the 
    //  marker's X position based on both angles.
    void updateWaypoint()
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
            angleDiff -= 360;
        if (angleDiff < -180)
            angleDiff += 360;
       
        //Debug.Log("angleDiff: " + angleDiff + " angle: " + angle + " playerAngle: " + playerAngle);

        if (angleDiff > - angleRange)
        {
            float newX = (angleDiff / angleRange) * xRange;
            reposition(newX);
        }
        else if (angleDiff < angleRange)
        {
            float newX = (angleDiff / angleRange) * xRange;
            reposition(newX);
        }
    }

    // Repositions the marker. This is called in the above function.
    void reposition(float newX)
    {
        rPosition.anchoredPosition = new Vector3(newX, yHeight, 0);
    }

    // Sets a new target for the compass waypoint to track. Needs to be tested.
    void setTarget(GameObject newTarget)
    {
        target = newTarget;
    }

}


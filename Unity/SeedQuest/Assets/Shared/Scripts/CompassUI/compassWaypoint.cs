using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compassWaypoint : MonoBehaviour {

    public Interactable target;
    
    public int xRange;
    public int xMax;
    public int angleRange;
    public int counter;
    public float yHeight;

    Vector3 targetDir;
    Vector3 forward;

    RectTransform rPosition;

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
        setTarget(PathManager.PathTarget);

        xRange = 125;
        xMax = 250;
        yHeight = 225;
        angleRange = 50;
        counter = 0;

     }

    // This calculates the angles for direction the player is facing, 
    //  angle between the player and the target, and sets the 
    //  marker's X position based on both angles.
    void updateWaypoint()
    {
        counter += 1;

        targetDir = target.transform.position - PlayerManager.Position;

        Transform angles = CameraStateController.Transform;

        float angle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        float playerAngle = angles.eulerAngles.y;

        // These calculations are required, if you change this code, 
        //  please double-check to make sure it still works properly.
        if (playerAngle > 180)
        {
            playerAngle -= 360; //  playerAngle;
        }

        float angleDiff = angle - playerAngle;
        if (angleDiff > 180)
            angleDiff -= 360;
        if (angleDiff < -180)
            angleDiff += 360;
       
        if (angleDiff > - angleRange)
        {
            float newX = (angleDiff / angleRange) * xRange;
            if(newX < xMax)
                reposition(newX);
        }
        else if (angleDiff < angleRange)
        {
            float newX = (angleDiff / angleRange) * xRange;
            if(newX > -xMax)
                reposition(newX);
        }

        if (counter >= 10)
        {
            counter = 0;
            setTarget(PathManager.PathTarget);
        }
    }

    // Repositions the marker. This is called in the above function.
    void reposition(float newX)
    {
        rPosition.anchoredPosition = new Vector3(newX, yHeight, 0);
    }

    // Sets a new target for the compass waypoint to track. Needs to be tested.
    void setTarget(Interactable newTarget)
    {
        target = newTarget;
    }

}
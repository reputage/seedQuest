using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass_tickbar : MonoBehaviour {

    public GameObject player;

    Vector3 forward;

    RectTransform rPosition;

    public int pixelLoop;
    public int yHeight;
    public int upperXLimit;
    public int lowerXLimit;
    public int xRange;


    void Start () 
    {
        initialize();
	}
	

    void Update () 
    {
        updatePosition();
	}


	private void updatePosition()
	{
        float playerAngle = player.transform.eulerAngles.y;
        float newX = (playerAngle - 180) / 360 * xRange;
        if (newX < lowerXLimit)
        {
            newX += xRange;
        }
        else if (newX > upperXLimit)
        {
            newX -= xRange;
        }

        reposition(newX);
	}


	private void initialize()
    { 
        forward = transform.forward;
        rPosition = GetComponent<RectTransform>();
        yHeight = 0;
        upperXLimit = 154;
        lowerXLimit = -154;
        xRange = upperXLimit - lowerXLimit;
    }


    private void reposition(float newX)
    {
        rPosition.anchoredPosition = new Vector3(newX, yHeight, 0);
    }

}

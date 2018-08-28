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

	// Use this for initialization
	void Start () 
    {
        initialize();
	}
	
	// Update is called once per frame
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
            Debug.Log("Looping tickbar. NewX val: " + newX);
            newX += xRange;
        }
        else if (newX > upperXLimit)
        {
            Debug.Log("Looping tickbar. NewX val: " + newX);
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

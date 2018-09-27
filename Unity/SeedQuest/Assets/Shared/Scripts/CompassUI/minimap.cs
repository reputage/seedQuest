using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{

    public float yScale;
    public float xScale;

    public int xOffset;
    public int yOffset;

    private int counter;

	private void Start()
	{
        scale();
	}

	void Update()
    {
        // Move the image around to simulate the camera following the player.
        transform.position = new Vector3(-(PlayerManager.Transform.position.x * xScale) + xOffset, -(PlayerManager.Transform.position.z * yScale) + yOffset, 0);
    
    }

    void scale()
    {
        RectTransform rt = GetComponent<RectTransform>();

        rt.sizeDelta = new Vector2(1250 * Screen.width / 1024f, 1250 * Screen.height / 768f);

        xOffset = 865 * Screen.width / 1024; // This is to make it work 
        yOffset = 140 * Screen.height / 768; //  independently of screen resolution

        xScale = 7.85f * Screen.width / 1024;
        yScale = 5.25f * Screen.height / 768;

        Debug.Log("xOffset: " + xOffset + "  xScale: " + xScale);
        Debug.Log("yOffset: " + yOffset + "  yScale: " + yScale);

        Debug.Log("Screen width: " + Screen.width + "  Screen height: " + Screen.height);

        // xOffset for 5:4 : 350 (screen width = 510) calculated: 430 diff : -80 (-15.7%)
        // yOffset for 5:4 : 130 (screen height = 408) calculated: 74 diff : +56 (+13.7%)

    }
}


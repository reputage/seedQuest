using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{

    public float yScale;
    public float xScale;

    public int xOffset;
    public int yOffset;


    void Update()
    {
        // Move the image around to simulate the camera following the player.
        transform.position = new Vector3(-(PlayerManager.Transform.position.x * xScale) + xOffset, -(PlayerManager.Transform.position.z * yScale) + yOffset, 0);
    }
}


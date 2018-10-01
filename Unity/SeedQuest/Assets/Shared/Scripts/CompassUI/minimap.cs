using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{

    //public Transform player = PlayerManager.Transform;

    public float yScale;
    public float xScale;

    public int xOffset;
    public int yOffset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(-(PlayerManager.Transform.position.x * xScale) + xOffset, -(PlayerManager.Transform.position.z * yScale) + yOffset, 0);
    }
}


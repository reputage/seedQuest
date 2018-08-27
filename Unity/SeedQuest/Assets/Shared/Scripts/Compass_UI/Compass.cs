using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour {
    
    public GameObject target;
    private Vector3 startPosition;

    public float numberOfPixelsNorthToNorth;
    float rationAngleToPixel;


    void Start()
    {
        startPosition = transform.position;
        rationAngleToPixel = numberOfPixelsNorthToNorth / 360f;
    }


    void Update()
    {
        Vector3 perp = Vector3.Cross(Vector3.forward, target.transform.forward);
        float dir = Vector3.Dot(perp, Vector3.up);
        transform.position = startPosition + (new Vector3(Vector3.Angle(target.transform.forward, Vector3.forward) * Mathf.Sign(dir) * rationAngleToPixel, 0, 0));
    }

}

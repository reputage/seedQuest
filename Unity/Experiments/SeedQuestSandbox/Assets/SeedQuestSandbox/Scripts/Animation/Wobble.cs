using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour {

    public float wobbleStrength = 10;
    public float wobbleSpeed = 5;

    void Update () {
        transform.Translate(Vector3.up * Mathf.Sin(wobbleSpeed * Time.time)); 
	}
}
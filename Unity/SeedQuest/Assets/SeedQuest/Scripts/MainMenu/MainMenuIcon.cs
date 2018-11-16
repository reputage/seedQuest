using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuIcon : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(new Vector3(45, 30, 0) * Time.deltaTime);
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime, Space.World);
	}
}

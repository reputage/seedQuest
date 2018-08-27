using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniPlayerIcon : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        transform.rotation = Quaternion.Euler(0, 0, -player.transform.eulerAngles.y);
	}
}

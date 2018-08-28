using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniPlayerIcon : MonoBehaviour {

    public GameObject player;

	void Start () 
    {
		
	}
	
	void LateUpdate () 
    {
        transform.rotation = Quaternion.Euler(0, 0, -player.transform.eulerAngles.y);
	}
}

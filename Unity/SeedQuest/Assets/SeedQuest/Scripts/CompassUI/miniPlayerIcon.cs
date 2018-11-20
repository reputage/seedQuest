using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniPlayerIcon : MonoBehaviour {

	void LateUpdate () 
    {
        transform.rotation = Quaternion.Euler(0, 0, -PlayerManager.Transform.eulerAngles.y);
	}
}
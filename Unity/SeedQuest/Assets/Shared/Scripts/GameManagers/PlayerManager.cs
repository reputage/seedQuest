using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    static public PlayerManager instance = null;
    public Transform player;

	// Use this for initialization
	void Awake () {
        instance = this;
	}
	
    static public Transform GetPlayer() {
        return instance.player;
    }
}

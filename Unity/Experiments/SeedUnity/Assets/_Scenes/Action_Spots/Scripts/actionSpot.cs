using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionSpot : MonoBehaviour {

    public GameObject item;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playerAlert(){
        //Debug.Log("player alert at aciton spot");
        item.GetComponent<item>().activateGlow();
    }

    public void playerClear()
    {
        //Debug.Log("player clear at aciton spot");
        item.GetComponent<item>().deactivateGlow();
    }

}


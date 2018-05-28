using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionOperator : MonoBehaviour {

    public GameObject actionMenu;
    public GameObject resultMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void activateMenu(){
        actionMenu.gameObject.SetActive(true);
    }

    public void actionButton(){

        //do something
        Debug.Log("action button pressed");
        actionMenu.gameObject.SetActive(false);
        resultMenu.gameObject.SetActive(true);

    }

    public void backButton() {
        //do something
        Debug.Log("back button pressed");
        actionMenu.gameObject.SetActive(false);
    }

    public void resultButton(){
        Debug.Log("result button clicked");
        resultMenu.gameObject.SetActive(false);
    }

}

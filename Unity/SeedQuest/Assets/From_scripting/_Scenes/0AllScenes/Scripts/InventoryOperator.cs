using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOperator : MonoBehaviour {

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    /* public GameObject item5;
    public GameObject item6;
    public GameObject item7;
    public GameObject item8; */

    private int index = 0;

	// Use this for initialization
	void Start () {
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);
        item4.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void addItem(int ID){
        
        switch (ID)
        {
            case 100001: // rock
                // do something
                //index += 1;
                item2.SetActive(true);
                break;
            case 100002: // soccer ball
                //index += 1;
                item1.SetActive(true);
                break;
            default:
                break;
                
        }


    }

    public void removeItem(){
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLog : MonoBehaviour {

    // This script is used to tack player actions

    public int actCount = 0;
    public int[] actionArr = new int[10];
   /* public int action1 = 0;
    public int action2 = 0;
    public int action3 = 0;
    public int action4 = 0; */

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void actionLogger(int actionInt)
    {
        actionArr[actCount] = actionInt;
        Debug.Log("Action successfully logged! ID: " + actionInt);
        actCount += 1;
    }


    //For removing the last performed action
    public void actionRemove()
    {

    }

    // Testing to see if the Blake2 hashing algorithm will work in unity
    // WIP
    public void hashTest()
    {
        //string str = "The quick brown fox jumps over the lazy dog";
        //byte[] pbText = Encoding.Default.GetBytes(str);
        //Blake512 blake512 = new Blake512();
        //byte[] pbHash = blake512.ComputeHash(pbText);
    }

}

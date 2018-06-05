using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLog : MonoBehaviour {


    /*
     Action location: 1-256
     Action spot: 1-32
     Action taken: 1-8
     
     Location: index 8 * 1
     Spot: index * 1000
     Taken action: index * 100,000

     Ex: location 199 * 1 = 199
     spot 15 * 1000 = 15000
     action 4 * 100000 = 400000
     number: 415,199

     Alternatively,
     199,154

     This isn't set in stone, just a possible solution
     
     */

    public int actCount = 0;
    public int action1 = 0;
    public int action2 = 0;
    public int action3 = 0;
    public int action4 = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void actionLogger(int actionInt)
    {

        switch (actCount)
        {
            case 0:
                //Log action
                action1 = actionInt;
                actCount += 1;
                Debug.Log("Action successfully logged! ID: " + actionInt);
                break;
            case 1:
                //Log action
                action2 = actionInt;
                actCount += 1;
                Debug.Log("Action successfully logged! ID: " + actionInt);
                break;
            case 2:
                //Log action
                action3 = actionInt;
                actCount += 1;
                Debug.Log("Action successfully logged! ID: " + actionInt);
                break;
            case 3:
                //Log action
                action4 = actionInt;
                actCount += 1;
                Debug.Log("Action successfully logged! ID: " + actionInt);
                break;
            case 4:
                //Log action
                Debug.Log("Max actions have been performed");
                break;
        }

    }


    //For removing the last performed action
    public void actionRemove(){
        switch (actCount)
        {
            case 0:
                Debug.Log("No actions have been performed yet");
                break;
            case 1:
                //Log action
                action1 = 0;
                actCount -= 1;
                break;
            case 2:
                //Log action
                action2 = 0;
                actCount -= 1;
                break;
            case 3:
                //Log action
                action3 = 0;
                actCount -= 1;
                break;
            case 4:
                //Log action
                action4 = 0;
                actCount -= 1;
                break;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLog : MonoBehaviour {
    
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
            default:
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

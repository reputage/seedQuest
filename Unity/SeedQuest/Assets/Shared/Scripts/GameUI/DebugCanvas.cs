using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour {

	void Update () {

        switch(GameManager.State)
        {
            case GameState.GameStart:
                GetComponentInChildren<Text>().text = "Mode: GameStart";
                break;
            case GameState.Rehearsal:
                GetComponentInChildren<Text>().text = "Mode: Rehersal";
                break;
            case GameState.Recall:
                GetComponentInChildren<Text>().text = "Mode: Recall";
                break;
            case GameState.GameEnd:
                GetComponentInChildren<Text>().text = "Mode: GameEnd";
                break;
            default:
                break;
        }
    }
}

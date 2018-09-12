using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    static public bool isPaused
    {
        get {
            return GameManager.State == GameState.GameStart;
        }
        set {
            if (value)
                GameManager.State = GameState.Pause;
            else
                GameManager.State = GameManager.PrevState;
        } 
    }

    
}

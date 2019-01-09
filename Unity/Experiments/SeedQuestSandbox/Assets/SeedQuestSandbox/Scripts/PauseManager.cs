using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    static public bool isPaused
    {
        get {
            return GameManager.State == GameState.Pause || GameManager.State == GameState.Interact;
        }
        set {
            if (value)
                GameManager.State = GameState.Pause;
            else
                GameManager.State = GameManager.PrevState;
        } 
    }

    
}

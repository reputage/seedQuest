using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the Pause state of SeedQuest
/// </summary>
public class PauseManager : MonoBehaviour {

    /// <summary>
    /// Returns current pause state of game i.e. SeedQuest is paused when menus are open.
    /// </summary>
    static public bool isPaused {
        get {  return GameManager.State == GameState.Pause || GameManager.State == GameState.Menu || GameManager.State == GameState.End; }
        set {
            if (value)
                GameManager.State = GameState.Pause;
            else
                GameManager.State = GameManager.PrevState;
        } 
    }

    /// <summary>
    /// Returns the current interaction state of the game i.e. whether a Player is interacting with Interactable
    /// </summary>
    static public bool isInteracting {
        get {  return GameManager.State == GameState.Interact;  }
        set
        {
            if (value)
                GameManager.State = GameState.Interact;
            else
                GameManager.State = GameManager.PrevState;
        }
    }
}

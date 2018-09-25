using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { GameStart, Rehearsal, Recall, Interact, Pause, GameEnd }

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;
    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public GameState state = GameState.GameStart;
    public GameState prevState = GameState.GameStart;
    public static GameState State { 
        get { return Instance.state; }
        set { if (value == Instance.state) return; Instance.prevState = Instance.state; Instance.state = value; UIManager.UpdateState(); }
    }
    public static GameState PrevState {
        get { return Instance.prevState; }
    }

    public GameSoundData gameSound = null;
    public static GameSoundData GameSound { get { return Instance.gameSound; } }

    public GameUIData gameUI = null;
    public static GameUIData GameUI { get { return Instance.gameUI; } }

    public void Update() { 
        if (Input.GetKeyDown("escape"))
        {
            if (GameManager.State == GameState.Pause || GameManager.State == GameState.Interact)
                GameManager.State = GameManager.PrevState;
            else
                GameManager.State = GameState.Pause;
        }
    }
}

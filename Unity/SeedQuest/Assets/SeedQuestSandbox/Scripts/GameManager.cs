using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { Sandbox, Rehearsal, Recall } 
public enum GameState { Play, Pause, Interact, Menu, End }

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;
    public static GameManager Instance {
        get  {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void OnApplicationQuit() {
        instance = null;
    }

    public GameMode mode = GameMode.Sandbox;
    public static GameMode Mode { 
        get { return Instance.mode; }
        set { Instance.mode = value; }
    }

    private static bool graduatedMode = false;
    public static bool GraduatedMode
    {
        get { return graduatedMode; }
        set { graduatedMode = value; }
    }

    public GameState state = GameState.Play;
    public GameState prevState = GameState.Play;
    public static GameState State {
        get { return Instance.state; }
        set { if (value == Instance.state) return; Instance.prevState = Instance.state; Instance.state = value; }
    }
    public static GameState PrevState {
        get { return Instance.prevState; }
    }

    public void Update() {
        ListenForKeyDown();
    }

    static public void ResetCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ListenForKeyDown() {
        if (Input.GetKeyDown("escape") &&  state != GameState.Menu) {
            //ESCMenuUI.ToggleOn();
            ScenePauseMenu.ToggleOn();
        }
    }
} 
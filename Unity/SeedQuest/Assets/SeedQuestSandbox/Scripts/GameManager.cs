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

    private static bool[] graduatedFlags = new bool[6];
    public static bool[] GraduatedFlags
    {
        get { return graduatedFlags; }
        set { graduatedFlags = value; }
    }

    public static void ResetGraduatedRehearsal()
    {
        GraduatedFlags = new bool[6];
    }

    private static bool reviewMode = false;
    public static bool ReviewMode
    {
        get { return reviewMode; }
        set { reviewMode = value; }
    }

    private static bool tutorialMode = false;
    public static bool TutorialMode
    {
        get { return tutorialMode; }
        set { tutorialMode = value; }
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
            //ScenePauseMenu.ToggleOn();
        }

        if (InputManager.GetKeyDown(KeyCode.F) && state != GameState.Menu)
        {
            if (Mode == GameMode.Rehearsal)
                return;
            FastRecoveryUI.ToggleActive();
        }
    }

    private static bool v2Menus = true;
    public static bool V2Menus
    {
        get { return v2Menus; }
        set { v2Menus = value; }
    }
} 
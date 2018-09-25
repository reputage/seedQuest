using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject gameUI = null;

    static private UIManager __instance = null;
    static public UIManager instance {
        get {
            if (__instance == null)
                __instance = GameObject.FindObjectOfType<UIManager>();
            return __instance;
        }
    }

    /// <summary> Updates state (active/inactive status) of UI canvas </summary>
    static public void UpdateState() {
        instance.SetGameUIState();
    }

    void Awake() { 
        InitGameUI();
        SetGameUIState();
    }

    void InitGameUI() {
        /*
        if(gameUI == null) { 
            Debug.Log("Warning: GameUI prefab missing.");
            return;
        }

        if (GameUI.instance == null)
            Instantiate(gameUI);
        */
    }

    void SetGameUIState() { 
        switch (GameManager.State) {
            case GameState.GameStart:
                GameUI.CursorUI = false;
                GameUI.StartMenuUI = true;
                GameUI.DebugCanvas = true;
                GameUI.ActionListCanvas = false;
                GameUI.InteractiableUI = false;
                GameUI.PauseMenuCanvas = false;
                GameUI.EndGameUI = false;
                GameUI.CompassUI = false;
                GameUI.MinimapUI = false;
                break;
            case GameState.Rehearsal:
                GameUI.CursorUI = true;
                GameUI.StartMenuUI = false;
                GameUI.DebugCanvas = true;
                GameUI.ActionListCanvas = true;
                GameUI.InteractiableUI = false;
                GameUI.PauseMenuCanvas = false;
                GameUI.CompassUI = true;
                GameUI.MinimapUI = true;
                break;
            case GameState.Recall:
                GameUI.CursorUI = true;
                GameUI.StartMenuUI = false;
                GameUI.DebugCanvas = true;
                GameUI.ActionListCanvas = true;
                GameUI.InteractiableUI = false;
                GameUI.PauseMenuCanvas = false;
                GameUI.CompassUI = false;
                GameUI.MinimapUI = false;
                break;
            case GameState.Interact:
                GameUI.ActionListCanvas = false;
                GameUI.InteractiableUI = true;
                break;
            case GameState.Pause:
                GameUI.PauseMenuCanvas = true;
                break;
            case GameState.GameEnd:
                GameUI.ActionListCanvas = false;
                GameUI.PauseMenuCanvas = false;
                GameUI.EndGameUI = true;
                break;
            default:
                break;
        } 

    }

}

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

    void Start() { 
        //InitGameUI();
        //SetGameUIState();
    }

    private void Update() {
        if (GameManager.State == GameState.LoadingRehersal)
            if (GameUI.instance != null)
                GameManager.State = GameState.Rehearsal;
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

    static public bool CheckUILoaded() {
        if (instance.GetComponentInChildren<CursorUI>(true) == null)
            return false;
        return true;
    }

    void SetGameUIState() {
        if (GameUI.instance == null) {
            return;
        }

        switch (GameManager.State) {
             /*
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
             */

            case GameState.LoadingRecall:
                break;
            case GameState.LoadingRehersal:
                break;
 
            case GameState.Rehearsal:
                GameUI.EndGameUI = false;
                GameUI.CursorUI = false;
                GameUI.StartMenuUI = false;
                GameUI.DebugCanvas = true;
                GameUI.ActionListCanvas = true;
                GameUI.InteractiableUI = false;
                GameUI.PauseMenuCanvas = false;
                GameUI.CompassUI = true;
                GameUI.LoadingScreen = false;
                if (Application.platform == RuntimePlatform.OSXPlayer)
                {
                    GameUI.MinimapUI = true;
                }
                else if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    GameUI.MinimapUI = true;
                }
                break;
            case GameState.Recall:
                GameUI.EndGameUI = false;
                GameUI.CursorUI = false;
                GameUI.StartMenuUI = false;
                GameUI.DebugCanvas = true;
                GameUI.ActionListCanvas = true;
                GameUI.InteractiableUI = false;
                GameUI.PauseMenuCanvas = false;
                GameUI.CompassUI = false;
                GameUI.MinimapUI = false;
                GameUI.LoadingScreen = false;
                break;
            case GameState.Interact:
                GameUI.ActionListCanvas = false;
                GameUI.InteractiableUI = true;
                break;
            case GameState.Pause:
                GameUI.PauseMenuCanvas = true;
                break;
            case GameState.GameEnd:
                GameUI.CompassUI = false;
                GameUI.ActionListCanvas = false;
                GameUI.PauseMenuCanvas = false;
                GameUI.EndGameUI = true;
                break;
            default:
                break;
        } 

    }

}

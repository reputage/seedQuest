using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;
using SeedQuest.Level;

public class LevelClearUI : MonoBehaviour {
    
    static private LevelClearUI instance = null;
    static private LevelClearUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<LevelClearUI>(true); return instance; }
    static public LevelClearUI Instance { get { return instance == null ? setInstance() : instance; } }

    static public void ToggleOn() {
        if (Instance.gameObject.activeSelf)
            return;
        GameManager.GraduatedFlags[InteractableLog.CurrentLevelIndex - 1] = true;
        Instance.gameObject.SetActive(true);
        GameManager.State = GameState.Menu;
    }

    static public void ToggleOff()
    {
        GameManager.State = GameState.Play;
        Instance.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Instance.gameObject.activeSelf && GameManager.State != GameState.Menu)
            GameManager.State = GameState.Menu;
    }

    public void GoToSceneSelect() {
        //LevelManager.GoToSceneSelect();
        if (MenuScreenV2.Instance != null)
            MenuScreenV2.Instance.ReturnToSceneLineUp();

        gameObject.SetActive(false);
    }

    public void ResetScene() {
        for (int i = 0; i < InteractableConfig.ActionsPerSite; i++) {
            InteractablePathManager.UndoLastAction();
        }

        InteractablePathManager.ShowLevelComplete = false;
        GameManager.State = GameManager.PrevState;
        if (GameManager.Mode == GameMode.Rehearsal)
            InteractablePreviewUI.ToggleShow();
        ToggleOff();
    }
}
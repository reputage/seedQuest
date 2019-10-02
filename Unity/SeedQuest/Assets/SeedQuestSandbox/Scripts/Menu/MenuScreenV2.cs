using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices;

using SeedQuest.Interactables;

public class MenuScreenV2 : MonoBehaviour
{
    static private MenuScreenV2 instance = null;
    static private MenuScreenV2 setInstance() { instance = GameObject.FindObjectOfType<MenuScreenV2>(); return instance; }
    static public MenuScreenV2 Instance { get { return instance == null ? setInstance() : instance; } }

    public MenuScreenStates state = MenuScreenStates.Start;

    private Canvas[] canvas;
    private Canvas startCanvas;
    private Canvas seedSetupCanvas;
    private Canvas encodeSeedCanvas;
    private Canvas sceneLineUpCanvas;
    private Canvas actionLineUpCanvas;
    private Canvas debugCanvas;

    public GameObject topMenu;

    void Awake() {
        SetComponentReferences();
    }

    private void Start() {
        if (MenuScreenV2.Instance.state == MenuScreenStates.Debug)
            return;
        else
            GameManager.State = GameState.Menu;

       GoToStart();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.BackQuote) && (state == MenuScreenStates.Start || state == MenuScreenStates.ModeSelect) ) {
            GoToDebugCanvas();
        }
    }

    private void SetComponentReferences() {
        canvas = GetComponentsInChildren<Canvas>(true);
        startCanvas = canvas[1];
        seedSetupCanvas = canvas[2];
        encodeSeedCanvas = canvas[3];
        sceneLineUpCanvas = canvas[4];
        actionLineUpCanvas = canvas[5];
        debugCanvas = canvas[6];
    }

    public void ResetCanvas() {
        foreach(Canvas _canvas in canvas) {
            if(_canvas != canvas[0])
                _canvas.gameObject.SetActive(false);
        }
    }

    public void GoToStart() {
        InteractablePathManager.Reset();

        state = MenuScreenStates.Start;
        ResetCanvas();
        startCanvas.gameObject.SetActive(true);
        topMenu.SetActive(true);
    }

    public void SetModeLearnSeed() {
        GameManager.Mode = GameMode.Rehearsal;
        GoToSeedSetup();
    }

    public void SetModeRecoverSeed() {
        GameManager.Mode = GameMode.Recall;
        GoToEncodeSeed();
    }

    public void GoToSeedSetup() {
        state = MenuScreenStates.SeedSetup;
        ResetCanvas();
        seedSetupCanvas.gameObject.SetActive(true);
    }

    public void GoToEncodeSeed() {
        state = MenuScreenStates.EncodeSeed;
        ResetCanvas();
        encodeSeedCanvas.gameObject.SetActive(true);
        encodeSeedCanvas.gameObject.GetComponent<EncodeSeedCanvas>().resetCanvas();
        encodeSeedCanvas.gameObject.GetComponent<EncodeSeedCanvas>().resetSeedStr();
        LevelSetManager.ResetCurrentLevels();
        WorldManager.Reset();
    }

    public void GoToSceneLineUp() {
        GameManager.State = GameState.Menu;
        state = MenuScreenStates.SceneLineUp;
        ResetCanvas();
        topMenu.SetActive(false);
        sceneLineUpCanvas.GetComponent<SceneLineUpCanvas>().ToggleOn();
        sceneLineUpCanvas.GetComponent<SceneLineUpCanvas>().StartScene();
    }

    public void ReturnToSceneLineUp() {
        GoToSceneLineUp();
        sceneLineUpCanvas.GetComponent<SceneLineUpCanvas>().Start();
    }

    public void GoToActionLineUp() {
        GameManager.State = GameState.Menu;
        if (GameManager.Mode == GameMode.Rehearsal){
            state = MenuScreenStates.ActionLineUp;
            ResetCanvas();
            actionLineUpCanvas.gameObject.SetActive(true);
            actionLineUpCanvas.GetComponent<ActionLineUpCanvas>().InitializeActionLineUp();
        }
        else {
            CloseSceneLineUp();
        }
    }

    public void CloseSceneLineUp() {
        CameraZoom.StartZoomIn();
        ResetCanvas();
        GameManager.State = GameState.Play;
    }

    public void GoToDebugCanvas() {
        ResetCanvas();
        debugCanvas.gameObject.SetActive(true);
    }

    public void DeactivateTopMenu()
    {
        topMenu.SetActive(false);
    }
}
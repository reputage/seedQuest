using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SeedQuest.Interactables;
using SeedQuest.HUD;

[System.Serializable]
public class HUDItemProps {
    public bool use = true;
    public GameObject prefab;
}

[ExecuteInEditMode]
public class HUDManager : MonoBehaviour {
    public HUDItemProps useCursor;
    public HUDItemProps useInteractablePreview;
    public HUDItemProps useHomeSelect;
    public HUDItemProps useEndGame;
    public HUDItemProps useESCMenu;
    public HUDItemProps useLevelClear;
    public HUDItemProps useLevelName;
    public HUDItemProps useLoadingScreen;
    public HUDItemProps usePauseMenu;
    public HUDItemProps useProgressTracker;
    public HUDItemProps useCLI;
    public HUDItemProps useTutorial;
    public HUDItemProps useInteractableTracker;
    public HUDItemProps useUndo;
    public HUDItemProps useZoomSlider;
    public HUDItemProps useHint;


    static private HUDManager instance = null;
    static private HUDManager setInstance() { instance = GameObject.FindObjectOfType<HUDManager>(); return instance; }
    static public HUDManager Instance { get => instance == null ? setInstance() : instance; }

    public void Awake() {
        GenerateHUD();
        DestroyImmediateHUD();
    }

    public void InstantiateHUDElement<T>(HUDItemProps props) {
        if(props.use && props.prefab != null && GetComponentInChildren<T>(true) == null) {

            //PrefabUtility.InstantiatePrefab(props.prefab, transform);
            Instantiate(props.prefab, transform);
        }
    }

    static public void InstantiateHUDElement<T>() {
        if (Instance == null)
            return;

        HUDItemProps props = Instance.GetProps<T>();
        if (props.prefab != null && Instance.GetComponentInChildren<T>(true) == null) {

           //PrefabUtility.InstantiatePrefab(props.prefab, Instance.transform);
           Instantiate(props.prefab, Instance.transform);
        }
    }

    public void DestroyHUDElement<T>(HUDItemProps props) {
        if (!props.use && GetComponentInChildren<T>(true) != null)
            DestroyImmediate((GetComponentInChildren<T>(true) as Component).gameObject);
    }

    public void GenerateHUD() {
        InstantiateHUDElement<CommandLineInputUI>(useCLI);
        InstantiateHUDElement<CursorUI>(useCursor);
        InstantiateHUDElement<EndGameUI>(useEndGame);
        InstantiateHUDElement<ESCMenuUI>(useESCMenu);
        InstantiateHUDElement<HomeSelectUI>(useHomeSelect);
        InstantiateHUDElement<InteractablePreviewUI>(useInteractablePreview);
        InstantiateHUDElement<InteractableTrackerUI>(useInteractableTracker);
        InstantiateHUDElement<LevelClearUI>(useLevelClear);
        InstantiateHUDElement<LevelNameUI>(useLevelName);
        InstantiateHUDElement<LoadingScreenUI>(useLoadingScreen);
        InstantiateHUDElement<ScenePauseMenu>(usePauseMenu);
        InstantiateHUDElement<ProgressTrackerUI>(useProgressTracker);
        InstantiateHUDElement<TutorialManager>(useTutorial);
        InstantiateHUDElement<UndoUI>(useUndo);
        InstantiateHUDElement<CameraSlider>(useZoomSlider);
        InstantiateHUDElement<GraduatedRehearsal>(useHint);
    }

    public void DestroyImmediateHUD() {
        DestroyHUDElement<CommandLineInputUI>(useCLI);
        DestroyHUDElement<CursorUI>(useCursor);
        DestroyHUDElement<EndGameUI>(useEndGame);
        DestroyHUDElement<ESCMenuUI>(useESCMenu);
        DestroyHUDElement<HomeSelectUI>(useHomeSelect);
        DestroyHUDElement<InteractablePreviewUI>(useInteractablePreview);
        DestroyHUDElement<InteractableTrackerUI>(useInteractableTracker);
        DestroyHUDElement<LevelClearUI>(useLevelClear);
        DestroyHUDElement<LevelNameUI>(useLevelName);
        DestroyHUDElement<LoadingScreenUI>(useLoadingScreen);
        DestroyHUDElement<ScenePauseMenu>(usePauseMenu);
        DestroyHUDElement<ProgressTrackerUI>(useProgressTracker);
        DestroyHUDElement<TutorialManager>(useTutorial);
        DestroyHUDElement<UndoUI>(useUndo);
        DestroyHUDElement<CameraSlider>(useZoomSlider);
        DestroyHUDElement<GraduatedRehearsal>(useHint);
    }

    public HUDItemProps GetProps<T>() {
        Type listType = typeof(T);
        if (listType == typeof(CommandLineInputUI)) { return useCLI; }
        else if (listType == typeof(CursorUI)) { return useCursor; }
        else if (listType == typeof(EndGameUI)) { return useEndGame; }
        else if (listType == typeof(ESCMenuUI)) { return useESCMenu; }
        else if (listType == typeof(HomeSelectUI)) { return useHomeSelect; }
        else if (listType == typeof(InteractablePreviewUI)) { return useInteractablePreview; }
        else if (listType == typeof(InteractableTrackerUI)) { return useInteractableTracker; }
        else if (listType == typeof(LevelClearUI)) { return useLevelClear; }
        else if (listType == typeof(LevelNameUI)) { return useLevelName; }
        else if (listType == typeof(LoadingScreenUI)) { return useLoadingScreen; }
        else if (listType == typeof(ScenePauseMenu)) { return usePauseMenu; }
        else if (listType == typeof(ProgressTrackerUI)) { return useProgressTracker; }
        else if (listType == typeof(TutorialManager)) { return useTutorial; }
        else if (listType == typeof(UndoUI)) { return useUndo; }
        else if (listType == typeof(CameraSlider)) { return useZoomSlider; }
        else if (listType == typeof(GraduatedRehearsal)) { return useHint; }
        return null;
    }
}
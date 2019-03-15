using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public HUDItemProps useProgressTracker;
    public HUDItemProps useCLI;
    public HUDItemProps useTutorial;
    public HUDItemProps useInteractableTracker;

    static private HUDManager instance = null;
    static private HUDManager setInstance() { instance = GameObject.FindObjectOfType<HUDManager>(); return instance; }
    static public HUDManager Instance { get => instance == null ? setInstance() : instance; }

    public void Awake() {
        GenerateHUD();
        DestroyImmediateHUD();
    }

    public void InstantiateHUDElement<T>(HUDItemProps props) {
        if(props.use && props.prefab != null && GetComponentInChildren<T>(true) == null) {
            var gameobj = Instantiate(props.prefab, transform);
            //gameobj.name.Replace("(Clone)", "");            
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
        InstantiateHUDElement<ProgressTrackerUI>(useProgressTracker);
        InstantiateHUDElement<TutorialManager>(useTutorial);
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
        DestroyHUDElement<ProgressTrackerUI>(useProgressTracker);
        DestroyHUDElement<TutorialManager>(useTutorial);
    }
}
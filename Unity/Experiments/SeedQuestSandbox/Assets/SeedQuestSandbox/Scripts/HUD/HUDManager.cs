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
    public HUDItemProps useLevelClear;
    public HUDItemProps useLevelName;
    public HUDItemProps useProgressTracker;
    public HUDItemProps useCLI;
    public HUDItemProps useTutorial;
    public HUDItemProps useInteractableTracker;

    public void Awake()
    {
        GenerateHUD();
        DestroyImmediateHUD();
    }

    public void GenerateHUD() {
        var test2 = GetComponentInChildren<CursorUI>();
        var test4 = GetComponentInChildren<CursorUI>() == null;
        var test3 = useCursor.prefab != null;
        var test = useCursor.use && useCursor.prefab != null && GetComponentInChildren<CursorUI>(true) == null;

        if(useCursor.use && useCursor.prefab != null && GetComponentInChildren<CursorUI>(true) == null)
            Instantiate(useCursor.prefab, transform);
        if (useInteractablePreview.use && useInteractablePreview.prefab != null && GetComponentInChildren<InteractablePreviewUI>(true) == null)
            Instantiate(useInteractablePreview.prefab, transform);
        if (useHomeSelect.use && useHomeSelect.prefab != null && GetComponentInChildren<HomeSelectUI>(true) == null)
            Instantiate(useHomeSelect.prefab, transform);
        if (useEndGame.use && useEndGame.prefab != null && GetComponentInChildren<EndGameUI>(true) == null)
            Instantiate(useEndGame.prefab, transform);
        if (useLevelClear.use && useLevelClear.prefab != null && GetComponentInChildren<LevelClearUI>(true) == null)
            Instantiate(useLevelClear.prefab, transform);
        if (useLevelName.use && useLevelName.prefab != null && GetComponentInChildren<LevelNameUI>(true) == null)
            Instantiate(useLevelName.prefab, transform);
        if (useProgressTracker.use && useProgressTracker.prefab != null && GetComponentInChildren<ProgressTrackerUI>(true) == null)
            Instantiate(useProgressTracker.prefab, transform);
        if (useCLI.use && useCLI.prefab != null && GetComponentInChildren<CommandLineInputUI>(true) == null)
            Instantiate(useCLI.prefab, transform);
        if (useTutorial.use && useTutorial.prefab != null && GetComponentInChildren<TutorialManager>(true) == null)
            Instantiate(useTutorial.prefab, transform);
        if (useInteractableTracker.use && useInteractableTracker.prefab != null && GetComponentInChildren<InteractableTrackerUI>(true) == null)
            Instantiate(useInteractableTracker.prefab, transform);
    }

    public void DestroyImmediateHUD() {
        if (!useCursor.use && GetComponentInChildren<CursorUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<CursorUI>(true).gameObject);
        if (!useInteractablePreview.use && GetComponentInChildren<InteractablePreviewUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<InteractablePreviewUI>(true).gameObject);
        if (!useHomeSelect.use && GetComponentInChildren<HomeSelectUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<HomeSelectUI>(true).gameObject);
        if (!useEndGame.use && GetComponentInChildren<EndGameUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<EndGameUI>(true).gameObject);
        if (!useLevelClear.use && GetComponentInChildren<LevelClearUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<LevelClearUI>(true).gameObject);
        if (!useLevelName.use && GetComponentInChildren<LevelNameUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<LevelNameUI>(true).gameObject);
        if (!useProgressTracker.use && GetComponentInChildren<ProgressTrackerUI>() != null)
            DestroyImmediate(GetComponentInChildren<ProgressTrackerUI>(true).gameObject);
        if (!useCLI.use && GetComponentInChildren<CommandLineInputUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<CommandLineInputUI>(true).gameObject);
        if (!useTutorial.use && GetComponentInChildren<TutorialManager>(true) != null)
            DestroyImmediate(GetComponentInChildren<TutorialManager>(true).gameObject);
        if (!useInteractableTracker.use && GetComponentInChildren<InteractableTrackerUI>(true) != null)
            DestroyImmediate(GetComponentInChildren<InteractableTrackerUI>(true).gameObject);
    }
}
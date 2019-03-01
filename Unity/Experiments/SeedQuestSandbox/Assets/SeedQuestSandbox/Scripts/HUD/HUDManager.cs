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
    }

    public void GenerateHUD() {
        if(useCursor.use && useCursor.prefab != null && GetComponentInChildren<CursorUI>() != null)
            Instantiate(useCursor.prefab, transform);
        if (useInteractablePreview.use && useInteractablePreview.prefab != null && GetComponentInChildren<InteractablePreviewUI>() != null)
            Instantiate(useInteractablePreview.prefab, transform);
        if (useHomeSelect.use && useHomeSelect.prefab != null && GetComponentInChildren<HomeSelectUI>() != null)
            Instantiate(useHomeSelect.prefab, transform);
        if (useEndGame.use && useEndGame.prefab != null && GetComponentInChildren<EndGameUI>() != null)
            Instantiate(useEndGame.prefab, transform);
        if (useLevelClear.use && useLevelClear.prefab != null && GetComponentInChildren<LevelClearUI>() != null)
            Instantiate(useLevelClear.prefab, transform);
        if (useLevelName.use && useLevelName.prefab != null && GetComponentInChildren<LevelNameUI>() != null)
            Instantiate(useLevelName.prefab, transform);
        if (useProgressTracker.use && useProgressTracker.prefab != null && GetComponentInChildren<ProgressTrackerUI>() != null)
            Instantiate(useProgressTracker.prefab, transform);
        if (useCLI.use && useCLI.prefab != null && GetComponentInChildren<CommandLineInputUI>() != null)
            Instantiate(useCLI.prefab, transform);
        if (useTutorial.use && useTutorial.prefab != null && GetComponentInChildren<TutorialManager>() != null)
            Instantiate(useTutorial.prefab, transform);
        if (useInteractableTracker.use && useInteractableTracker.prefab != null && GetComponentInChildren<InteractableTrackerUI>() != null)
            Instantiate(useInteractableTracker.prefab, transform);
    }

}
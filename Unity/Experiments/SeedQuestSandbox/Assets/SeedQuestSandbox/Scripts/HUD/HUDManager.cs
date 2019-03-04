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
        DestroyHUD();
    }

    public void GenerateHUD() {
        if(useCursor.use && useCursor.prefab != null && Resources.FindObjectsOfTypeAll<CursorUI>()[0] == null)
            Instantiate(useCursor.prefab, transform);
        if (useInteractablePreview.use && useInteractablePreview.prefab != null && Resources.FindObjectsOfTypeAll<InteractablePreviewUI>()[0] == null)
            Instantiate(useInteractablePreview.prefab, transform);
        if (useHomeSelect.use && useHomeSelect.prefab != null && Resources.FindObjectsOfTypeAll<HomeSelectUI>()[0] == null)
            Instantiate(useHomeSelect.prefab, transform);
        if (useEndGame.use && useEndGame.prefab != null && Resources.FindObjectsOfTypeAll<EndGameUI>()[0] == null)
            Instantiate(useEndGame.prefab, transform);
        if (useLevelClear.use && useLevelClear.prefab != null && Resources.FindObjectsOfTypeAll<LevelClearUI>()[0] == null)
            Instantiate(useLevelClear.prefab, transform);
        if (useLevelName.use && useLevelName.prefab != null && Resources.FindObjectsOfTypeAll<LevelNameUI>()[0] == null)
            Instantiate(useLevelName.prefab, transform);
        if (useProgressTracker.use && useProgressTracker.prefab != null && Resources.FindObjectsOfTypeAll<ProgressTrackerUI>()[0] == null)
            Instantiate(useProgressTracker.prefab, transform);
        if (useCLI.use && useCLI.prefab != null && Resources.FindObjectsOfTypeAll<CommandLineInputUI>()[0] == null)
            Instantiate(useCLI.prefab, transform);
        if (useTutorial.use && useTutorial.prefab != null && Resources.FindObjectsOfTypeAll<TutorialManager>()[0] == null)
            Instantiate(useTutorial.prefab, transform);
        if (useInteractableTracker.use && useInteractableTracker.prefab != null && Resources.FindObjectsOfTypeAll<InteractableTrackerUI>()[0] == null)
            Instantiate(useInteractableTracker.prefab, transform);
    }

    public void DestroyHUD() {
        if (!useCursor.use && Resources.FindObjectsOfTypeAll<CursorUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<CursorUI>()[0].gameObject);
        if (!useInteractablePreview.use && Resources.FindObjectsOfTypeAll<InteractablePreviewUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<InteractablePreviewUI>()[0].gameObject);
        if (!useHomeSelect.use && Resources.FindObjectsOfTypeAll<HomeSelectUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<HomeSelectUI>()[0].gameObject);
        if (!useEndGame.use && Resources.FindObjectsOfTypeAll<EndGameUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<EndGameUI>()[0].gameObject);
        if (!useLevelClear.use && Resources.FindObjectsOfTypeAll<LevelClearUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<LevelClearUI>()[0].gameObject);
        if (!useLevelName.use && Resources.FindObjectsOfTypeAll<LevelNameUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<LevelNameUI>()[0].gameObject);
        if (!useProgressTracker.use && Resources.FindObjectsOfTypeAll<ProgressTrackerUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<ProgressTrackerUI>()[0].gameObject);
        if (!useCLI.use && Resources.FindObjectsOfTypeAll<CommandLineInputUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<CommandLineInputUI>()[0].gameObject);
        if (!useTutorial.use && Resources.FindObjectsOfTypeAll<TutorialManager>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<TutorialManager>()[0].gameObject);
        if (!useInteractableTracker.use && Resources.FindObjectsOfTypeAll<InteractableTrackerUI>()[0] != null)
            Destroy(Resources.FindObjectsOfTypeAll<InteractableTrackerUI>()[0].gameObject);
    }
}
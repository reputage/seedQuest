using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

public class ActionLineCameraRig : MonoBehaviour
{
    InteractableCameraRig[] rigs;

    private void Awake()
    {
        rigs = GetComponentsInChildren<InteractableCameraRig>();
    }

    public void Initialize() {
        Interactable[] interactables = InteractablePath.Path.ToArray();
        int[] actionIds = InteractablePath.ActionIds.ToArray();

        int sceneIndex = InteractableLog.CurrentLevelIndex;
        int baseIndex = sceneIndex * InteractableConfig.ActionsPerSite;

        for (int i = 0; i < InteractableConfig.ActionsPerSite; i++) {
            Interactable interactable = interactables[baseIndex + i];
            rigs[i].SetPreviewObject(interactable);
            rigs[i].SetPreviewAction(actionIds[baseIndex + i]);
        }
    }

    public void Continue() {
        MenuScreenV2.Instance.CloseSceneLineUp();
    }
}

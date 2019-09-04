using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SeedQuest.Interactables;

public class ActionLineUpCanvas : MonoBehaviour
{
    public void InitializeActionLineUp() {
        
        TextMeshProUGUI text = GetComponentsInChildren<TextMeshProUGUI>()[1];
        text.text = WorldManager.CurrentWorldScene.name;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        Interactable[] interactables = InteractablePath.Path.ToArray();
        int[] actionIds = InteractablePath.ActionIds.ToArray();
        int sceneIndex = InteractableLog.CurrentLevelIndex;
        int baseIndex = sceneIndex * InteractableConfig.ActionsPerSite;

        for (int i = 0; i < InteractableConfig.ActionsPerSite; i++) {
            Interactable interactable = interactables[baseIndex + i];
            interactable.ID.actionID = actionIds[baseIndex + i];

            texts[2 * i + 2].text = interactable.Name;
            texts[2 * i + 3].text = interactable.RehearsalActionName;
        }

        GameManager.Instance.GetComponentInChildren<ActionLineCameraRig>().Initialize();
    }

    public void Continue() {
        MenuScreenV2.Instance.CloseSceneLineUp();
    }
}
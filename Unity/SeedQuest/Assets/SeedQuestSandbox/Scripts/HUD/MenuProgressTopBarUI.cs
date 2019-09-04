using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SeedQuest.Level;
using SeedQuest.Interactables;

public class MenuProgressTopBarUI : MonoBehaviour
{
    private TextMeshProUGUI sceneText;
    private TextMeshProUGUI actionText;
    private Image progress;

    public void Awake() {
        sceneText = GetComponentsInChildren<TextMeshProUGUI>()[3];
        actionText = GetComponentsInChildren<TextMeshProUGUI>()[4];
        progress = GetComponentsInChildren<Image>()[6];
    }

    public void Update() {
        UpdateText();
        UpdateProgress();
    }

    public void UpdateText() {
        sceneText.text = (InteractableLog.CurrentLevelIndex + 1) + ". "  + LevelManager.LevelName;
        actionText.text = InteractableLog.Count + "/" + InteractableConfig.ActionsPerGame + " actions";
    }

    public void UpdateProgress() {
        float width = 600.0f;
        float value = width * (InteractableLog.PercentComplete / 100.0f - 1.0f);
        progress.GetComponent<RectTransform>().offsetMax = new Vector2(value, 0);
    }

    public void OnClickMenuButton() {
        PauseMenuUI.ToggleOn();
    }

    public void OnClickHelpButton() {
        HelpMenuUI.ToggleOn();
    }

    public void OnClickUndoButton()
    {
        int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
        if (actionsThisScene > 0) {
            InteractablePathManager.UndoLastAction();
        }
        else {
            Debug.Log("Unable to undo actions from a previous scene.");
        }
    }

    public void OnClickFastRecoveryButton()
    {
        FastRecoveryUI.ToggleActive();
    }
}

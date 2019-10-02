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
    private Button fastRecovery;

    public void Awake() {
        sceneText = GetComponentsInChildren<TextMeshProUGUI>()[3];
        actionText = GetComponentsInChildren<TextMeshProUGUI>()[4];
        progress = GetComponentsInChildren<Image>()[6];
        fastRecovery = GetComponentsInChildren<Button>()[3];
    }

    public void Update() {
        UpdateText();
        UpdateProgress();
        UpdateMode();
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

    public void UpdateMode()
    {
        if (GameManager.Mode == GameMode.Rehearsal)
        {
            fastRecovery.interactable = false;
            Color disabledColor = new Color(0.133f, 0.353f, 0.502f, 0.4f);
            fastRecovery.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = disabledColor;
            fastRecovery.gameObject.GetComponent<SimpleTextButton>().defaultColor = disabledColor;
            fastRecovery.gameObject.GetComponent<SimpleTextButton>().hoverColor = disabledColor;
        }

        else if (!fastRecovery.interactable)
        {
            fastRecovery.interactable = true;
            fastRecovery.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.133f, 0.353f, 0.502f, 1.0f);
            fastRecovery.gameObject.GetComponent<SimpleTextButton>().defaultColor = new Color(0.133f, 0.353f, 0.502f, 1.0f);
            fastRecovery.gameObject.GetComponent<SimpleTextButton>().hoverColor = new Color(0.227f, 0.592f, 0.839f, 1.0f);
        }
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

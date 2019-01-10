using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum InteractableUIMode { LeftRight, List };

[System.Serializable]
public class InteractableUI {

    public int fontSize = 36;
    public float scaleSize = 1;
    public InteractableUIMode mode;

    private GameObject actionUI = null;
    private Button[] actionButtons;

    public void Initialize(Interactable interactable) {
        Vector3 positionOffset = Vector3.zero;
        if (interactable.stateData != null)
            positionOffset = interactable.stateData.labelPosOffset;
        Vector3 position = interactable.transform.position + positionOffset;
        Quaternion rotate = Quaternion.identity;

        int modeIndex = mode == InteractableUIMode.LeftRight ? 0 : 1;
        actionUI = GameObject.Instantiate(InteractableManager.Instance.actionSpotIcons[modeIndex], position, rotate, InteractableManager.Instance.transform);

        setScale();

        var text = actionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.fontSize = fontSize; 

        if (interactable.stateData != null)
            text.text = interactable.stateData.interactableName;
        else
            text.text = "Error: Missing StateData";
        
        actionButtons = actionUI.GetComponentsInChildren<Button>();
        if (mode == InteractableUIMode.LeftRight) {
            actionButtons[0].onClick.AddListener(interactable.NextAction);
            actionButtons[1].onClick.AddListener(interactable.PrevAction);
        }
        else if (mode == InteractableUIMode.List) {
            for (int i = 0; i < 4; i++) {
                var actionText = actionButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                actionText.text = interactable.stateData.getStateName(i);
            }

            actionButtons[0].onClick.AddListener(delegate { interactable.DoAction(0); });
            actionButtons[1].onClick.AddListener(delegate { interactable.DoAction(1); });
            actionButtons[2].onClick.AddListener(delegate { interactable.DoAction(2); });
            actionButtons[3].onClick.AddListener(delegate { interactable.DoAction(3); });
        }

        hideActions();

        // Create Triggers for HoverEvents
        foreach (Button button in actionButtons) {
            setButtonHoverEvents(button);
        }
    }

    public void Update() {
        if(actionUI != null) {
            BillboardInteractable();
        }
    }

    public bool isReady() {
        return actionUI != null;
    }

    // Create TriggerEntry and add callback
    public void setButtonHoverEvents(Button button)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { GameManager.State = GameState.Interact; });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { GameManager.State = GameState.Sandbox; });
        trigger.triggers.Add(exit);
    }

    public void hideActions() {
        foreach (Button button in actionButtons) {
            button.transform.gameObject.SetActive(false);
        }
    }

    public void showActions() {
        foreach (Button button in actionButtons) {
            button.transform.gameObject.SetActive(true);
        }
    }

    public void BillboardInteractable()
    {
        Vector3 targetPosition = Camera.main.transform.position;
        Vector3 interactablePosition = actionUI.transform.position;
        Vector3 lookAtDir = targetPosition - interactablePosition;

        Quaternion rotate = Quaternion.LookRotation(lookAtDir);
        actionUI.transform.rotation = rotate;
    }

    public void setScale() {
        actionUI.GetComponent<RectTransform>().localScale = new Vector3(-0.01f * scaleSize, 0.01f * scaleSize, 0.01f * scaleSize);
    }

}

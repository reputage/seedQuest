using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using SeedQuest.Interactables;

public class InteractableLabelUI
{
    private GameObject labelObject;
    private Canvas labelCanvas;
    private Button labelButton;
    private TextMeshProUGUI labelText;
    private TextMeshProUGUI labelText2;
    private Image labelIcon;
    private Image trackerIcon;

    private Interactable interactable;
    private Vector3 labelPosition = new Vector3();
    private Animator animator;

    static private bool show;

    public void Initialize(Interactable parentInteractable) {
        show = true;

        InstantiateLabel(parentInteractable);
        SetComponentRef();
        SetLabelText();
        SetLabelText2();
        SetPosition();
        SetHoverEvents();
        ToggleTrackerIcon(false);
        ToggleText(false);
    }

    public void Update() {
        if (!isReady()) return;
        SetPosition();
        ListenForNear();
    }

    static public void ToggleAll(bool active) {
        show = active;
    }

    static public void ClearInteractableUI() {
        GameObject container = GameObject.Find("InteractableUIContainer");
        if (container != null) {
            foreach (Transform child in container.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    private void InstantiateLabel(Interactable parentInteractable) {
        if (isReady()) return;

        // Set interactable reference 
        interactable = parentInteractable;

        // Get InteractableUIContainer 
        Transform UIContainer;
        if (!GameObject.Find("InteractableUIContainer")) {
            UIContainer = new GameObject("InteractableUIContainer").transform;
            UIContainer.parent = InteractableManager.Instance.transform;
        }
        else {
            UIContainer = GameObject.Find("InteractableUIContainer").transform;
        } 

        // Create label object 
        labelObject = GameObject.Instantiate(InteractableManager.Instance.interactableLabelUI, UIContainer);
    }

    private bool isReady() {
        return labelObject != null;
    }

    private void SetComponentRef() {
        animator = labelObject.GetComponent<Animator>();
        labelCanvas = labelObject.GetComponentsInChildren<Canvas>(true)[1];
        labelButton = labelObject.GetComponentInChildren<Button>(true);
        labelText = labelObject.GetComponentsInChildren<TextMeshProUGUI>(true)[0];
        labelText2 = labelObject.GetComponentsInChildren<TextMeshProUGUI>(true)[1];
        labelIcon = labelObject.GetComponentsInChildren<Image>(true)[0];
        trackerIcon = labelObject.GetComponentsInChildren<Image>(true)[1];

        labelButton.onClick.AddListener(ActivateInteractable);
    }

    public void SetLabelText() {
        labelText.text = interactable.Name;
    }

    public void SetLabelText2()
    {
        labelText2.text = interactable.Name;
    }

    public Vector3 LabelPosition {
        get {
            Vector3 offset = interactable.stateData != null ? interactable.stateData.interactableUI.positionOffset : Vector3.zero;
            Vector3 position = interactable.transform.position + offset;
            return position;
        }
    }

    private void SetPosition() {
        labelPosition = IsometricCamera.Camera.WorldToScreenPoint(LabelPosition);
        labelCanvas.transform.position = labelPosition;
    }   

    public void ToggleTrackerIcon(bool active) {
        if (labelObject == null) return;

        trackerIcon.gameObject.SetActive(active);
        labelIcon.gameObject.SetActive(!active);
    }

    private void ToggleIcon(bool active) {
        labelIcon.gameObject.SetActive(active);
    }

    private void ToggleText(bool active) {
        labelText.gameObject.SetActive(active);
        labelText2 .gameObject.SetActive(active);
    }

    private void SetHoverEvents() {
        EventTrigger trigger = labelObject.GetComponent<EventTrigger>();
        if (trigger == null) {
            labelObject.gameObject.AddComponent<EventTrigger>();
            trigger = labelObject.GetComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnHoverEnter(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnHoverExit(); });
        trigger.triggers.Add(exit);
    }

    public void OnHoverEnter() {
        if(GameManager.Mode == GameMode.Sandbox)
            InteractableManager.SetActiveInteractable(interactable);
        
        ToggleText(true);
        animator.Play("HoverStartLabelAnimation");
        AudioManager.Play("UI_Hover");
    }

    public void OnHoverExit() {
        ToggleText(false);
        animator.Play("HoverStopLabelAnimation");
    }
    
    public void ActivateInteractable() {
        if (!labelObject.activeSelf) return;
        if (FastRecoveryUI.Instance != null) { 
            if (FastRecoveryUI.Instance.gameObject.activeSelf) return;
        }

        if (GameManager.State == GameState.Menu)
            return;

        AudioManager.Play("UI_Click");
        InteractableManager.SetActiveInteractable(interactable);
        InteractableActionsUI.Toggle(true);
    }

    private void ListenForNear() {
        if(interactable.PlayerIsNear() && show) {
            labelObject.SetActive(true);
        }
        else {
            labelObject.SetActive(false);
        }

        if (InteractablePath.isNextInteractable(interactable) && show)
            labelObject.SetActive(true);
    }
}
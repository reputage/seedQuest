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
    private Image labelIcon;

    private Interactable interactable;
    private Vector3 labelPosition = new Vector3();

    public void Initialize(Interactable parentInteractable) {
        InstantiateLabel(parentInteractable);
        SetComponentRef();
        SetLabelText();
        SetPosition();
        ToggleIcon(false);
        SetHoverEvents();
    }

    public void Update() {
        if (!isReady()) return;
        SetPosition();
        //SetIcon();
        //ListenForNear();
    }

    public void DeleteUI() {
        GameObject.Destroy(labelObject);
    }

    static public void ToggleAll(bool active) {
        GameObject container = GameObject.Find("InteractableUIContainer");
        if (container != null) {
            foreach (Transform child in container.transform){
                child.gameObject.SetActive(active);
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

    static public void ClearInteractableUI() {
        GameObject container = GameObject.Find("InteractableUIContainer");
        if (container != null) {
            foreach (Transform child in container.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    private bool isReady() {
        return labelObject != null;
    }

    private void SetComponentRef() {
        labelCanvas = labelObject.GetComponentsInChildren<Canvas>()[1];
        labelButton = labelObject.GetComponentInChildren<Button>();
        labelText = labelObject.GetComponentInChildren<TextMeshProUGUI>();
        labelIcon = labelObject.GetComponentsInChildren<Image>(true)[0];

        labelButton.onClick.AddListener(ActivateInteractable);
    }

    private void SetLabelText() {
        labelText.text = interactable.Name;
    }

    private void SetPosition() {
        Vector3 position = interactable.transform.position + interactable.interactableUI.positionOffset + interactable.stateData.labelPosOffset;
        labelPosition = IsometricCamera.Camera.WorldToScreenPoint(position);
        labelCanvas.transform.position = labelPosition;
    }

    private void SetIcon() {
        bool active = interactable == InteractableManager.ActiveInteractable;
        labelIcon.gameObject.SetActive(active);
    }

    private void ToggleIcon(bool active) {
        labelIcon.gameObject.SetActive(active);
    }

    private void SetHoverEvents() {
        EventTrigger trigger = labelObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
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

    private void OnHoverEnter() {
        ToggleIcon(true);
        AudioManager.Play("UI_Hover");
    }

    private void OnHoverExit() {
        ToggleIcon(false);
    }

    public void ActivateInteractable() {
        InteractableManager.SetActiveInteractable(interactable, interactable.ActionIndex);
        InteractableActionsUI.Toggle(true);
        ToggleIcon(false);
    }

    private void ListenForNear() {
        Vector3 playerPosition = IsometricCamera.instance.playerTransform.position;
        Vector3 interactablePosition = interactable.GetComponent<BoxCollider>().center + interactable.interactableCamera.lookAtOffset;
        //interactablePosition = interactable.transform.InverseTransformPoint(interactablePosition);

        float dist = (interactablePosition - playerPosition).magnitude;
        if (dist < InteractableManager.Instance.nearDistance)
            labelObject.SetActive(true);
        else
            labelObject.SetActive(false);
    }
}
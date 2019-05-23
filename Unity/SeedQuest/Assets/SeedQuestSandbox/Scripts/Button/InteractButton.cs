using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Button))]
public class InteractButton : MonoBehaviour
{
    private bool isOnHover = false;
    private bool isOnHoverFlag = false;
    public bool IsOnHover { get => isOnHoverFlag; }

    void Awake() {
        SetButtonClickEvents();
        SetButtonHoverEvents();
    }

    void Update() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            OnClickForLockedCursor();
            OnHoverForLockedCursor();
        }
    }
    private void OnHoverEnter() {
        if (PauseManager.isPaused == true) return;
        GameManager.State = GameState.Interact;
        AudioManager.Play("UI_Hover");
        isOnHoverFlag = true;

        if (Cursor.lockState == CursorLockMode.Locked)
            GetComponent<Button>().targetGraphic.color = GetComponent<Button>().colors.highlightedColor;
    }

    private void OnHoverExit()  {
        if (PauseManager.isPaused == true) return;
        GameManager.State = GameState.Play;
        isOnHoverFlag = false;

        if (Cursor.lockState == CursorLockMode.Locked)
            GetComponent<Button>().targetGraphic.color = GetComponent<Button>().colors.normalColor;
    }

    public void SetButtonClickEvents() {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(delegate () { AudioManager.Play("UI_Click"); });
    }

    public void SetButtonHoverEvents() {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnHoverEnter(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnHoverExit(); });
        trigger.triggers.Add(exit);
    }

    public void OnClickForLockedCursor() {
        if (PauseManager.isPaused == true)
            return;

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))  {
                bool hitThis = hit.transform.GetInstanceID() == transform.GetInstanceID();
                if (!hitThis)
                    return;

                IPointerClickHandler clickHandler = hit.transform.gameObject.GetComponent<IPointerClickHandler>();
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                if (clickHandler != null)
                {
                    clickHandler.OnPointerClick(pointerEventData);
                }
            }
        }
    }

    public void OnHoverForLockedCursor() {
        if (PauseManager.isPaused == true)
            return;

        Camera c = Camera.main;
        RaycastHit hit;
        Ray ray = c.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            bool hitThis = hit.transform.GetInstanceID() == transform.GetInstanceID();
            if (hitThis)  {
                if (!isOnHover) {
                    OnHoverEnter();
                }
                isOnHover = true;
            }
            else {
                if (isOnHover) {
                    OnHoverExit();
                }
                isOnHover = false;
            }
        }
    }
}
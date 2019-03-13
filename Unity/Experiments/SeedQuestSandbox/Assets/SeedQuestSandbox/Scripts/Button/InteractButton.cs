using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Button))]
public class InteractButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() {
        setButtonClickEvents();
        setButtonHoverEvents();
    }

    public void setButtonClickEvents()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(delegate () { AudioManager.Play("UI_Click"); });
    }

    // Set hover and click sounds
    public void setButtonHoverEvents()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => {
            if (PauseManager.isPaused == true) return;
            GameManager.State = GameState.Interact;
            AudioManager.Play("UI_Hover");

        });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => {
            if (PauseManager.isPaused == true) return;
            GameManager.State = GameState.Play;
        });
        trigger.triggers.Add(exit);
    }
}
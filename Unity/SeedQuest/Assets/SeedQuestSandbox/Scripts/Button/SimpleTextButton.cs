using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleTextButton : MonoBehaviour
{
    public Color32 defaultColor = Color.black;
    public Color32 hoverColor = Color.black;

    public void Awake() {
        SetButtonHoverEvents();
    }

    public void Start() {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.color = defaultColor; 
    }

    public void SetButtonHoverEvents() {
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null) {
            gameObject.AddComponent<EventTrigger>();
            trigger = GetComponent<EventTrigger>();
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
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.color = hoverColor;
        else
            Debug.LogError("Error: Button doesn't TextMeshProUGUI.");
                
        AudioManager.Play("UI_Hover");
    }

    private void OnHoverExit() {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.color = defaultColor;
        else
            Debug.LogError("Error: Button doesn't TextMeshProUGUI.");
    }
}

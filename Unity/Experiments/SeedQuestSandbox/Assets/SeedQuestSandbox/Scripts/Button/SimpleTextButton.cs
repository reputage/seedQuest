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

    public void Awake()
    {
        SetButtonHoverEvents();
    }

    public void SetButtonHoverEvents()
    {
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

    private void OnHoverEnter()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.color = hoverColor;
        AudioManager.Play("UI_Hover");
    }

    private void OnHoverExit()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.color = defaultColor;
    }
}

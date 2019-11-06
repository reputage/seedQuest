using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Button))]
public class SimpleHoverButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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

    }

    public void OnHoverEnter()
    {
        AudioManager.Play("UI_Hover");
    }



}

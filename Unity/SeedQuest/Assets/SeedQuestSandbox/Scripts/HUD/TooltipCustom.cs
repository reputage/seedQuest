using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Button))]
public class TooltipCustom : MonoBehaviour
{

    public Vector3 offset;
    public string tooltipText;
    public GameObject tooltipPrefab;
    private GameObject tooltipObj;

    void Awake()
    {
        SetButtonHoverEvents();
        Vector3 empty = new Vector3(0, 0, 0);

        // if no input offseet has been given, default to a premade offset value
        if (offset == empty)
            offset = new Vector3(90, -40, 0);
        
        tooltipObj = Instantiate(tooltipPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        tooltipObj.transform.parent = transform;
        tooltipObj.GetComponentInChildren<TextMeshProUGUI>().text = tooltipText;
        tooltipObj.SetActive(false);

    }

    void Update()
    {
        //tooltipObj.transform.position = Input.mousePosition + offset;
    }

    private void OnHoverEnter()
    {
        tooltipObj.SetActive(true);
        tooltipObj.transform.position = Input.mousePosition + offset;
    }

    private void OnHoverExit()
    {
        tooltipObj.SetActive(false);
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

}

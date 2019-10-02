using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Wobble : MonoBehaviour {

    public float wobbleStrength = 10;
    public float wobbleSpeed = 5;

    /*private bool play = false;
    private float x;
    private float y;
    private float z;

    private void Awake()
    {
        SetHoverEvents();
        SetRefs();
    }*/

    void Update () {
        //if (play)
            transform.Translate(Vector3.up * Mathf.Sin(wobbleSpeed * Time.time));
        /*else
            transform.localPosition = new Vector3(x, y, z);*/
	}

    /*private void OnHoverEnter()
    {
        play = true;
    }


    private void OnHoverExit()
    {
        play = false;
    }

    private void SetHoverEvents()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            gameObject.gameObject.AddComponent<EventTrigger>();
            trigger = gameObject.GetComponent<EventTrigger>();
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

    private void SetRefs()
    {
        x = gameObject.transform.localPosition.x;
        y = gameObject.transform.localPosition.y;
        z = gameObject.transform.localPosition.z;
    }*/
}
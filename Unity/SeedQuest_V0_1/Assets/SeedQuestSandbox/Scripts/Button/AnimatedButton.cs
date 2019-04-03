using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
[ExecuteInEditMode()]
public class AnimatedButton : MonoBehaviour {
    
    public float width = 256.0f;
    public float height = 80.0f;

    /*
    private void Awake()
    {
        // Set width and height
        RectTransform rTransform = GetComponent<RectTransform>();
        rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        // Set Button properties
        Button button = GetComponent<Button>();
        Image image = GetComponent<Image>();
        image.sprite = GameManager.GameUI.buttonSprites[0];
        button.targetGraphic = image;
        button.transition = Selectable.Transition.Animation;

        // Set animator controller
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = GameManager.GameUI.buttonAnimationCtrl;

        // Set hover and click sounds
        button.onClick.AddListener(delegate () { AudioManager.Play("ButtonClick"); });

        EventTrigger trigger = GetComponent<EventTrigger>(); 
        if(trigger.triggers.Count == 0) { 
            // Create TriggerEntry and add callback
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { AudioManager.Play("ButtonHover"); });
            trigger.triggers.Add(entry);
        }
        else {
            // Only add callback if TriggerEntry already exists
            trigger.triggers[0].callback.AddListener((data) => { AudioManager.Play("ButtonHover"); });
        }

        // Add a child object for text if needed
        GameObject text;
        if (GetComponentInChildren<Text>(true) == null) {
            text = Instantiate(new GameObject("Text"), transform);
            text.AddComponent<Text>();
        } 
        else
            text = GetComponentInChildren<Text>().gameObject;

        // Set text properties
        text.GetComponent<Text>().text = name;
        text.GetComponent<Text>().font = GameManager.GameUI.textFont;
        text.GetComponent<Text>().fontSize = 32;
        text.GetComponent<Text>().color = new Color(50/255, 50/255, 50/255);
        text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
    */
}

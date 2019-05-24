using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum UIButtonHoverStyle { SmallEffect, MediumEffect }

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Button))]
public class UIButtonHover : MonoBehaviour
{
    public bool useClickSound = true;
    public bool useHoverSound = true;
    public bool useHoverAnimation = true;
    public UIButtonHoverStyle animationStyle = UIButtonHoverStyle.SmallEffect;

    private Animator animator = null;

    void Awake() {
        SetButtonClickEvents();
        SetButtonHoverEvents();
        SetupAnimation();
    }

    private void OnHoverEnter() {
        if (animator != null && useHoverAnimation) {
            if(animationStyle == UIButtonHoverStyle.SmallEffect)
                animator.Play("UIButtonHover");
            else if (animationStyle == UIButtonHoverStyle.MediumEffect)
                animator.Play("UIButtonHighlight");
        }

        if(useHoverSound)
            AudioManager.Play("UI_Hover");
    }

    private void OnHoverExit() {
        if (animator != null && useHoverAnimation)
            animator.Play("UIButtonIdle");
    }

    public void SetupAnimation() {
        animator = GetComponent<Animator>();
    }

    public void SetButtonClickEvents() {
        if(useClickSound) {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(delegate () { AudioManager.Play("UI_Click"); });            
        }
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
}

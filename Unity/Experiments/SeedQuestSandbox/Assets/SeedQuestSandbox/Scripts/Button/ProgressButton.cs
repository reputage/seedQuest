using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProgressButton : MonoBehaviour
{
    private Canvas canvas;
    private Image progress;
    private Image checkmark;
    private Animator[] animators;

    [SerializeField]
    private bool progressComplete;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private float progressTime = 0;
    private Action progressCompleteAction;
    public float maxTime = 2.0f;

    public bool ProgressComplete { get => progressComplete; }
    public Action ProgressCompleteAction { set => progressCompleteAction = value; }

    void Start() {
        progressComplete = false;
        isActive = false;
        progressCompleteAction = null;

        canvas = GetComponentInChildren<Canvas>();
        progress = canvas.GetComponentsInChildren<Image>()[1];
        checkmark = canvas.GetComponentsInChildren<Image>()[2];
        animators = GetComponentsInChildren<Animator>();

        canvas.gameObject.SetActive(false);
        checkmark.gameObject.SetActive(false);

        SetupLabelClickEvents();
    }

    void Update() {
        if (isActive)
            updateProgress();
    }

    private void ResetProgress() {
        progressComplete = false;
        progressTime = 0;
        progress.GetComponent<RectTransform>().offsetMax = new Vector2(-200, 0);
    }

    public void SetActive(bool value) {
        canvas.gameObject.SetActive(value);
        if (!value)
            ResetProgress();
    }

    public void SetShow(bool value, float delay) {
        isActive = value;

        if(value)  {
            if(!canvas.gameObject.activeSelf)
                canvas.gameObject.SetActive(true);

            animators[0].Play("ProgressShowAnimation");
            ResetProgress();
        }
        else {
            animators[0].Play("ProgressHideAnimation");
        }
    }

    private void SetupLabelClickEvents() {
        EventTrigger trigger = GetComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => startProgress());
        trigger.triggers.Add(pointerDown);

        trigger = GetComponent<EventTrigger>();
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => checkProgress());
        trigger.triggers.Add(pointerUp);

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnHoverEnter(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { checkProgress(); OnHoverExit(); });
        trigger.triggers.Add(exit);
    }

    private void OnHoverEnter() {
        animators[0].Play("ProgressHoverAnimation");
    }

    private void OnHoverExit() {
        if (progressComplete) {
            isActive = true;
            ResetProgress();
        }

        animators[0].Play("ProgressIdleAnimation");
    }

    public void startProgress() {
        if (!isActive)
            return;
        
        canvas.gameObject.SetActive(true);
        GameManager.State = GameState.Interact;

        progressTime += Time.deltaTime;
    }

    private void updateProgress() {
        if (progressTime >= maxTime) {
            progressTime = maxTime;
            checkProgress();
            return;
        } 
        else if (progressTime > 0) {
            progressTime += Time.deltaTime;
            float progressPosition = 200 * (-1 + (progressTime / maxTime));
            progress.GetComponent<RectTransform>().offsetMax = new Vector2(progressPosition, 0);
        }
    }

    private void checkProgress() {
        if (!isActive)
            return;

        GameManager.State = GameState.Play;

        if (progressTime == maxTime ) {
            if(!progressComplete) {
                checkmarkAnimate();
                progressComplete = true;
                progressCompleteAction?.Invoke();
            }
        }
        else  {
            progressTime = 0;
            progress.GetComponent<RectTransform>().offsetMax = new Vector2(-200, 0);
        }
    }
    
    private void checkmarkAnimate() {
        checkmark.gameObject.SetActive(true);
        animators[0].Play("ProgressCompleteAnimation");
        animators[1].Play("CompleteCheckAnimation");
        AudioManager.Play("UI_CheckmarkComplete");
    }
}

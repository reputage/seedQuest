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
    private Animator animator;

    private bool progressComplete;
    private bool isActive;
    private Action progressCompleteAction;
    private float progressTime = 0;
    public float maxTime = 2.0f;

    public bool ProgressComplete { get => progressComplete; }
    public Action ProgressCompleteAction { set => progressCompleteAction = value; }

    void Start() {
        progressComplete = false;
        isActive = false;
        progressCompleteAction = null;

        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        progress = canvas.GetComponentsInChildren<Image>()[1];
        checkmark = canvas.GetComponentsInChildren<Image>()[2];
        checkmark.gameObject.SetActive(false);
        animator = checkmark.GetComponent<Animator>();

        SetupLabelClickEvents();
    }

    void Update() {
        if (isActive)
            updateProgress();
    }

    private void Reset() {
        progressComplete = true;
        isActive = false;
        progressTime = 0;
        canvas.gameObject.SetActive(false);
        checkmark.gameObject.SetActive(false);
    }

    public void SetActive(bool value, float delay) {
        isActive = value;

        if(value)  {
            canvas.gameObject.SetActive(value);
            progress.GetComponent<RectTransform>().offsetMax = new Vector2(-200, 0);
        }
        else {
            StartCoroutine(ExecuteAfterTime(delay));
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        canvas.gameObject.SetActive(false);
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

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { checkProgress(); });
        trigger.triggers.Add(exit);
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
        animator.Play("CompleteCheckAnimation");
        AudioManager.Play("UI_CheckmarkComplete");
    }
}

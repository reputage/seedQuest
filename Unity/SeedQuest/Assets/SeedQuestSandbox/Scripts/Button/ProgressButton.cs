using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProgressButton : MonoBehaviour
{
    private Image progress;
    private Animator[] animators;
    private bool isOnHover = false;
    private bool isOnHoverFlag = false;
    public bool IsOnHover { get => isOnHoverFlag; }

    [SerializeField]
    private bool progressComplete;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private float progressTime = 0;
    private Action progressCompleteAction;

    public float maxTime = 1.0f;

    public bool ProgressComplete { get => progressComplete; }
    public float ProgressTime { get => progressTime; }
    public Action ProgressCompleteAction { set => progressCompleteAction = value; }
    public bool IsActive { get => isActive; }

    Camera c;

    void Start() {
        c = Camera.main;
        progressComplete = false;
        isActive = false;
        progressCompleteAction = null;

        Canvas canvas = GetComponentInChildren<Canvas>(true);
        progress = canvas.GetComponentsInChildren<Image>(true)[1];
        animators = GetComponentsInChildren<Animator>();
        animators[0].Play("ProgressOffAnimation");

        SetupLabelClickEvents();
    }

    void Update() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            OnClickForLockedCursor();
            OnHoverForLockedCursor();
        }

        if (isActive)
            updateProgress();
    }

    private void ResetProgress() {
        progressComplete = false;
        progressTime = 0;
        progress.GetComponent<RectTransform>().offsetMax = new Vector2(-200, 0);
    }

    public void SetActive(bool value) {
        if (!value)
            ResetProgress();
    }

    public void SetShow(bool value, float delay) {

        if(value)  {
            animators[0].Play("ProgressShowAnimation");
            ResetProgress();
        }
        else if(isActive) { 
                animators[0].Play("ProgressHideAnimation");
        }

        isActive = value;
    }

    private void SetupLabelClickEvents() {
        EventTrigger trigger = GetComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnPointerDown());
        trigger.triggers.Add(pointerDown);

        trigger = GetComponent<EventTrigger>();
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => OnPointerUp());
        trigger.triggers.Add(pointerUp);

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnHoverEnter(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnHoverExit(); });
        trigger.triggers.Add(exit);
    }

    private void OnPointerDown() {
        startProgress();
    }

    private void OnPointerUp() {
        checkProgress();
    }

    private void OnHoverEnter(){
        isOnHoverFlag = true;

        if (isActive)
            animators[0].Play("ProgressHoverAnimation");
    }

    private void OnHoverExit(){
        isOnHoverFlag = false;

        if (!isActive)
            return;

        checkProgress();

        if (progressComplete){
            isActive = true;
            ResetProgress();
        }

        animators[0].Play("ProgressIdleAnimation");
    }

    public void startProgress() {
        if (!isActive)
            return;
        
        GameManager.State = GameState.Interact;

        progressTime += Time.deltaTime;
        animators[0].Play("ProgressHoverAnimation");
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

    public void checkProgress() {
        if (!isActive)
            return;

        GameManager.State = GameState.Play;

        if (progressTime == maxTime ) {
            if(!progressComplete) {
                progressComplete = true;
                progressTime = 0;
                progressCompleteAction?.Invoke();
            }
        }
        else  {
            progressTime = 0;
            progress.GetComponent<RectTransform>().offsetMax = new Vector2(-200, 0);
        }
    }
    
    public void checkmarkAnimate() {
        animators[0].Play("ProgressCompleteAnimation");
        animators[1].Play("CompleteCheckAnimation");
        AudioManager.Play("UI_CheckmarkComplete");
    }

    public void exAnimate()
    {
        Debug.Log(animators.Length);
        animators[0].Play("ProgressCompleteAnimation");
        animators[2].Play("CompleteExAnimation");
        AudioManager.Play("UI_CheckmarkComplete");
    }

    public void OnClickForLockedCursor()
    {
        if (PauseManager.isPaused == true)
            return;

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = c.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f)) {
                bool hitThis = hit.transform.GetInstanceID() == transform.GetInstanceID();
                if (!hitThis)
                    return;

                OnPointerDown();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            RaycastHit hit;
            Ray ray = c.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f)) {
                bool hitThis = hit.transform.GetInstanceID() == transform.GetInstanceID();
                if (!hitThis)
                    return;

                OnPointerUp();
            }
        }
    }

    public void OnHoverForLockedCursor()
    {
        if (PauseManager.isPaused == true)
            return;

        RaycastHit hit;
        Ray ray = c.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f)) {
            bool hitThis = hit.transform.GetInstanceID() == transform.GetInstanceID();
            if (hitThis) {
                if (!isOnHover) {
                    OnHoverEnter();
                }
                isOnHover = true;
            }
            else {
                if (isOnHover) {
                    OnHoverExit();
                }
                isOnHover = false;
            }
        }
    }
}

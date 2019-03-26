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
    private bool isOnHover = false;

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

        canvas = GetComponentInChildren<Canvas>(true);
        progress = canvas.GetComponentsInChildren<Image>(true)[1];
        checkmark = canvas.GetComponentsInChildren<Image>(true)[2];
        animators = GetComponentsInChildren<Animator>();

        canvas.gameObject.SetActive(false);
        checkmark.gameObject.SetActive(false);

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

    private void OnHoverEnter() {
        animators[0].Play("ProgressHoverAnimation");
    }

    private void OnHoverExit() {
        checkProgress();

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

    public void OnClickForLockedCursor()
    {
        if (PauseManager.isPaused == true)
            return;

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f)) {
                bool hitThis = hit.transform.GetInstanceID() == transform.GetInstanceID();
                if (!hitThis)
                    return;

                OnPointerDown();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

        Camera c = Camera.main;
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

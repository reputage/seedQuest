using UnityEngine;
using UnityEngine.UI;

using SeedQuest.Interactables;

public class ProgressTrackerUI : MonoBehaviour {
    
    public float progress = 0;
    public Sprite tickOnIcon;
    public Sprite tickOffIcon;

    private Image progressBar;
	private Image[] progressTicks;

    void Start() {
        progressBar = GameObject.FindGameObjectWithTag("ProgressBar").GetComponent<Image>();
        progressTicks = GameObject.FindGameObjectWithTag("ProgressTicks").GetComponentsInChildren<Image>();
    }

    private void Update() {
        setProgress();
        SetProgressBar();
        SetProgressTicks();
    }

    private void setProgress() {
        if (GameManager.Mode == GameMode.Rehearsal) {
            gameObject.SetActive(true);
            progress = InteractablePath.PercentComplete / 100.0f;
        }
        else if (GameManager.Mode == GameMode.Recall) {
            gameObject.SetActive(true);
            progress = InteractableLog.PercentComplete / 100.0f;
        }
        else {
            gameObject.SetActive(false);   
        }            
    }

    private void SetProgressBar() {
        float width = 400.0f;
        float value = width * (progress - 1);
        progressBar.GetComponent<RectTransform>().offsetMax = new Vector2(value, 0);
    }

    private void SetProgressTicks() {
        int value =  (int )Mathf.Round(progress * InteractableConfig.ActionsPerGame);
        for (int i = 0; i < progressTicks.Length; i++) {
            if (i < value)
                progressTicks[i].sprite = tickOnIcon;
            else
                progressTicks[i].sprite = tickOffIcon;
        }
    }
}
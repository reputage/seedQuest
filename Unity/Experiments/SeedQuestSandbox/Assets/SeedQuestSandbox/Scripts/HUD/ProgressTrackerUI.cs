using UnityEngine;
using UnityEngine.UI;

using SeedQuest.Interactables;

public enum ProgressTrackerLocation { TopLeft, BottomCenter };

public class ProgressTrackerUI : MonoBehaviour {

    public ProgressTrackerLocation location = ProgressTrackerLocation.BottomCenter;
    public float progress = 0;
    public Sprite tickOnIcon;
    public Sprite tickOffIcon;

    private RectTransform progressCanvas;
    private Image progressBar;
	private Image[] progressTicks;

    void Start() {
        progressCanvas = GameObject.FindGameObjectWithTag("ProgressCanvas").GetComponent<RectTransform>();
        progressBar = GameObject.FindGameObjectWithTag("ProgressBar").GetComponent<Image>();
        progressTicks = GameObject.FindGameObjectWithTag("ProgressTicks").GetComponentsInChildren<Image>();

        SetProgressBar();
        SetProgressTicks();
        SetLocation();
    }

    private void Update() {
        setProgress();
        SetProgressBar();
        SetProgressTicks();
        SetLocation();
    }

    private void setProgress() {
        if (GameManager.Mode == GameMode.Rehearsal) {
            progress = InteractablePath.PercentComplete / 100.0f;
        }
        else if (GameManager.Mode == GameMode.Recall) {
            progress = InteractableLog.PercentComplete / 100.0f;
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

    private void SetLocation() {
        if (location == ProgressTrackerLocation.BottomCenter) {
            progressCanvas.anchorMax = new Vector2(0.5f, 0);
            progressCanvas.anchorMin = new Vector2(0.5f, 0);
            progressCanvas.pivot = new Vector2(0.5f, 0);
            progressCanvas.anchoredPosition3D = Vector3.zero;
        }
        else if (location == ProgressTrackerLocation.TopLeft) {
            progressCanvas.anchorMax = new Vector2(0, 1);
            progressCanvas.anchorMin = new Vector2(0, 1);
            progressCanvas.pivot = new Vector2(0.5f, 0);
            progressCanvas.anchoredPosition3D = new Vector3(265, -380, 0);
        }
    }
}
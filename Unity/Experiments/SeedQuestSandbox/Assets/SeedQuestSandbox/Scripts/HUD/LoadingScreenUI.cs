using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SeedQuest.Interactables;

public class LoadingScreenUI : MonoBehaviour
{
    static private LoadingScreenUI instance = null;
    static private LoadingScreenUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<LoadingScreenUI>(true); return instance; }
    static public LoadingScreenUI Instance { get { return instance == null ? setInstance() : instance; } }

    private Slider progressSlider;

    public float loadProgress = 0;
    static public float LoadProgress { get => Instance.loadProgress; set => Instance.loadProgress = value; }
    static private bool show = false;
    static public bool Show {
        get {
            return show;
        }
        set {
            show = value;
            if(Instance != null)
                Instance.gameObject.SetActive(show);
        }
    }

    private void Awake() {
        progressSlider = GetComponentInChildren<Slider>();
    }

    private void Update() {            
        progressSlider.value = loadProgress;
    }

    AsyncOperation operation = null;
    IEnumerator LoadAsync(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);

        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (LoadingScreenUI.Instance != null)
                LoadingScreenUI.LoadProgress = progress;

            if (operation.progress >= 0.9f) {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    static public void LoadScene(string sceneName) {
        if (Instance == null) {
            HUDManager.InstantiateHUDElement<LoadingScreenUI>();
            if (Instance == null) return;
        }

        if (Instance == null) return;
        LoadingScreenUI.Show = true;
        GameManager.State = GameState.Play;
        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();
        Instance.StartCoroutine(Instance.LoadAsync(sceneName));
    }

    static public void LoadRehearsal(string sceneName) {
        if (Instance == null) {
            HUDManager.InstantiateHUDElement<LoadingScreenUI>();
            if (Instance == null) return;
        }

        if (Instance == null) return;
        LoadingScreenUI.Show = true;
        GameManager.Mode = GameMode.Rehearsal;
        GameManager.State = GameState.Play;
        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();
        Instance.StartCoroutine(Instance.LoadAsync(sceneName));
    }

    static public void LoadRecall(string sceneName) {
        if (Instance == null) {
            HUDManager.InstantiateHUDElement<LoadingScreenUI>();
            if (Instance == null) return;
        }

        LoadingScreenUI.Show = true;
        GameManager.Mode = GameMode.Recall;
        GameManager.State = GameState.Play;
        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();
        Instance.StartCoroutine(Instance.LoadAsync(sceneName));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using SeedQuest.Interactables;

public class SceneLineUpCanvas : MonoBehaviour
{
    public GameObject continueButton;
    public Image[] worldImages;
    public Image[] worldOutlines;
    public TextMeshProUGUI[] worldText;

    public void Start() {
        Initialize();
    }

    public void Update() {
        
    }

    public void Initialize() {

        int count = 0;
        foreach (Image outline in worldOutlines) {
            outline.gameObject.SetActive(false);

            if(count >= InteractableConfig.SitesPerGame)
                outline.transform.parent.gameObject.SetActive(false);
            count++;
        }

        SetImages();
    }

    public void ToggleOn() {
        gameObject.SetActive(true);
    }

    public void SetupLoading(int index) {
        foreach (Image outline in worldOutlines) {
            outline.gameObject.SetActive(false);
        }
        worldOutlines[index].gameObject.SetActive(true);
    }

    public void SetImages() {
        for (int i = 0; i < InteractableConfig.SitesPerGame; i++) {
            worldImages[i].sprite = WorldManager.CurrentSceneList[i].preview;
            worldText[i].text = WorldManager.CurrentSceneList[i].name;
        }
    }

    public void Continue() {
        MenuScreenV2.Instance.GoToActionLineUp();
    }

    IEnumerator LoadAsync(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone) {
            //sceneLoadProgressValue = Mathf.Clamp01(operation.progress / 0.9f);

            if (operation.progress >= 0.9f) {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        //sceneLoadProgress.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
    }

    public void StartScene() {
        GameManager.State = GameState.Menu;
        CameraZoom.ResetZoom();
        InteractableLabelUI.ClearInteractableUI();
        SetImages();

        continueButton.gameObject.SetActive(false);
        if(WorldManager.CurrentWorldScene != null)
            StartCoroutine(LoadAsync(WorldManager.CurrentWorldScene.sceneName));
    }
}
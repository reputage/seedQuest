using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SeedQuest.Interactables;

public class EncodeSeedCanvas : MonoBehaviour {
    private EncodeSeed_ScenePreview[] worldPreviews;
    private int[] currentList = new int[6];

    private int sceneCount;
    private SceneSelectedIndicator[] indicators;
    private Button[] buttons;
    private Button continueButton;

    private void Start() {
        sceneCount = 0;
        Initialize();
        UnInteractiveButtons();
        EnableNext();
    }

    public void Initialize() {
        indicators = GetComponentsInChildren<SceneSelectedIndicator>();
        worldPreviews = GetComponentsInChildren<EncodeSeed_ScenePreview>();
        foreach(EncodeSeed_ScenePreview item in worldPreviews) {
            item.preview.gameObject.SetActive(false);
            item.text.gameObject.SetActive(false);
        }

        Button[] _buttons = GetComponentsInChildren<Button>();
        buttons = new Button[16];
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i] = _buttons[i + 1];
        }

        continueButton = _buttons[17];
        continueButton.gameObject.SetActive(false);
    }

    public void SelectScene(int i) {
        if (sceneCount >= currentList.Length) return;

        currentList[sceneCount] = i;
        worldPreviews[sceneCount].preview.gameObject.SetActive(true);
        worldPreviews[sceneCount].preview.sprite = WorldManager.WorldScenes[i].preview;
        worldPreviews[sceneCount].text.gameObject.SetActive(true);
        worldPreviews[sceneCount].text.text = WorldManager.WorldScenes[i].name;
        worldPreviews[sceneCount].shade.gameObject.SetActive(true);
        indicators[i].Activate(sceneCount);
        sceneCount++;

        buttons[i].GetComponentsInChildren<Image>()[3].gameObject.SetActive(false);
        EnableNext();

        if (sceneCount >= currentList.Length)
            continueButton.gameObject.SetActive(true);
    }

    public void UnInteractiveButtons() {
        if (GameManager.Mode == GameMode.Rehearsal) {
            foreach(Button button in buttons) {
                button.interactable = false;
                button.GetComponentsInChildren<Image>(true)[3].gameObject.SetActive(true);
            }
        }
        else {
            foreach (Button button in buttons) {
                button.interactable = true;
                button.GetComponentsInChildren<Image>(true)[3].gameObject.SetActive(false);
            }
        } 
    }

    public void EnableNext() {
        if (GameManager.Mode != GameMode.Rehearsal) return;

        int[] siteIDs = InteractablePathManager.GetPathSiteIDs();
        if(sceneCount < siteIDs.Length) {
            int nextID = siteIDs[sceneCount];
            buttons[nextID].interactable = true;
            indicators[nextID].Activate(sceneCount);
        }
    }

    public void SetWorldScenes() {
        WorldManager.Reset();
        foreach(int sceneIndex in currentList) {
            WorldManager.Add(sceneIndex);
        }
    }

    public void Continue() {
        if (DebugSeedUtility.debugLearnRun)
        {
            Debug.Log("Starting debug run!");
            SetWorldScenes();
            MenuScreenV2.Instance.ResetCanvas();
            startDebugRun();
        }
        else
        {
            SetWorldScenes();
            MenuScreenV2.Instance.GoToSceneLineUp();
        }
    }

    public void resetCanvas()
    {
        sceneCount = 0;
        continueButton.gameObject.SetActive(false);
        worldPreviews = GetComponentsInChildren<EncodeSeed_ScenePreview>();
        foreach (EncodeSeed_ScenePreview item in worldPreviews)
        {
            item.preview.gameObject.SetActive(false);
            item.text.gameObject.SetActive(false);
            item.shade.gameObject.SetActive(false);
        }
        foreach (SceneSelectedIndicator indicator in indicators)
        {
            indicator.Reset();
        }
        UnInteractiveButtons();
        EnableNext();
    }

    public void backButton()
    {
        MenuScreenV2.Instance.GoToStart();
    }

    public void startDebugRun()
    {
        if (DebugSeedUtility.debugLearnRand)
            DebugSeedUtility.startRandom();
        else
            DebugSeedUtility.startIterative();
    }
} 
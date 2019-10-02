using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SeedQuest.Interactables;

public class EncodeSeedCanvas : MonoBehaviour {
    private EncodeSeed_ScenePreview[] worldPreviews;
    private int[] currentList;

    private int sceneCount;
    private SceneSelectedIndicator[] indicators;
    private Button[] buttons;
    private Button continueButton;
    private bool initialized = false;

    private void Start() {
        sceneCount = 0;
        Initialize();
        UnInteractiveButtons();
        EnableNext();
    }

    private void Update() {
        bool doUpdate = GetComponentInChildren<SeedStrSelection>(true).updateFlag;

        if(doUpdate) {
            resetCanvas();
            Initialize();
            UnInteractiveButtons();
            EnableNext();

            GetComponentInChildren<SeedStrSelection>().updateFlag = false;
        }
    }

    public void Initialize() {
        InitalizeWorldPreviews();

        if (GameManager.Mode == GameMode.Rehearsal)
            GetComponentInChildren<SeedStrSelection>(true).gameObject.SetActive(false);
        else if (GameManager.Mode == GameMode.Recall)
            GetComponentInChildren<SeedStrSelection>(true).gameObject.SetActive(true);
        indicators = GetComponentsInChildren<SceneSelectedIndicator>(true);
        Button[] _buttons = GetComponentsInChildren<Button>(true);
        buttons = new Button[16];
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i] = _buttons[i + 1];
        }


        continueButton = _buttons[17];
        continueButton.gameObject.SetActive(false);
        initialized = true;
    }

    public void InitalizeWorldPreviews() {
        currentList = new int[InteractableConfig.SitesPerGame];
        worldPreviews = GetComponentsInChildren<EncodeSeed_ScenePreview>(true);
        int count = 0;
        foreach (EncodeSeed_ScenePreview item in worldPreviews) {
            item.gameObject.SetActive(true);
            item.preview.gameObject.SetActive(false);
            item.text.gameObject.SetActive(false);

            if(count >= InteractableConfig.SitesPerGame) {
                item.gameObject.SetActive(false);
            }
            count++;
        }
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
        ResetInteractiveButtons();
        EnableNext();

        if (sceneCount >= currentList.Length)
            continueButton.gameObject.SetActive(true);
        
        buttons[i].GetComponentsInChildren<Image>(true)[3].gameObject.SetActive(false);
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

    public void ResetInteractiveButtons()
    {
        if (GameManager.Mode == GameMode.Rehearsal){
            foreach (Button button in buttons){
                button.interactable = false;
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
        if (DebugSeedUtility.debugLearnRun) {
            SetWorldScenes();
            MenuScreenV2.Instance.ResetCanvas();
            startDebugRun();
        }
        else {
            SetWorldScenes();
            MenuScreenV2.Instance.GoToSceneLineUp();
        }

        continueButton.gameObject.SetActive(false);
    }

    public void resetCanvas() {
        sceneCount = 0;
        Initialize();

        worldPreviews = GetComponentsInChildren<EncodeSeed_ScenePreview>();
        foreach (EncodeSeed_ScenePreview item in worldPreviews) {
            item.preview.gameObject.SetActive(false);
            item.text.gameObject.SetActive(false);
            item.shade.gameObject.SetActive(false);
        }
        if (initialized) {
            foreach (SceneSelectedIndicator indicator in indicators) {
                indicator.Reset();
            }

            UnInteractiveButtons();
            EnableNext();
        }
    }

    public void undoSelect()
    {
        if (sceneCount <= 0) return;

        sceneCount -= 1;
        int i = currentList[sceneCount];

        currentList[sceneCount] = 0;
        worldPreviews[sceneCount].preview.gameObject.SetActive(false);
        worldPreviews[sceneCount].text.gameObject.SetActive(false);
        worldPreviews[sceneCount].shade.gameObject.SetActive(false);
        ResetInteractiveButtons();
        EnableNext();

        if (sceneCount < 6)
            indicators[i].Deactivate(sceneCount);
        
        continueButton.gameObject.SetActive(false);
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

    public void resetSeedStr()
    {
        if (GameManager.Mode == GameMode.Recall)
        {
            GetComponentInChildren<SeedStrSelection>().reset();
        }
    }
} 
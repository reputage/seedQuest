using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using SeedQuest.Interactables;

public enum MenuScreenStates { Start, ModeSelect, SeedSetup, EncodeSeed, SceneLineUp, ActionLineUp, Debug }

public class MenuScreenManager : MonoBehaviour
{
    static private MenuScreenManager instance = null;
    static private MenuScreenManager setInstance() { instance = GameObject.FindObjectOfType<MenuScreenManager>(); return instance; }
    static public MenuScreenManager Instance { get { return instance == null ? setInstance() : instance; } }

    public GameObject warningText;

    public MenuScreenStates state = MenuScreenStates.Start;
    private Canvas[] canvas;
    private Canvas startCanvas;
    private Canvas motionBackgroundCanvas;
    private Canvas encodeSeedCanvas;
    [HideInInspector]
    public Canvas encodeSeedContinueCanvas;
    private Canvas sceneLineUpCanvas;
    private Canvas actionLineUpCanvas;

    private float sceneLoadProgressValue;
    private Slider sceneLoadProgress;
    private Button sceneContinueButton;

    public void Awake()
    {
        canvas = GetComponentsInChildren<Canvas>(true);
        motionBackgroundCanvas = canvas[1];
        startCanvas = canvas[2];
        encodeSeedCanvas = canvas[5];
        encodeSeedContinueCanvas = canvas[6];
        sceneLineUpCanvas = canvas[7];
        actionLineUpCanvas = canvas[8];

        sceneLoadProgress = GetComponentInChildren<Slider>(true);
        sceneContinueButton = sceneLineUpCanvas.GetComponentInChildren<Button>(true);
    }

    public void Start()
    {
        if (MenuScreenManager.Instance.state == MenuScreenStates.Debug)
            return;

        motionBackgroundCanvas.gameObject.SetActive(true);
        GoToStart();
    }

    private void Update()
    {
        RotateBackground();

        sceneLoadProgress.value = sceneLoadProgressValue;
    }

    private void SetBackground(bool active)
    {
        motionBackgroundCanvas.gameObject.SetActive(active);
    }

    private void ResetCanvas()
    {
        canvas[2].gameObject.SetActive(false);
        canvas[3].gameObject.SetActive(false);
        canvas[4].gameObject.SetActive(false);
        canvas[5].gameObject.SetActive(false);
        canvas[6].gameObject.SetActive(false);
        canvas[7].gameObject.SetActive(false);
        canvas[8].gameObject.SetActive(false);
    }

    public void GoToStart()
    {
        state = MenuScreenStates.Start;
        ResetCanvas();
        startCanvas.gameObject.SetActive(true);
        SetupRotateBackground(0);
    }

    static public void ActivateStart() {
        InteractablePathManager.Reset();
        LevelSetManager.ResetCurrentLevels();
        LevelIconButton.ResetButtonIcons();

        Instance.SetBackground(true);
        Instance.GoToStart();
    }

    public void GoToModeSelect()
    {
        state = MenuScreenStates.ModeSelect;
        ResetCanvas();
        canvas[3].gameObject.SetActive(true);
        SetupRotateBackground(150);
    }

    public void SetModeLearnSeed()
    {
        GameManager.Mode = GameMode.Rehearsal;
        GoToSeedSetup();
    }

    public void SetModeRecoverSeed()
    {
        GameManager.Mode = GameMode.Recall;
        GoToEncodeSeed();
    }

    public void GoToSeedSetup()
    {
        state = MenuScreenStates.SeedSetup;
        ResetCanvas();
        canvas[4].gameObject.SetActive(true);
        SetupRotateBackground(270);
        SetupSeedSetup();
    }

    public void GoToEncodeSeed()
    {
        state = MenuScreenStates.EncodeSeed;
        ResetCanvas();
        encodeSeedCanvas.gameObject.SetActive(true);
        SetupRotateBackground(330);
        SetupEncodeSeed();
    }

    public void GoToEncodeSeedFromSeedSetup() {
        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        bool validSeed = validSeedString(seedInputField.text);
        if (validSeed)
        {
            Debug.Log("Seed: " + seedInputField.text);
            GoToEncodeSeed();
        }
    }

    public void GoToSceneLineUp()
    {
        state = MenuScreenStates.SceneLineUp;
        ResetCanvas();
        sceneLineUpCanvas.gameObject.SetActive(true);
        SetupSceneLineUp();
    }

    static public void ActivateSceneLineUp() {
        Instance.SetBackground(true);
        Instance.GoToSceneLineUp();
    }

    public void GoToActionLineUp()
    {
        if (GameManager.Mode == GameMode.Rehearsal)
        {
            state = MenuScreenStates.ActionLineUp;
            ResetCanvas();
            actionLineUpCanvas.gameObject.SetActive(true);
            SetupActionLineUp();
        }
        else
        {
            CloseMenuScreen();
        }
    }

    public void SetupSeedSetup()
    {
        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        seedInputField.text = InteractablePathManager.SeedString;
        seedInputField.characterLimit = InteractableConfig.SeedHexLength;
    }

    public void SetupEncodeSeed()
    {
        SetLevelPanelDefault();

        if (GameManager.Mode == GameMode.Rehearsal)
        {
            TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>(true);
            InteractablePathManager.SeedString = seedInputField.text;

            int[] siteIDs = InteractablePathManager.GetPathSiteIDs();
            SetIconAndPanelForRehearsal(siteIDs); 
        }
    }

    public void SetupSceneLineUp()
    {
        Image lineUp = sceneLineUpCanvas.GetComponentsInChildren<Image>(true)[5];
        lineUp.transform.localPosition = new Vector3(-600 * InteractableLog.CurrentLevelIndex, 35, 0);

        LevelPanel[] panels = sceneLineUpCanvas.GetComponentsInChildren<LevelPanel>();
        int index = 0;
        foreach (LevelPanel panel in panels)
        {
            panel.GetComponentsInChildren<Image>()[1].sprite = LevelSetManager.CurrentLevels[index].preview;
            index++;
        }
        Image icon = sceneLineUpCanvas.GetComponentsInChildren<Image>()[1];
        icon.sprite = LevelSetManager.CurrentLevel.icon;
        TextMeshProUGUI text = sceneLineUpCanvas.GetComponentsInChildren<TextMeshProUGUI>()[2];
        text.text = LevelSetManager.CurrentLevel.name;

        sceneLoadProgress.gameObject.SetActive(true);
        sceneContinueButton.gameObject.SetActive(false);
        StartScene();
    }

    public void SetupActionLineUp()
    {
        Image preview = actionLineUpCanvas.GetComponentsInChildren<Image>()[2];
        preview.sprite = LevelSetManager.CurrentLevel.preview;
        Image icon = actionLineUpCanvas.GetComponentsInChildren<Image>()[3];
        icon.sprite = LevelSetManager.CurrentLevel.icon;
        TextMeshProUGUI text = actionLineUpCanvas.GetComponentsInChildren<TextMeshProUGUI>()[0];
        text.text = LevelSetManager.CurrentLevel.name;

        Image[] images = actionLineUpCanvas.GetComponentsInChildren<Image>();
        images[5].gameObject.SetActive(false);
        images[6].gameObject.SetActive(false);
        images[7].gameObject.SetActive(false);
        images[8].gameObject.SetActive(false);

        TextMeshProUGUI[] texts = actionLineUpCanvas.GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void CloseSceneLineUp()
    {
        IsometricCamera.StartLevelZoomIn();
        CloseMenuScreen();
    }

    public void CloseMenuScreen()
    {
        ResetCanvas();
        SetBackground(false);
    }

    IEnumerator LoadAsync(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {

            sceneLoadProgressValue = Mathf.Clamp01(operation.progress / 0.9f);

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;

                sceneLoadProgress.gameObject.SetActive(false);
                sceneContinueButton.gameObject.SetActive(true);
            }

            yield return null;
        }
    }

    public void StartScene()
    {
        GameManager.State = GameState.Menu;
        IsometricCamera.ResetLevelZoomIn();
        Instance.StartCoroutine(Instance.LoadAsync(LevelSetManager.CurrentLevel.scenename));
    }

    static public void SetIconAndPanelForRehearsal(int[] siteIDs) {
        int orderIndex = 0;
        foreach (int siteID in siteIDs) {
            LevelIconButton.ActivateIconForRehersal(siteID, orderIndex);
            orderIndex++;
        }
    }

    public void SetLevelPanelDefault() {
        LevelPanel[] levelPanels = encodeSeedCanvas.GetComponentsInChildren<LevelPanel>();
        foreach(LevelPanel panel in levelPanels) {
            panel.GetComponentsInChildren<Image>(true)[2].gameObject.SetActive(false);
            panel.GetComponentsInChildren<TextMeshProUGUI>(true)[0].gameObject.SetActive(false);
            panel.GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.SetActive(false);
        }
    }

    static public void SetLevelPanel(int panelIndex, int levelIndex)
    {
        LevelSetManager.AddLevel(levelIndex);

        LevelPanel selectedPanel = Instance.encodeSeedCanvas.GetComponentsInChildren<LevelPanel>()[panelIndex];
        selectedPanel.GetComponentsInChildren<Image>(true)[2].gameObject.SetActive(true);
        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>(true)[0].gameObject.SetActive(true);
        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.SetActive(true);

        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>()[1].text = LevelSetManager.AllLevels[levelIndex].name;
        selectedPanel.GetComponentsInChildren<Image>()[2].sprite = LevelSetManager.AllLevels[levelIndex].preview;

        if (panelIndex == 3)
        {
            Instance.encodeSeedContinueCanvas.gameObject.SetActive(true);
        }
    }

    private Vector3 rotate = new Vector3(0, 0, 0);
    private Vector3 targetRotate = new Vector3(0, 0, 0);
    private float time = 0;
    public void SetupRotateBackground(float angle)
    {
        if (angle > 180)
            motionBackgroundCanvas.GetComponentsInChildren<Image>(true)[4].gameObject.SetActive(false);
        else
            motionBackgroundCanvas.GetComponentsInChildren<Image>(true)[4].gameObject.SetActive(true);

        rotate.z = targetRotate.z;
        targetRotate.z = angle;
        time = Time.time;
    }

    public void RotateBackground()
    {
        float timeDuration = 1.0f;
        float t = Mathf.Clamp01((Time.time - time) / timeDuration);
        Vector3 newRotate = Vector3.Lerp(rotate, targetRotate, t);
        motionBackgroundCanvas.GetComponentInChildren<RectTransform>().localRotation = Quaternion.Euler(newRotate);
    }

    public void SetRandomSeed()
    {
        InteractablePathManager.SetRandomSeed();

        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        seedInputField.text = InteractablePathManager.SeedString;
    }

    public bool validSeedString(string seedString)
    {
        bool validHex = true;
        foreach (var hexChar in seedString)
        {
            validHex = ((hexChar >= '0' && hexChar <= '9') ||
                        (hexChar >= 'a' && hexChar <= 'f') ||
                        (hexChar >= 'A' && hexChar <= 'F'));
        }

        if (!validHex)
            warningText.GetComponent<TextMeshProUGUI>().text = "Warning: seed must only contain hex characters";
        else if (seedString.Length < 28)
        {
            // send warning message that the length is too short
            validHex = false;
            warningText.GetComponent<TextMeshProUGUI>().text = "Warning: seed must be 28 characters long";
        }

        return validHex;
    }

}

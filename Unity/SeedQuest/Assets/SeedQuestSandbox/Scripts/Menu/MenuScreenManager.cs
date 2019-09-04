using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices;

using SeedQuest.Interactables;

public enum MenuScreenStates { Start, ModeSelect, SeedSetup, EncodeSeed, SceneLineUp, ActionLineUp, Debug }

public class MenuScreenManager : MonoBehaviour
{
    #if UNITY_WEBGL
        [DllImport("__Internal")]
        public static extern void Paste(string gameObj);
    #endif

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
    private Canvas backButtonCanvas;
    private Canvas debugCanvas;
    private Canvas startDebugCanvas;

    private float sceneLoadProgressValue;
    private Slider sceneLoadProgress;
    private Button sceneContinueButton;
    private bool isBip;
    private bool debugRandom;
    private bool counting;
    public bool isDebug;

    private Image greenCheck;
    private Image redWarning;
    private Image greenOutline;
    private Image redOutline;

    private InteractableAutoCounter autoCounter;

    public void Awake()
    {
        isBip = false;
        isDebug = false;
        canvas = GetComponentsInChildren<Canvas>(true);
        motionBackgroundCanvas = canvas[1];
        startCanvas = canvas[2];
        encodeSeedCanvas = canvas[5];
        encodeSeedContinueCanvas = canvas[6];
        sceneLineUpCanvas = canvas[7];
        actionLineUpCanvas = canvas[8];
        backButtonCanvas = canvas[9];
        startDebugCanvas = canvas[10];
        debugCanvas = canvas[11];

        Image[] images = canvas[4].GetComponentsInChildren<Image>();
        // Unity is bugged and doesn't allow you to properly re-order child objects of prefabs
        greenCheck = images[5];
        redWarning = images[7];
        redOutline = images[8];
        greenOutline = images[9];
        deactivateCheckSymbols();

        sceneLoadProgress = GetComponentInChildren<Slider>(true);
        sceneContinueButton = sceneLineUpCanvas.GetComponentInChildren<Button>(true);

        autoCounter = GetComponentInChildren<InteractableAutoCounter>(true);
    }

    public void Start()
    {
        if (MenuScreenManager.Instance.state == MenuScreenStates.Debug) {
            return;
        }
        else {
            GameManager.State = GameState.Menu;
        }

        motionBackgroundCanvas.gameObject.SetActive(true);
        GoToStart();
    }

    private void Update()
    {
        RotateBackground();

        sceneLoadProgress.value = sceneLoadProgressValue;

        if (state == MenuScreenStates.ModeSelect && Input.GetKeyDown(KeyCode.BackQuote))
        {
            GoToDebugCanvas();
        }
        if (counting)
        {
            if (autoCounter.finished)
            {
                setCounterDebugText();
                counting = false;
            }
        }

    #if UNITY_WEBGL
        if (state == MenuScreenStates.SeedSetup)
        {
            if (Input.GetKeyDown(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftCommand)))
            {
                TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
                Paste(seedInputField.name);
            }
        }
    #endif
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
        canvas[9].gameObject.SetActive(false);
        canvas[10].gameObject.SetActive(false);
        canvas[11].gameObject.SetActive(false);

    }

    public void GoToStart()
    {
        state = MenuScreenStates.Start;
        ResetCanvas();
        startCanvas.gameObject.SetActive(true);
        SetupRotateBackground(0);
    }

    public void GoBack()
    {
        GameManager.Mode = GameMode.Rehearsal;
        isDebug = false;

        if (state == MenuScreenStates.EncodeSeed)
        {
            LevelIconButton.ResetButtonIcons();
            LevelIconButton.ResetButtonStatus();
        }

        GoToModeSelect();
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
        AudioManager.Play("UI_StartButton");

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
        Debug.Log("Setting game mode to recall");
        GoToEncodeSeed();
        LevelIconButton.ResetButtonStatus();
    }

    public void GoToSeedSetup()
    {
        state = MenuScreenStates.SeedSetup;
        ResetCanvas();
        canvas[4].gameObject.SetActive(true);
        SetupRotateBackground(270);
        SetupSeedSetupBip();
        checkInputSeed();

        canvas[9].gameObject.SetActive(true);
        var buttonGroup = canvas[9].transform.GetChild(0);
        buttonGroup.localPosition = new Vector3(-640, 340, 0);
    }

    public void GoToEncodeSeed()
    {
        state = MenuScreenStates.EncodeSeed;
        ResetCanvas();
        LevelSetManager.ResetCurrentLevels();
        encodeSeedCanvas.gameObject.SetActive(true);
        SetupRotateBackground(330);
        SetupEncodeSeedBip();

        canvas[9].gameObject.SetActive(true);
        var buttonGroup = canvas[9].transform.GetChild(0);
        buttonGroup.localPosition = new Vector3(-790, 380, 0);
    }

    public void GoToEncodeSeedFromSeedSetup() {
        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        string seed = removeHexPrefix(seedInputField.text);
        bool validSeed = validSeedString(seed);

        if (validSeed)
        {
            Debug.Log("Valid hex seed: " + seed);
            GoToEncodeSeed();
        }
        else if (validBip(seed))
        {
            Debug.Log("Valid bip39 seed: " + seed);
            GoToEncodeSeed();
        }

    }

    public void UndoLastSceneEncodeStep() {
        HideLevelPanel(LevelIconButton.activeIndex);
        LevelIconButton.Undo();
    }

    public void GoToSceneLineUp()
    {
        GameManager.State = GameState.Menu;
        state = MenuScreenStates.SceneLineUp;
        ResetCanvas();
        sceneLineUpCanvas.gameObject.SetActive(true);
        SetupSceneLineUp();
    }

    static public void ActivateSceneLineUp() {
        Instance.SetBackground(true);
        Instance.GoToSceneLineUp();
    }

    public void GoToActionLineUp() {
        GameManager.State = GameState.Menu;
        if (GameManager.Mode == GameMode.Rehearsal) {
            state = MenuScreenStates.ActionLineUp;
            ResetCanvas();
            actionLineUpCanvas.gameObject.SetActive(true);
            SetupActionLineUp();
        }
        else {
            CloseSceneLineUp();
        }
    }

    public void SetupSeedSetup()
    {
        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        seedInputField.text = InteractablePathManager.SeedString;
        int charLimit = InteractableConfig.SeedHexLength;

        seedInputField.characterLimit = charLimit;
    }

    public void SetupSeedSetupBip()
    {
        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        seedInputField.text = checkHexLength(InteractablePathManager.SeedString);
        int charLimit = 700;

        seedInputField.characterLimit = charLimit;
    }

    public void SetupEncodeSeed()
    {
        SetLevelPanelDefault();

        if (GameManager.Mode == GameMode.Rehearsal)
        {
            TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>(true);

            string seedFromInput = removeHexPrefix(seedInputField.text);

            if (InteractableConfig.SeedHexLength % 2 == 1)
            {
                if (seedFromInput.Length == InteractableConfig.SeedHexLength)
                {
                    string seedText = seedFromInput + "0";
                    char[] array = seedText.ToCharArray();
                    array[array.Length - 1] = array[array.Length - 2];
                    array[array.Length - 2] = '0';
                    seedFromInput = new string(array);
                }
                else if (seedFromInput.Length == InteractableConfig.SeedHexLength + 1)
                {
                    char[] array = seedFromInput.ToCharArray();
                    array[array.Length - 2] = '0';
                    seedFromInput = new string(array);
                }
                else
                    Debug.Log("Seed: " + seedFromInput);
            }

            InteractablePathManager.SeedString = seedFromInput;

            int[] siteIDs = InteractablePathManager.GetPathSiteIDs();
            SetIconAndPanelForRehearsal(siteIDs);
        }
    }

    public void SetupEncodeSeedBip()
    {
        SetLevelPanelDefault();

        if (GameManager.Mode == GameMode.Rehearsal)
        {
            TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>(true);

            string seedFromInput = seedInputField.text;
            string hexSeed = "";

            if (!detectHex(seedFromInput) && validBip(seedFromInput))
            {
                BIP39Converter bpc = new BIP39Converter();
                hexSeed = bpc.getHexFromSentence(seedFromInput);
            }
            else
            {
                hexSeed = seedFromInput;
                if (InteractableConfig.SeedHexLength % 2 == 1)
                {
                    if (seedFromInput.Length == InteractableConfig.SeedHexLength)
                    {
                        string seedText = seedFromInput + "0";
                        char[] array = seedText.ToCharArray();
                        array[array.Length - 1] = array[array.Length - 2];
                        array[array.Length - 2] = '0';
                        hexSeed = new string(array);
                    }
                    else if (seedFromInput.Length == InteractableConfig.SeedHexLength + 1)
                    {
                        char[] array = seedFromInput.ToCharArray();
                        array[array.Length - 2] = '0';
                        hexSeed = new string(array);
                    }
                    else
                        Debug.Log("Seed: " + hexSeed);
                }
            }

            //Debug.Log("Sentence: " + seedFromInput);
            Debug.Log("Seed: " + hexSeed);

            InteractablePathManager.SeedString = hexSeed;

            int[] siteIDs = InteractablePathManager.GetPathSiteIDs();

            SetIconAndPanelForRehearsal(siteIDs);
        }
    }

    static public void EnableUndoButton() {
        Instance.encodeSeedCanvas.GetComponentsInChildren<Button>(true)[16].gameObject.SetActive(true);
    }

    static public void DisableUndoButton() {
        Instance.encodeSeedCanvas.GetComponentsInChildren<Button>(true)[16].gameObject.SetActive(false);
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
        Image background = actionLineUpCanvas.GetComponentsInChildren<Image>(true)[5];
        Image currentBackground = sceneLineUpCanvas.GetComponentsInChildren<LevelPanel>()[0].GetComponentInChildren<Image>();
        background.color = currentBackground.color;

        TextMeshProUGUI text = actionLineUpCanvas.GetComponentsInChildren<TextMeshProUGUI>()[1];
        text.text = LevelSetManager.CurrentLevel.name;

        TextMeshProUGUI[] texts = actionLineUpCanvas.GetComponentsInChildren<TextMeshProUGUI>();

        Interactable[] interactables = InteractablePath.Path.ToArray();
        int[] actionIds = InteractablePath.ActionIds.ToArray();
        int sceneIndex = InteractableLog.CurrentLevelIndex;
        int baseIndex = sceneIndex * InteractableConfig.ActionsPerSite;

        for (int i = 0; i < InteractableConfig.ActionsPerSite; i++) {
            Interactable interactable = interactables[baseIndex + i];
            interactable.ID.actionID = actionIds[baseIndex + i];

            texts[2 * i + 3].text = interactable.Name;
            texts[2 * i + 4].text = interactable.RehearsalActionName;
        }

        GameManager.Instance.GetComponentInChildren<ActionLineCameraRig>().Initialize();


    }

    public void CloseSceneLineUp() {
        CameraZoom.StartZoomIn();
        //IsometricCamera.StartLevelZoomIn();
        CloseMenuScreen();
        GameManager.State = GameState.Play;
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

            //sceneLoadProgressValue = Mathf.Clamp01(operation.progress / 0.9f);

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;

                //sceneLoadProgress.gameObject.SetActive(false);
                //sceneContinueButton.gameObject.SetActive(true);
            }

            yield return null;
        }

        sceneLoadProgress.gameObject.SetActive(false);
        sceneContinueButton.gameObject.SetActive(true);
    }

    public void StartScene()
    {
        GameManager.State = GameState.Menu;
        CameraZoom.ResetZoom();
        //IsometricCamera.ResetLevelZoomIn();
        Instance.StartCoroutine(Instance.LoadAsync(LevelSetManager.CurrentLevel.scenename));
    }

    static public void SetIconAndPanelForRehearsal(int[] siteIDs) {
        LevelIconButton.EnableNextIconButton();
    }

    public void SetLevelPanelDefault() {
        LevelPanel[] levelPanels = encodeSeedCanvas.GetComponentsInChildren<LevelPanel>();
        foreach(LevelPanel panel in levelPanels) {
            panel.GetComponentsInChildren<Image>(true)[2].gameObject.SetActive(false);
            panel.GetComponentsInChildren<TextMeshProUGUI>(true)[0].gameObject.SetActive(false);
            panel.GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.SetActive(false);
        }
    }

    static public void SetEncodeSeedContinueCanvas() {
        Instance.encodeSeedContinueCanvas.gameObject.SetActive(true);
    }

    static public void SetLevelPanel(int panelIndex, int levelIndex)
    {
        LevelPanel selectedPanel = Instance.encodeSeedCanvas.GetComponentsInChildren<LevelPanel>()[panelIndex];
        selectedPanel.GetComponentsInChildren<Image>(true)[2].gameObject.SetActive(true);
        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>(true)[0].gameObject.SetActive(true);
        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.SetActive(true);

        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>()[1].text = LevelSetManager.AllLevels[levelIndex].name;
        selectedPanel.GetComponentsInChildren<Image>()[2].sprite = LevelSetManager.AllLevels[levelIndex].preview;
    }

    static public void HideLevelPanel(int panelIndex) {
        if (panelIndex >= 6)
            return;

        LevelPanel selectedPanel = Instance.encodeSeedCanvas.GetComponentsInChildren<LevelPanel>()[panelIndex];
        selectedPanel.GetComponentsInChildren<Image>(true)[2].gameObject.SetActive(false);
        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>(true)[0].gameObject.SetActive(false);
        selectedPanel.GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.SetActive(false);
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
        BIP39Converter bpc = new BIP39Converter();

        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        seedInputField.text = InteractablePathManager.SeedString;
    }

    public void SetRandomBIP39Seed()
    {
        BIP39Converter bpc = new BIP39Converter();
        InteractablePathManager.SetRandomSeed();

        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        seedInputField.text = bpc.getSentenceFromHex(InteractablePathManager.SeedString);
    }

    public void SwitchSeedFormat()
    {
        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();

        if (isBip)
        {
            seedInputField.text = InteractablePathManager.SeedString;
            isBip = false;
        }
        else
        {
            BIP39Converter bpc = new BIP39Converter();
            seedInputField.text = bpc.getSentenceFromHex(InteractablePathManager.SeedString);
            isBip = true;
        }
    }

    public bool detectHex(string seed)
    {
        if (seed.Length <= InteractableConfig.SeedHexLength + 1 && 
                 System.Text.RegularExpressions.Regex.IsMatch(seed, @"\A\b[0-9a-fA-F]+\b\Z"))
        {
            return true;
        }

        //Debug.Log("Seed doesn't appear to be hex.");
        return false;
    }

    public bool validSeedString(string seedString)
    {
        bool validHex = true;
        foreach (char c in seedString)
        {
            validHex = (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || 
                        c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || 
                        c == 'a' || c == 'A' || c == 'b' || c == 'B' || c == 'c' || 
                        c == 'C' || c == 'd' || c == 'D' || c == 'e' || c == 'E' || 
                        c == 'f' || c == 'F') && validHex;
        }

        string[] wordArray = seedString.Split(null);

        if (seedString == "" || seedString.Length < 1)
        {
            deactivateCheckSymbols();
            warningText.GetComponent<TextMeshProUGUI>().text = "";
            validHex = false;
        }
        else if (!validHex && wordArray.Length> 1 && wordArray.Length < 12)
        {
            Debug.Log("array length: " + wordArray.Length);
            warningText.GetComponent<TextMeshProUGUI>().text = "Remember to add spaces between the words.";
            warningText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 20, 20, 255);
            setRedWarning();
        }
        else if (!validHex && wordArray.Length > 1 && !validBip(seedString))
        {
            warningText.GetComponent<TextMeshProUGUI>().text = "Make sure the words are spelled correctly.";
            warningText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 20, 20, 255);
            setRedWarning();

        }
        else if (!validHex)
        {
            warningText.GetComponent<TextMeshProUGUI>().text = "Character seeds must only contain hex characters.";
            warningText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 20, 20, 255);
            setRedWarning();
        }
        else if (seedString.Length < 33)
        {
            // send warning message that the length is too short
            validHex = false;

            warningText.GetComponent<TextMeshProUGUI>().text = "Not enough characters!";
            warningText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 20, 20, 255);
            setRedWarning();
        }
        else if (seedString.Length > 34)
        {
            validHex = false;

            warningText.GetComponent<TextMeshProUGUI>().text = "Too many characters!";
            warningText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 20, 20, 255);
            setRedWarning();
        }

        return validHex;
    }

    public bool validBip(string seed)
    {
        BIP39Converter bpc = new BIP39Converter();
        string hex = "";

        try
        {
            hex = bpc.getHexFromSentence(seed);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
            return false;
        }

        //Debug.Log("hex: " + hex);
        return true;
    }

    public void checkInputSeed()
    {
        //Debug.Log("Hello from checkInputSeed()");
        TMP_InputField seedInputField = GetComponentInChildren<TMP_InputField>();
        string seed = removeHexPrefix(seedInputField.text);
        bool validSeed = validSeedString(seed);

        if (validSeed)
        {
            Debug.Log("Valid hex seed: " + seed);
            warningText.GetComponent<TextMeshProUGUI>().text = "Character seed detected!";
            warningText.GetComponent<TextMeshProUGUI>().color = new Color32(81, 150, 55, 255);
            setGreenCheck();
        }
        else if (validBip(seedInputField.text))
        {
            Debug.Log("Valid bip39 seed: " + seed);
            warningText.GetComponent<TextMeshProUGUI>().text = "Word seed detected!";
            warningText.GetComponent<TextMeshProUGUI>().color = new Color32(81, 150, 55, 255);
            setGreenCheck();
        }
    }

    public void setGreenCheck()
    {
        redWarning.gameObject.SetActive(false);
        redOutline.gameObject.SetActive(false);
        greenCheck.gameObject.SetActive(true);
        greenOutline.gameObject.SetActive(true);
    }

    public void setRedWarning()
    {
        redWarning.gameObject.SetActive(true);
        redOutline.gameObject.SetActive(true);
        greenCheck.gameObject.SetActive(false);
        greenOutline.gameObject.SetActive(false);
    }

    public void deactivateCheckSymbols()
    {
        redWarning.gameObject.SetActive(false);
        redOutline.gameObject.SetActive(false);
        greenCheck.gameObject.SetActive(false);
        greenOutline.gameObject.SetActive(false);
    }

    // Removes the hex prefix from a string, if it has one
    public static string removeHexPrefix(string hexString)
    {
        if (hexString != null && hexString.StartsWith("0x"))
            hexString = hexString.Substring(2);
        return hexString;
    }

    // The end game UI removes a superfluous character from hex strings, this 
    //  fixes a UI issue that arises when restarting from end game UI
    public static string checkHexLength(string hexString)
    {
        if (hexString.Length == 34 && hexString[32] == '0')
        {
            hexString = hexString.Substring(0, 32) + hexString.Substring(33, 1);
        }
        Debug.Log("Fixed string: " + hexString);

        return hexString;   
    }

    public void GoToDebugCanvas()
    {
        ResetCanvas();
        debugCanvas.gameObject.SetActive(true);
    }

    public void GoToEncodeDebugOrdered()
    {
        debugRandom = false;
        isDebug = true;
        debugCanvas.gameObject.SetActive(false);
        state = MenuScreenStates.EncodeSeed;
        SetModeRecoverSeed();
    }

    public void GoToEncodeDebugRand()
    {
        debugRandom = true;
        isDebug = true;
        debugCanvas.gameObject.SetActive(false);
        state = MenuScreenStates.EncodeSeed;
        SetModeRecoverSeed();
    }

    public void startDebugRun()
    {
        if (!debugRandom)
        {
            CloseMenuScreen();
            DebugSeedUtility.startIterative();
        }
        else
        {
            CloseMenuScreen();
            DebugSeedUtility.startRandom();
        }
    }

    public void autoCountInteractables()
    {
        autoCounter.loadFirstScene();
        GoToWaitingForCounter();
    }

    public void GoToWaitingForCounter()
    {
        // This function will be finished at a later time - since 
        //  the start menu is going to be changing anyways.

        // To do: 
        //  deactivate debug card
        //  activate waiting card

        debugCanvas.GetComponentsInChildren<RectTransform>()[4].gameObject.SetActive(false);
        counting = true;
    }

    public void setCounterDebugText()
    {
        Debug.Log("Setting text: " + autoCounter.results);
        TextMeshProUGUI counterText = debugCanvas.GetComponentsInChildren<TextMeshProUGUI>()[0];
        counterText.text = autoCounter.results;
    }

    public static void SetEncodeSeedDebugCanvas()
    {
        Instance.startDebugCanvas.gameObject.SetActive(true);
    }
}

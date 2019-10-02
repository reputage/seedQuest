using System.Collections.Generic;
using System.Linq;
using SeedQuest.Interactables;
using SeedQuest.Level;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FastRecoveryUI : MonoBehaviour
{
    private FastRecoveryData settings;
    private Image map;
    private RawImage rawMap;
    private Image overlay;
    private Image pin;
    private Image mapInstructions;
    private Image preview;
    private Image skip;
    private Image back;
    private List<Button> buttons;
    private GameObject buttonPrefab;
    private GameObject interactableGroup;
    private Button[] interactableButtons;
    private TMPro.TMP_Text mainTitle;
    private TMPro.TMP_Text instructions;
    private TMPro.TMP_Text interactableTitle;
    private TMPro.TMP_Text previewMapInstructions;
    //private Image startingTitleImage;
    private Slider slider;
    private Interactable[] interactables;
    private int interactableProgress;
    private int sliderMin;
    private int sliderMax;
    private bool levelFlag;
    private Animator animator;
    private List<InteractableLogItem> backupLog;

    static private FastRecoveryUI instance = null;
    static private FastRecoveryUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<FastRecoveryUI>(true); return instance; }
    static public FastRecoveryUI Instance { get { return instance == null ? setInstance() : instance; } }

    //====================================================================================================//

    private void Awake()
    {
        SetRefs();
        buttonPrefab.gameObject.SetActive(false);
        ToggleInteractableGroup(false);
        slider.onValueChanged.AddListener(delegate { OnSlideValueChanged(); });

        Transform buttonGroup;
        if (settings.useRenderTexture)
        {
            map.gameObject.SetActive(false);
            GameObject tempCameraObject = new GameObject();
            tempCameraObject.name = "TempCamera";
            Camera tempCamera = tempCameraObject.AddComponent<Camera>();
            tempCamera.transform.localPosition = new Vector3(0, settings.renderCameraHeight, 0);
            tempCamera.transform.eulerAngles = new Vector3(90, 0, 0);
            RenderTexture target = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            tempCamera.targetTexture = target;
            tempCamera.enabled = false;
            tempCamera.Render();
            rawMap.texture = tempCamera.targetTexture;
            if (settings.restrictViewport)
            {
                rawMap.rectTransform.sizeDelta = new Vector2(980, 980);
                slider.minValue = 980;
                slider.value = slider.minValue;
            }

            else
                rawMap.rectTransform.sizeDelta = new Vector2(1000, 1000);
            buttonGroup = rawMap.transform.GetChild(0);
        }

        else
        {
            rawMap.gameObject.SetActive(false);
            map.sprite = settings.source;
            if (settings.restrictViewport)
            {
                if ((980 / settings.source.bounds.size.y * settings.source.bounds.size.x) < 880f)
                {
                    map.rectTransform.sizeDelta = new Vector2(880, 880 / settings.source.bounds.size.x * settings.source.bounds.size.y);
                    slider.minValue = 880 / settings.source.bounds.size.x * settings.source.bounds.size.y;
                }
                else
                {
                    map.rectTransform.sizeDelta = new Vector2(980 / settings.source.bounds.size.y * settings.source.bounds.size.x, 980);
                    slider.minValue = 980;
                }
                slider.value = slider.minValue;
            }

            else
                map.rectTransform.sizeDelta = new Vector2(1000 / settings.source.bounds.size.y * settings.source.bounds.size.x, 1000);
            buttonGroup = map.transform.GetChild(0);
        }

        foreach (Interactable interactable in interactables)
        {
            GameObject buttonObject = Instantiate(buttonPrefab);
            Button button = buttonObject.GetComponentInChildren<Button>();
            buttonObject.transform.SetParent(buttonGroup);
            if (settings.useInteractableUIPositions)
                buttonObject.transform.localPosition = new Vector3(interactable.LookAtPosition.x * settings.scale, interactable.LookAtPosition.z * settings.scale, 0);
            else
                buttonObject.transform.localPosition = new Vector3(interactable.transform.localPosition.x * settings.scale, interactable.transform.localPosition.z * settings.scale, 0);
            buttonObject.gameObject.SetActive(true);
            button.onClick.AddListener(() => OnButtonClick(interactable, button));
            SetHoverEvents(buttonObject);
            buttons.Add(button);
        }

        buttonGroup.localPosition = new Vector3(settings.xOffset, settings.yOffset, 0);
        buttonGroup.localEulerAngles = new Vector3(0, 0, settings.rotation);

        if (GameManager.Mode == GameMode.Rehearsal)
        {
            mainTitle.text = "REVIEW";
            instructions.text = "Let's review the objects you just selected so you can remember them.";
            previewMapInstructions.alignment = TMPro.TextAlignmentOptions.BaselineLeft;
            previewMapInstructions.fontSize = 16;
            previewMapInstructions.text = "Choose an orb from the map to the right.";
            previewMapInstructions.transform.localPosition = new Vector3(previewMapInstructions.transform.localPosition.x, 140, previewMapInstructions.transform.localPosition.z);
            back.gameObject.SetActive(false);
            if (settings.useInteractableUIPositions)
                pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.LookAtPosition.x * settings.scale, InteractablePath.NextInteractable.LookAtPosition.z * settings.scale, 0);
            else
                pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.transform.localPosition.x * settings.scale, InteractablePath.NextInteractable.transform.localPosition.z * settings.scale, 0);
            pin.transform.localEulerAngles = new Vector3(0, 0, -settings.rotation);
            pin.transform.position = new Vector3(pin.transform.position.x, pin.transform.position.y + 30, pin.transform.position.z);
            pin.transform.SetAsLastSibling();
        }

        else
        {
            pin.gameObject.SetActive(false);
            skip.gameObject.SetActive(false);
        }
    }

    //====================================================================================================//

    private void Update()
    {
        ListenForKeyDown();
        CheckForProgress();
        CheckForLevelChange();
        CheckForPreviewUI();
    }

    //====================================================================================================//

    private void ListenForKeyDown()
    {
        var input = Input.GetAxis("Mouse ScrollWheel") * 20;

        if (input > 0.0f)
        {
            if (slider.value + input > slider.maxValue)
            {
                slider.value = slider.maxValue;
            }
            else
            {
                slider.value += input;
            }

        }
        else if (input < 0.0f)
        {
            if (slider.value + input < slider.minValue)
            {
                slider.value = slider.minValue;
            }
            else
            {
                slider.value += input;
            }
        }
    }

    //====================================================================================================//

    private void SetRefs()
    {
        settings = LevelManager.FastRecoveryData;
        animator = gameObject.GetComponent<Animator>();
        buttons = new List<Button>();
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        overlay = images[0];
        map = images[2];
        if (settings.useRenderTexture)
            pin = images[4];
        else
            pin = images[3];
        rawMap = gameObject.GetComponentInChildren<RawImage>();
        //startingTitleImage = images[8];
        buttonPrefab = images[6].transform.parent.gameObject;
        preview = images[7];
        back = images[16];
        mapInstructions = images[23];
        skip = images[24];
        interactableGroup = gameObject.transform.GetChild(0).GetChild(0).GetChild(6).GetChild(2).gameObject;
        interactableButtons = interactableGroup.GetComponentsInChildren<Button>();
        TMPro.TMP_Text[] texts = gameObject.GetComponentsInChildren<TMPro.TMP_Text>();
        mainTitle = texts[0];
        instructions = texts[1];
        interactableTitle = texts[2];
        previewMapInstructions = texts[11];
        slider = gameObject.GetComponentInChildren<Slider>();
        interactableProgress = 0;
        List<Interactable> interactableList = new List<Interactable>();
        foreach (Interactable interactable in InteractableManager.InteractableList)
        {
            if (interactable.transform.parent.tag == "PreviewObject")
                continue;
            else
                interactableList.Add(interactable);
        }
        interactables = interactableList.ToArray();
    }

    //====================================================================================================//

    public void Toggle(bool active)
    {
        if (!active)
        {
            GameManager.State = GameState.Menu;
            animator.Play("SlideUp");
            ToggleInteractableGroup(false);

            for (int i = 0; i < 4; i++)
            {
                interactableButtons[i].onClick.RemoveAllListeners();
                interactableButtons[i].gameObject.GetComponent<Animation>().Stop();
                interactableButtons[i].gameObject.GetComponent<Image>().color = Color.white;
            }

            foreach (Button button in buttons)
            {
                button.gameObject.GetComponent<Animation>().Stop();
                button.gameObject.GetComponent<Image>().sprite = settings.interactableIcon;
                ColorBlock colors = button.gameObject.GetComponent<Button>().colors;
                colors.normalColor = Color.white;
                button.gameObject.GetComponent<Button>().colors = colors;
            }

            if (settings.useRenderTexture)
            {
                rawMap.transform.localPosition = new Vector3(0, 0, 0);
                if (settings.restrictViewport)
                {
                    rawMap.rectTransform.sizeDelta = new Vector2(980, 980);
                    slider.value = slider.minValue;
                }
            }
            else
            {
                map.transform.localPosition = new Vector3(0, 0, 0);
                if (settings.restrictViewport)
                {
                    if ((980 / settings.source.bounds.size.y * settings.source.bounds.size.x) < 880f)
                    {
                        map.rectTransform.sizeDelta = new Vector2(880, 880 / settings.source.bounds.size.x * settings.source.bounds.size.y);
                    }
                    else
                        map.rectTransform.sizeDelta = new Vector2(980 / settings.source.bounds.size.y * settings.source.bounds.size.x, 980);
                    slider.value = slider.minValue;
                }
                else
                {
                    map.rectTransform.sizeDelta = new Vector2(1000 / settings.source.bounds.size.y * settings.source.bounds.size.x, 1000);
                    slider.value = 1000;
                }
            }
            if (GameManager.GraduatedFlags[InteractableLog.CurrentLevelIndex] == true)
                pin.gameObject.SetActive(false);
            else
            {
                if (settings.useInteractableUIPositions)
                    pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.LookAtPosition.x * settings.scale, InteractablePath.NextInteractable.LookAtPosition.z * settings.scale, 0);
                else
                    pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.transform.localPosition.x * settings.scale, InteractablePath.NextInteractable.transform.localPosition.z * settings.scale, 0);
                pin.transform.localEulerAngles = new Vector3(0, 0, -settings.rotation);
                pin.transform.position = new Vector3(pin.transform.position.x, pin.transform.position.y + 30, pin.transform.position.z);
            }

            InteractablePreviewUI.ClearPreviewObject();
        }
        else
        {
            GameManager.State = GameState.Play;
            if (settings.useRenderTexture)
                EventSystem.current.SetSelectedGameObject(rawMap.gameObject);
            else
                EventSystem.current.SetSelectedGameObject(map.gameObject);

        }

        if (GameManager.Mode == GameMode.Recall && active && InteractablePreviewUI.Show)
            InteractablePreviewUI.ToggleShow();
        else if (GameManager.Mode == GameMode.Rehearsal && active && InteractableLog.Count % 3 != 0)
        {
            InteractablePreviewUI.SetPreviewObject(InteractablePath.NextInteractable, InteractablePath.Instance.actionIds[InteractablePath.Instance.nextIndex]);
            InteractablePreviewUI.SetPreviewAction(InteractablePath.Instance.actionIds[InteractablePath.Instance.nextIndex]);
        }
    }

    //====================================================================================================//

    public static void ToggleActive()
    {
        bool active = Instance.gameObject.activeSelf;
        Instance.gameObject.SetActive(!active);
        Instance.animator.Play("Default");
        Instance.Toggle(active);
    }

    //====================================================================================================//

    public void BackButtonOnClick()
    {
        ToggleActive();
    }

    //====================================================================================================//

    public void SkipButtonOnClick()
    {
        GameManager.ReviewMode = false;
        InteractableLog.Clear();
        foreach (InteractableLogItem interactable in backupLog)
        {
            InteractableLog.Add(interactable.siteIndex, interactable.interactableIndex, interactable.actionIndex);

        }
        InteractablePath.Instance.nextIndex = backupLog.Count;
        ToggleActive();
        LevelClearUI.ToggleOn();
    }


    //====================================================================================================//

    public void ToggleInteractableGroup(bool toggle)
    {
        if (!toggle)
        {
            interactableTitle.text = "Titl";//"Choose a";
            //startingTitleImage.gameObject.SetActive(!toggle);
            //interactableGroup.SetActive(toggle);
        }

        /*else
        {
            interactableGroup.SetActive(toggle);
            //startingTitleImage.gameObject.SetActive(!toggle);
        }*/

        preview.gameObject.SetActive(toggle);
        mapInstructions.gameObject.SetActive(!toggle);
        if (GameManager.Mode == GameMode.Rehearsal && GameManager.ReviewMode == true && toggle)
        {
            previewMapInstructions.text = "Select an option and keep the button pressed to perform an action.";
        }
        else if (GameManager.Mode == GameMode.Rehearsal && GameManager.ReviewMode == true && !toggle)
        {
            previewMapInstructions.text = "Choose an orb from the map to the right.";
        }
        else
            previewMapInstructions.gameObject.SetActive(!toggle);
        interactableTitle.gameObject.SetActive(toggle);
        interactableGroup.SetActive(toggle);
    }

    //====================================================================================================//

    public void OnButtonClick(Interactable interactable, Button button)
    {
        AudioManager.Play("UI_Hover");
        InteractableManager.SetActiveInteractable(interactable);

        foreach (Button interactableButton in buttons)
        {
            if (interactableButton != button)
            {
                interactableButton.gameObject.GetComponent<Image>().sprite = settings.interactableIcon;
            }
            else
            {
                interactableButton.gameObject.GetComponent<Image>().sprite = settings.interactableIconSelected;
            }
        }

        if (interactableTitle.text != interactable.Name)
        {
            interactable.ClickOnInteractable();
            interactableTitle.text = interactable.Name;
            ToggleInteractableGroup(true);
            InteractablePreviewUI.SetPreviewObject(interactable, 0);

            for (int i = 0; i < 4; i++)
            {
                int temp = i;
                interactableButtons[i].gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = interactable.stateData.getStateName(i);
                interactableButtons[i].gameObject.GetComponent<FastRecoveryButton>().Interactable = interactable;
                interactableButtons[i].gameObject.GetComponent<FastRecoveryButton>().ActionIndex = temp;



                if (GameManager.Mode == GameMode.Rehearsal && GameManager.GraduatedFlags[InteractableLog.CurrentLevelIndex] != true)
                {
                    if (InteractablePath.NextInteractable.ID == interactable.ID)
                    {
                        if (InteractablePath.NextAction == temp)
                        {
                            interactableButtons[i].gameObject.GetComponent<Animation>().Play();
                            /*ColorBlock colors = interactableButtons[i].colors;
                            colors.normalColor = new Color(189, 205, 217);
                            colors.highlightedColor = new Color(189, 205, 217);
                            interactableButtons[i].colors = colors;*/
                        }
                    }
                }
                interactableButtons[i].onClick.AddListener(() => OnInteractableButtonClick(temp));
            }
        }

        else
        {
            button.gameObject.GetComponent<Image>().sprite = settings.interactableIcon;
            InteractablePreviewUI.ClearPreviewObject();
            //startingTitleImage.gameObject.SetActive(false);
            ToggleInteractableGroup(false);

            for (int i = 0; i < 4; i++)
            {
                interactableButtons[i].onClick.RemoveAllListeners();
            }
        }
    }

    //====================================================================================================//

    public void OnInteractableButtonClick(int index)
    {
        AudioManager.Play("UI_Click");
        InteractablePreviewUI.SetPreviewAction(index);
    }

    //====================================================================================================//

    public void OnSlideValueChanged()
    {
        InteractablePreviewUI.ClearPreviewObject();
        //startingTitleImage.gameObject.SetActive(false);
        ToggleInteractableGroup(false);

        for (int i = 0; i < 4; i++)
        {
            interactableButtons[i].onClick.RemoveAllListeners();
        }

        float newScale;
        float newXOffset;
        float newYOffset;

        if (settings.restrictViewport)
        {
            newScale = (settings.scale * slider.value) / slider.minValue;
            newXOffset = (settings.xOffset * slider.value) / slider.minValue;
            newYOffset = (settings.yOffset * slider.value) / slider.minValue;
        }
        else
        {
            newScale = (settings.scale * slider.value) / 1000;
            newXOffset = (settings.xOffset * slider.value) / 1000;
            newYOffset = (settings.yOffset * slider.value) / 1000;
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.GetComponent<Image>().sprite = settings.interactableIcon;
            if (settings.useInteractableUIPositions)
            {
                buttons[i].transform.parent.localPosition = new Vector3(interactables[i].LookAtPosition.x * newScale, interactables[i].LookAtPosition.z * newScale, 0);
            }
            else
            {
                buttons[i].transform.parent.localPosition = new Vector3(interactables[i].transform.localPosition.x * newScale, interactables[i].transform.localPosition.z * newScale, 0);
            }
        }

        if (GameManager.Mode == GameMode.Rehearsal)
        {
            if (settings.useInteractableUIPositions)
                pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.LookAtPosition.x * newScale, InteractablePath.NextInteractable.LookAtPosition.z * newScale, 0);
            else
                pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.transform.localPosition.x * newScale, InteractablePath.NextInteractable.transform.localPosition.z * newScale, 0);
            pin.transform.position = new Vector3(pin.transform.position.x, pin.transform.position.y + 30, pin.transform.position.z);
        }

        if (settings.useRenderTexture)
        {
            rawMap.transform.localPosition = new Vector3(0, 0, 0);
            rawMap.rectTransform.sizeDelta = new Vector2(slider.value, slider.value);
            rawMap.transform.GetChild(0).localPosition = new Vector3(newXOffset, newYOffset, 0);
        }
        else
        {
            map.transform.localPosition = new Vector3(0, 0, 0);
            map.rectTransform.sizeDelta = new Vector2(slider.value / settings.source.bounds.size.y * settings.source.bounds.size.x, slider.value);
            map.transform.GetChild(0).localPosition = new Vector3(newXOffset, newYOffset, 0);
        }
    }

    //====================================================================================================//

    public void CheckForProgress()
    {
        if (InteractableLog.Count > interactableProgress)
        {
            interactableProgress = InteractableLog.Count;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].gameObject.GetComponent<Image>().sprite == settings.interactableIconSelected)
                {
                    buttons[i].gameObject.GetComponent<Animation>().Play();

                    if (GameManager.Mode == GameMode.Rehearsal)
                    {
                        buttons[i].gameObject.GetComponent<Image>().sprite = settings.interactableIcon;
                        ToggleInteractableGroup(false);
                    }
                }
            }

            if (GameManager.Mode == GameMode.Rehearsal && InteractableLog.Count % 3 != 0)
            {
                InteractablePreviewUI.ClearPreviewObject();
                float currentScale;
                if (settings.restrictViewport)
                    currentScale = (settings.scale * slider.value) / slider.minValue;
                else
                    currentScale = (settings.scale * slider.value) / 1000;

                if (settings.useInteractableUIPositions)
                    pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.LookAtPosition.x * currentScale, InteractablePath.NextInteractable.LookAtPosition.z * currentScale, 0);
                else
                    pin.transform.localPosition = new Vector3(InteractablePath.NextInteractable.transform.localPosition.x * currentScale, InteractablePath.NextInteractable.transform.localPosition.z * currentScale, 0);
                pin.transform.position = new Vector3(pin.transform.position.x, pin.transform.position.y + 30, pin.transform.position.z);

                for (int i = 0; i < 4; i++)
                {
                    interactableButtons[i].gameObject.GetComponent<Animation>().Stop();
                    interactableButtons[i].gameObject.GetComponent<Image>().color = Color.white;
                    /*ColorBlock colors = interactableButtons[i].colors;
                    colors.normalColor = Color.white;
                    colors.highlightedColor = Color.white;
                    interactableButtons[i].colors = colors;*/
                }
            }
        }

        else if (InteractableLog.Count < interactableProgress)
        {
            interactableProgress = InteractableLog.Count;
        }
    }

    //====================================================================================================//

    public void CheckForLevelChange()
    {
        if (InteractableLog.Count > 0 && InteractableLog.Count % 3 == 0 && levelFlag)
        {
            if (gameObject.activeSelf)
            {
                Toggle(true);
                gameObject.SetActive(false);
            }

            GameManager.ReviewMode = false;
            levelFlag = false;
        }

        else levelFlag |= InteractableLog.Count % 3 != 0;
    }

    //====================================================================================================//

    public void CheckForPreviewUI()
    {
        if (overlay.gameObject.activeSelf && !InteractablePreviewUI.Show)
        {
            InteractablePreviewUI.ToggleShow();
        }
    }

    //====================================================================================================//

    public void StartFastRehearsal()
    {
        backupLog = new List<InteractableLogItem>();
        foreach (InteractableLogItem item in InteractableLog.Log)
        {
            backupLog.Add(item);
        }
        InteractablePath.UndoLastAction();
        InteractablePath.UndoLastAction();
        InteractablePath.UndoLastAction();
        ToggleActive();
    }

    //====================================================================================================//

    private void OnHoverEnter(GameObject button)
    {
        Animator buttonAnimator = button.GetComponent<Animator>();
        buttonAnimator.Play("ButtonHoverEnter");
    }

    //====================================================================================================//

    private void OnHoverExit(GameObject button)
    {
        Animator buttonAnimator = button.GetComponent<Animator>();
        buttonAnimator.Play("ButtonHoverExit");
    }

    //====================================================================================================//

    private void SetHoverEvents(GameObject buttonObject)
    {
        EventTrigger trigger = buttonObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            buttonObject.gameObject.AddComponent<EventTrigger>();
            trigger = buttonObject.GetComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnHoverEnter(buttonObject); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnHoverExit(buttonObject); });
        trigger.triggers.Add(exit);
    }

    //====================================================================================================//
}


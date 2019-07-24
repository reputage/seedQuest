using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using SeedQuest.Interactables;

public class LevelIconButton : MonoBehaviour {
    static private int activeLevelCount = 6;
    static public int activeIndex = -1;
    static public LevelIconButton[] activeButtons = new LevelIconButton[activeLevelCount];
    static public LevelIconButton[] allIconButtons = new LevelIconButton[16];

    public int iconIndex;
    public string name;
    public string scenename;
    public Color backgroundColor;
    public Sprite iconImage;

    private bool isActive = false;
    private Image[] images;
    private Image icon;
    private Image border;
    private Image enableCover;
    private Image[] numberIcons = new Image[6];
    private Animator animator;

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();

        icon = images[2];
        border = images[3];
        enableCover = images[4];
        numberIcons[0] = images[5];
        numberIcons[1] = images[6];
        numberIcons[2] = images[7];
        numberIcons[3] = images[8];
        numberIcons[4] = images[9];
        numberIcons[5] = images[10];

        icon.sprite = iconImage;
        border.color = new Color(0, 0, 0, 0);
        foreach (Image icon in numberIcons) {
            icon.gameObject.SetActive(false);
            icon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        allIconButtons[iconIndex] = this;

        if (GameManager.Mode == GameMode.Rehearsal) {
            GetComponent<Button>().interactable = false;
            enableCover.gameObject.SetActive(true);
        }
        else {
            GetComponent<Button>().interactable = true;
            enableCover.gameObject.SetActive(false);
        }
    }

    private void Start() {
        SetupHoverEvents();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        images[1].color = backgroundColor;
    }

    public void onClickButton() {
        ActivateNumberIcon(this, iconIndex, true);
        EnableNextIconButton();

        if (GameManager.Mode == GameMode.Recall)
            MenuScreenManager.EnableUndoButton();
    }

    static public void Undo() {
        activeButtons[activeIndex].DeactivateIcon();

        if(GameManager.Mode == GameMode.Recall && activeIndex < 0) {
            MenuScreenManager.DisableUndoButton();
        }
    }

    public void DeactivateIcon() {
        ActivateNumberIcon(this, iconIndex, false);
    }

    private void SetupHoverEvents()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnHoverEnter(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnHoverExit(); });
        trigger.triggers.Add(exit);
    }

    private void OnHoverEnter() {
        if(GameManager.Mode == GameMode.Recall)
            animator.Play("UIButtonHover");

        MenuScreenManager.SetLevelPanel(activeIndex + 1, iconIndex);

        if (GetComponent<Button>().interactable == false)
            return;
    }

    private void OnHoverExit() {
        if (GameManager.Mode == GameMode.Recall)
            animator.Play("UIButtonIdle");

        MenuScreenManager.HideLevelPanel(activeIndex + 1);

        if (GetComponent<Button>().interactable == false)
            return;        
    }

    public void ActivateIconForRehersal(int iconOrderIndex) {
        numberIcons[iconOrderIndex].gameObject.SetActive(true);
        border.color = numberIcons[iconOrderIndex].color;
    }

    public static void EnableNextIconButton() {
        if (GameManager.Mode != GameMode.Rehearsal)
            return;

        int[] siteIDs = InteractablePathManager.GetPathSiteIDs();

        // Disable last scene button
        if (activeIndex >= 0) {
            int lastSiteID = siteIDs[activeIndex];
            allIconButtons[lastSiteID].GetComponent<Button>().interactable = false;
            allIconButtons[lastSiteID].enableCover.gameObject.SetActive(true);

            Animator prevAnimator = allIconButtons[lastSiteID].GetComponent<Animator>();
            prevAnimator.Play("UIButtonIdle");
        }

        // Enable next scene button
        if(activeIndex + 1 < siteIDs.Length) {
            int nextSiteID = siteIDs[activeIndex + 1];
            allIconButtons[nextSiteID].GetComponent<Button>().interactable = true;
            allIconButtons[nextSiteID].enableCover.gameObject.SetActive(false);       
            LevelIconButton.ActivateIconForRehersal(nextSiteID, activeIndex + 1);

            Animator nextAnimator = allIconButtons[nextSiteID].GetComponent<Animator>();
            nextAnimator.Play("UIButtonHighlight");
        }

    }

    public static void ActivateIconForRehersal(int siteID, int iconOrderIndex) {
        allIconButtons[siteID].ActivateIconForRehersal(iconOrderIndex);
    }

    public static void ActivateNumberIcon(LevelIconButton iconButton, int iconIndex, bool isActive) {
        if (activeIndex >= activeButtons.Length)
            return;

        if (isActive) {
            activeIndex++;
            string sound = "UI_SceneSelect" + (activeIndex + 1);
            AudioManager.Play(sound);
            activeButtons[activeIndex] = iconButton;
            iconButton.numberIcons[activeIndex].gameObject.SetActive(true);
            iconButton.border.color = iconButton.numberIcons[activeIndex].color;

            LevelSetManager.AddLevel(iconIndex);
            MenuScreenManager.SetLevelPanel(activeIndex, iconIndex);

            if (activeIndex == InteractableConfig.SitesPerGame - 1) {
                MenuScreenManager.SetEncodeSeedContinueCanvas();
            }
        }
        else {
            iconButton.border.color = new Color(0, 0, 0, 0);
            iconButton.numberIcons[activeIndex].gameObject.SetActive(false);
            activeIndex--;
        
            LevelSetManager.RemoveLevel();
        }
    }

    public static void ResetButtonIcons() {
        activeIndex = -1;
        activeButtons = new LevelIconButton[activeLevelCount];

        foreach(LevelIconButton icon in allIconButtons) {
            icon.border.color = new Color(0, 0, 0, 0);
            icon.numberIcons[0].gameObject.SetActive(false);
            icon.numberIcons[1].gameObject.SetActive(false);
            icon.numberIcons[2].gameObject.SetActive(false);
            icon.numberIcons[3].gameObject.SetActive(false);
            icon.numberIcons[4].gameObject.SetActive(false);
            icon.numberIcons[5].gameObject.SetActive(false);

        }
    }

    public static void ResetButtonStatus()
    {
        activeIndex = -1;
        activeButtons = new LevelIconButton[activeLevelCount];

        foreach (LevelIconButton icon in allIconButtons)
        {
            if (GameManager.Mode == GameMode.Recall)
            {
                icon.GetComponent<Button>().interactable = true;
                icon.enableCover.gameObject.SetActive(false);
            }

            else
            {
                icon.GetComponent<Button>().interactable = false;
                icon.enableCover.gameObject.SetActive(true);
            }
        }
    }

}

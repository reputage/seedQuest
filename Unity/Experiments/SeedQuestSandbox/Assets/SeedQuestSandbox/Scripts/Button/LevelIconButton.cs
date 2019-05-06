using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIconButton : MonoBehaviour {
    static public int activeIndex = -1;
    static public LevelIconButton[] activeButtons = new LevelIconButton[4];
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
    private Image[] numberIcons = new Image[4];

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();

        icon = images[2];
        border = images[3];
        numberIcons[0] = images[4];
        numberIcons[1] = images[5];
        numberIcons[2] = images[6];
        numberIcons[3] = images[7];

        icon.sprite = iconImage;
        border.color = new Color(0, 0, 0, 0);
        foreach (Image icon in numberIcons)
            icon.gameObject.SetActive(false);

        allIconButtons[iconIndex] = this;
    }

    private void Start() {
        
        if (Application.isEditor) {
            //print("We are running this from inside of the editor!");
        }
    }

    private void Update()
    {
        images[1].color = backgroundColor;
    }

    public void onClickButton() {
        ActivateNumberIcon(this, iconIndex, true);
    }

    public void ActivateIconForRehersal(int iconOrderIndex) {
        numberIcons[iconOrderIndex].gameObject.SetActive(true);
        border.color = numberIcons[iconOrderIndex].color;
    }

    public static void ActivateIconForRehersal(int siteID, int iconOrderIndex) {
        allIconButtons[siteID].ActivateIconForRehersal(iconOrderIndex);
    }

    public static void ActivateNumberIcon(LevelIconButton iconButton, int iconIndex, bool isActive) {
        if (activeIndex >= activeButtons.Length)
            return;

        if (isActive) {
            activeIndex++;
            activeButtons[activeIndex] = iconButton;
            iconButton.numberIcons[activeIndex].gameObject.SetActive(true);
            iconButton.border.color = iconButton.numberIcons[activeIndex].color;

            MenuScreenManager.SetLevelPanel(activeIndex, iconIndex);
        }
        else {
            iconButton.numberIcons[activeIndex].gameObject.SetActive(false);
            activeIndex--;
        }
    }

    public static void ResetButtonIcons() {
        activeIndex = -1;
        activeButtons = new LevelIconButton[4];

        foreach(LevelIconButton icon in allIconButtons) {
            icon.border.color = new Color(0, 0, 0, 0);
            icon.numberIcons[0].gameObject.SetActive(false);
            icon.numberIcons[1].gameObject.SetActive(false);
            icon.numberIcons[2].gameObject.SetActive(false);
            icon.numberIcons[3].gameObject.SetActive(false);
        }
    }
}

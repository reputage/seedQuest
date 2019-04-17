using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIconButton : MonoBehaviour {
    static public int activeIndex = -1;
    static public LevelIconButton[] activeButtons = new LevelIconButton[4];


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
    }

    private void Start() {
        
        if (Application.isEditor) {
            print("We are running this from inside of the editor!");
        }
    }

    private void Update()
    {
        images[1].color = backgroundColor;
    }

    public void onClickButton() {
        ActivateNumberIcon(this, true);
    }

    static void ActivateNumberIcon(LevelIconButton iconButton, bool isActive) {
        if (activeIndex >= activeButtons.Length)
            return;

        if (isActive) {
            activeIndex++;
            activeButtons[activeIndex] = iconButton;
            activeButtons[activeIndex].numberIcons[activeIndex].gameObject.SetActive(true);
            Color color = activeButtons[activeIndex].numberIcons[activeIndex].color;
            activeButtons[activeIndex].border.color = color;
        }
        else {
            activeButtons[activeIndex].numberIcons[activeIndex].gameObject.SetActive(false);
            activeIndex--;
        }
    }
}

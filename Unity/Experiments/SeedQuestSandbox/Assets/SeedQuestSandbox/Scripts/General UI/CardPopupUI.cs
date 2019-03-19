using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPopupUI : MonoBehaviour
{
    public Color32 headerColor = new Color32(0x21, 0x96, 0xf3, 0xff);
    public Color32 headerTextColor = new Color32(0xff, 0xff, 0xff, 0xff);
    public Color32 cardColor = new Color32(0xff, 0xff, 0xff, 0xff);
    public Color32 cardTextColor = new Color32(0x00, 0x00, 0x00, 0xff);
    public Color backdropColor = new Color(0, 0, 0, 0.4f);

    public string headerText = "Title";
    public string cardText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ";

    public bool useButtonOne = true;
    public string buttonOneText = "Accept";
    public bool useButtonTwo = true;
    public string buttonTwoText = "Cancel";

    private Image[] images;
    private TextMeshProUGUI[] texts;
    private Button[] buttons;

    public void Update() {
        UpdateCardInfo();
    }

    public void GetCardReferences() {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        buttons = GetComponentsInChildren<Button>();
    }

    public void UpdateCardInfo() {
        if (images == null)
            GetCardReferences();

        images[0].color = backdropColor;
        images[1].color = cardColor;
        images[2].color = headerColor;

        texts[0].text = headerText;
        texts[1].color = headerTextColor;
        texts[1].text = cardText;
        texts[1].color = cardTextColor;
        texts[2].text = buttonOneText;
        texts[3].text = buttonTwoText;

        buttons[1].gameObject.SetActive(useButtonOne);
        buttons[2].gameObject.SetActive(useButtonTwo);
    }

    public void toggleShow() {
        if(GameManager.Instance != null) {
            if (!gameObject.activeSelf)
                GameManager.State = GameState.Menu;
            else
                GameManager.State = GameManager.PrevState;    
        }

        if (!gameObject.activeSelf)
            UpdateCardInfo();

        gameObject.SetActive(!gameObject.activeSelf);
    }
}
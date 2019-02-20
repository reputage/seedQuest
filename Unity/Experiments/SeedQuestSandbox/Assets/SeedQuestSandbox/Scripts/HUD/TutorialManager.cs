using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    
    public GameObject body;
    public Image image;
    public Image arrow;
    public Button button;
    public TMP_Text headerText;
    public TMP_Text bodyText;
    public TutorialData data;

    private int card = 0;

    void Start ()
    {
        button.onClick.AddListener(onButtonClick);
        setCard();
    }

    private void setCard()
    {

        body.GetComponent<RectTransform>().sizeDelta = new Vector2(850, data.tutorialData[card].popupHeight);
        headerText.text = data.tutorialData[card].headerText;
        bodyText.text = data.tutorialData[card].bodyText;
        bodyText.GetComponent<RectTransform>().sizeDelta = new Vector2(590, data.tutorialData[card].bodyTextHeight);

        if (data.tutorialData[card].useArrow)
        {
            arrow.enabled = true;
            arrow.transform.position = data.tutorialData[card].arrowPosition;
            arrow.transform.eulerAngles = data.tutorialData[card].arrowRotation;
        }

        else
        {
            arrow.enabled = false;
        }

        if (data.tutorialData[card].useImage)
        {
            image.enabled = true;
            image.sprite = data.tutorialData[card].image;
            image.GetComponent<RectTransform>().sizeDelta = data.tutorialData[card].imageSize;
            image.transform.localPosition = data.tutorialData[card].imageLocalPosition;
        }

        else
        {
            image.enabled = false;
        }



        if (card == data.tutorialData.Count - 1)
        {
            button.transform.GetChild(0).GetComponent<TMP_Text>().text = "Finish";
        }
    }

    private void onButtonClick()
    {
        card++;
        if (card >= data.tutorialData.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        setCard();
    }
}

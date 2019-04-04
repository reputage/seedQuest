using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using SeedQuest.Interactables;

public class TutorialManager : MonoBehaviour {

    public GameObject body;
    public Image image;
    public Image arrow;
    public Button button;
    public Button skip;
    public TMP_Text headerText;
    public TMP_Text bodyText;
    public TMP_Text bodyText2;
    public TutorialData data;

    private int card = 0;

    void Start () {
        if (GameManager.Mode == GameMode.Rehearsal && TutorialState.Skip == false && InteractablePathManager.LevelsComplete == 0) {
            GameManager.State = GameState.Menu;
            button.onClick.AddListener(onButtonClick);
            skip.onClick.AddListener(onSkipClick);
            setCard();
        }
        else {
            gameObject.SetActive(false);
        }
    }

    private void setCard() {
        body.GetComponent<RectTransform>().sizeDelta = new Vector2(850, data.tutorialData[card].popupHeight);
        headerText.text = data.tutorialData[card].headerText;
        bodyText.text = data.tutorialData[card].bodyText;
        bodyText.GetComponent<RectTransform>().sizeDelta = new Vector2(590, data.tutorialData[card].bodyTextHeight);
        bodyText.transform.localPosition = new Vector3(bodyText.transform.localPosition.x, data.tutorialData[card].bodyTextYOffset, 0);

        if (data.tutorialData[card].useSecondBodyText) {
            bodyText2.enabled = true;
            bodyText2.text = data.tutorialData[card].secondBodyText;
            bodyText2.GetComponent<RectTransform>().sizeDelta = new Vector2(590, data.tutorialData[card].secondBodyTextHeight);
            bodyText2.transform.localPosition = new Vector3(bodyText2.transform.localPosition.x, data.tutorialData[card].secondBodyTextYOffset, 0);

        }
        else {
            bodyText2.enabled = false;
        }

        if (data.tutorialData[card].useArrow) {
            arrow.enabled = true;
            arrow.transform.localPosition = data.tutorialData[card].arrowPosition;
            arrow.transform.eulerAngles = data.tutorialData[card].arrowRotation;
        }
        else {
            arrow.enabled = false;
        }

        if (data.tutorialData[card].useImage) {
            image.enabled = true;
            image.sprite = data.tutorialData[card].image;
            image.GetComponent<RectTransform>().sizeDelta = data.tutorialData[card].imageSize;
            image.transform.localPosition = data.tutorialData[card].imageLocalPosition;
        }  
        else {
            image.enabled = false;
        }

        if (card == data.tutorialData.Count - 1) {
            button.transform.GetChild(0).GetComponent<TMP_Text>().text = "Finish";
        }
    }

    private void onSkipClick() {
        gameObject.SetActive(false);
        GameManager.State = GameState.Play;
        TutorialState.Skip = true;
    }

    private void onButtonClick() {
        card++;
        if (card >= data.tutorialData.Count) {
            gameObject.SetActive(false);
            GameManager.State = GameState.Play;
            return;
        }

        setCard();
    }
}

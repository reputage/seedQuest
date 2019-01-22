using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;

public class SurveyManager : MonoBehaviour
{
    public SurveyData data;
    public GameObject cardContainer;
    public GameObject dotContainer;
    public GameObject CardTemplate;
    public Button PreviousButton;
    public Button NextButton;

    public Sprite ring;
    public Sprite dot;

    private int xOffset = 0;
    private int imageXOffset = -84;
    private float cardContainerSize = 0;
    private int currentCardIndex = 0;

    private List<GameObject> dots = new List<GameObject>();

    private void Start()
    {
        int surveyQuestions = data.surveyData.Count;
        var cardContainerTransform = cardContainer.transform as RectTransform;
        cardContainerSize = surveyQuestions * 687.5f;
        cardContainerTransform.sizeDelta = new Vector2(cardContainerSize, cardContainerTransform.sizeDelta.y);

        var dotContainerTransform = dotContainer.transform as RectTransform;
        float dotContainerSize = 40 * surveyQuestions - 8;
        dotContainerTransform.sizeDelta = new Vector2(dotContainerSize, dotContainerTransform.sizeDelta.y);
        for(int i = 0; i < surveyQuestions; i++)
        {
            var newCard = Instantiate(CardTemplate);
            newCard.transform.parent = cardContainer.transform;
            newCard.transform.localPosition = new Vector3(xOffset, 0, 0);
            Debug.Log(xOffset);
            xOffset += 750;
            Text text = newCard.transform.GetChild(1).GetChild(1).GetComponent<Text>();
            text.text = data.surveyData[i].question;

            text = newCard.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            text.text = (i + 1).ToString() + "/" + surveyQuestions;

            GameObject newDot = new GameObject();
            RectTransform dotTransform = newDot.AddComponent<RectTransform>();
            dotTransform.sizeDelta = new Vector2(32, 32);
            dotTransform.anchorMin = new Vector2(0, 0.5f);
            dotTransform.anchorMax = new Vector2(0, 0.5f);
            Image newImage = newDot.AddComponent<Image>();
            if (i == currentCardIndex)
            {
                newImage.sprite = dot;
            }
            else
            {
                newImage.sprite = ring;
            }
            newDot.transform.parent = dotContainer.transform;
            newDot.transform.localPosition = new Vector3(imageXOffset, 0, 0);
            imageXOffset += 40;
            dots.Add(newDot);
        }

        PreviousButton.onClick.AddListener(onClickPrevious);
        NextButton.onClick.AddListener(onClickNext);
    }

    public void onClickPrevious()
    {
        if (currentCardIndex > 0)
        {
            Vector3 destination = cardContainer.transform.position + new Vector3(750, 0, 0);
            StartCoroutine(MoveOverSeconds(cardContainer, destination, 1));

            dots[currentCardIndex].GetComponent<Image>().sprite = ring;
            currentCardIndex -= 1;
            dots[currentCardIndex].GetComponent<Image>().sprite = dot;
        }
    }

    public void onClickNext()
    {
        if (currentCardIndex < data.surveyData.Count-1)
        {
            Vector3 destination = cardContainer.transform.position - new Vector3(750, 0, 0);
            StartCoroutine(MoveOverSeconds(cardContainer, destination, 1));


            dots[currentCardIndex].GetComponent<Image>().sprite = ring;
            currentCardIndex += 1;
            dots[currentCardIndex].GetComponent<Image>().sprite = dot;
        }
    }

    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }
}
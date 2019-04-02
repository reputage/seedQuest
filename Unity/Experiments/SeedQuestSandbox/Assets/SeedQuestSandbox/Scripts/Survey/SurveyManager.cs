using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MaterialUI;
using TMPro;

public class SurveyManager : MonoBehaviour
{
    public SurveyData data;
    public GameObject cardContainer;
    public GameObject dotContainer;
    public GameObject cardTemplateOpen;
    public GameObject cardTemplateScale;
    public GameObject cardTemplateSubmit;
    //public GameObject CardTemplateRank;
    public GameObject cardWarningPopup;
    public GameObject warningSubmit;

    public Button PreviousButton;
    public Button NextButton;
    private Button SubmitButton;
    private TextMeshProUGUI SubmitInfoText;

    public Sprite ring;
    public Sprite dot;

    private string serverUrl = "http://178.128.0.208:8000/surveys";
    private int xOffset = 0;
    private int imageXOffset = -34;
    private float cardContainerSize = 0;
    private int currentCardIndex = 0;
    private bool sentDataOnce = false;

    private List<GameObject> dots = new List<GameObject>();

    private void Start()
    {
        Button[] buttons = cardWarningPopup.GetComponentsInChildren<Button>();
        warningSubmit = buttons[0].gameObject;
        buttons[0].onClick.AddListener(sendSurveyData);
        buttons[1].onClick.AddListener(deactivateWarning);

        int surveyQuestions = data.surveyData.Count;
        var cardContainerTransform = cardContainer.transform as RectTransform;
        //cardContainerSize = surveyQuestions * 4000f;
        //cardContainerTransform.sizeDelta = new Vector2(cardContainerSize, cardContainerTransform.sizeDelta.y);

        ColorBlock cb = PreviousButton.colors;
        cb.disabledColor = new Color(0, 0, 0, 0);
        cb = NextButton.colors;
        cb.disabledColor = new Color(0, 0, 0, 0);

        var dotContainerTransform = dotContainer.transform as RectTransform;
        //float dotContainerSize = 40 * surveyQuestions - 8;
        //dotContainerTransform.sizeDelta = new Vector2(dotContainerSize, dotContainerTransform.sizeDelta.y);
        for (int i = 0; i < surveyQuestions; i++)
        {
            if (data.surveyData[i].type == SurveyDataItem.QuestionType.Open)
            {

                var newCard = Instantiate(cardTemplateOpen);
                newCard.SetActive(true);
                newCard.transform.parent = cardContainer.transform;
                newCard.transform.localPosition = new Vector3(xOffset, 0, 0);
                xOffset += 4000;
                TMP_Text text = newCard.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
                text.text = data.surveyData[i].question;

                TMP_InputField input = newCard.transform.GetChild(1).GetChild(2).GetComponent<TMP_InputField>();
                int tempI = i;
                input.onValueChanged.AddListener(delegate
                {
                    data.surveyData[tempI].answer = input.text;
                });
                text = newCard.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                text.text = (i + 1).ToString() + "/" + surveyQuestions;
            }

            else
            {
                var newCard = Instantiate(cardTemplateScale);
                newCard.SetActive(true);
                newCard.transform.parent = cardContainer.transform;
                newCard.transform.localPosition = new Vector3(xOffset, 0, 0);
                xOffset += 4000;
                TMP_Text text = newCard.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
                text.text = data.surveyData[i].question;

                data.surveyData[i].answers = new string[data.surveyData[i].questions.Length];

                text = newCard.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                text.text = (i + 1).ToString() + "/" + surveyQuestions;

                var column = newCard.transform.GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                var row = newCard.transform.GetChild(1).GetChild(4).GetChild(1).GetChild(0).gameObject;
                var thumbnail = newCard.transform.GetChild(1).GetChild(4).GetChild(2).GetChild(0).gameObject;
                List<GameObject> rows = new List<GameObject>();
                List<TMP_Text> columns = new List<TMP_Text>();
                List<GameObject> thumbnails = new List<GameObject>();
                float columnXOffset = -112f;
                float columnWidth = 436f / (float)data.surveyData[i].headers.Length;
                float rowYOffset = 28f;
                float rowHeight = 245f / (float)data.surveyData[i].questions.Length;

                foreach (string header in data.surveyData[i].headers)
                {
                    TMP_Text newColumn = Instantiate(column);
                    newColumn.transform.parent = column.transform.parent;
                    newColumn.transform.localPosition = new Vector3(columnXOffset, 66, 0);
                    newColumn.transform.localScale = new Vector3(1, 1, 1);
                    newColumn.GetComponent<RectTransform>().sizeDelta = new Vector2(columnWidth, 32);
                    newColumn.text = header;
                    columns.Add(newColumn);

                    columnXOffset += columnWidth;
                }

                Destroy(column.gameObject);

                foreach (string question in data.surveyData[i].questions)
                {
                    GameObject newRow = Instantiate(row);
                    newRow.transform.parent = row.transform.parent;
                    newRow.transform.localPosition = new Vector3(0, 0, 0);
                    newRow.transform.localScale = new Vector3(1, 1, 1);
                    rows.Add(newRow);



                    GameObject newThumbnail = Instantiate(thumbnail);
                    newThumbnail.transform.parent = thumbnail.transform.parent;
                    newThumbnail.transform.localScale = new Vector3(1, 1, 1);
                    thumbnails.Add(newThumbnail);
                }

                // I tried to include this in the above loop, but position data wasn't being saved for some reason.
                for (int j = 0; j < rows.Count; j++)
                {
                    TMP_Text question = rows[j].transform.GetChild(0).GetComponent<TMP_Text>();
                    question.transform.localPosition = new Vector3(-218, rowYOffset, 0);
                    question.transform.localScale = new Vector3(1, 1, 1);
                    question.GetComponent<RectTransform>().sizeDelta = new Vector2(136, rowHeight);
                    question.text = data.surveyData[i].questions[j];
                    question.name = j.ToString();


                    thumbnails[j].transform.localPosition = new Vector3(thumbnail.transform.localPosition.x, rowYOffset, 0);
                    Image image = thumbnails[j].transform.GetChild(2).gameObject.GetComponent<Image>();

                    try
                    {
                        image.sprite = data.surveyData[i].sprites[j];
                    }
                    catch (System.IndexOutOfRangeException e)
                    {
                        Debug.LogError("Questions and sprites are not equal.");
                    }

                    for (int k = 0; k < columns.Count; k++)
                    {
                        Toggle newToggle = Instantiate(rows[j].transform.GetChild(1).GetChild(0).GetComponent<Toggle>());
                        newToggle.transform.parent = rows[j].transform.GetChild(1).GetChild(0).parent;
                        newToggle.transform.localPosition = new Vector3(columns[k].transform.localPosition.x, rowYOffset, 0);
                        newToggle.transform.localScale = new Vector3(1, 1, 1);
                        newToggle.name = columns[k].text;
                        int tempI = i;
                        int tempJ = j;
                        newToggle.onValueChanged.AddListener(delegate {
                            if (newToggle.isOn)
                            {
                                data.surveyData[tempI].answers[tempJ] = newToggle.name;
                            }
                        });
                        newToggle.group = rows[j].transform.GetChild(1).gameObject.GetComponent<ToggleGroup>();
                    }
                    Destroy(rows[j].transform.GetChild(1).GetChild(0).gameObject);
                    rows[j].transform.GetChild(1).gameObject.GetComponent<ToggleGroup>().SetAllTogglesOff();
                    rowYOffset -= rowHeight;

                }

                Destroy(thumbnail);
                Destroy(row);
            }

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

        GameObject submitDot = new GameObject();
        RectTransform submitDotTransform = submitDot.AddComponent<RectTransform>();
        submitDotTransform.sizeDelta = new Vector2(32, 32);
        submitDotTransform.anchorMin = new Vector2(0, 0.5f);
        submitDotTransform.anchorMax = new Vector2(0, 0.5f);
        Image submitDotImage = submitDot.AddComponent<Image>();
        submitDotImage.sprite = ring;
        submitDot.transform.parent = dotContainer.transform;
        submitDot.transform.localPosition = new Vector3(imageXOffset, 0, 0);
        dots.Add(submitDot);

        var submitCard = Instantiate(cardTemplateSubmit);
        submitCard.SetActive(true);
        submitCard.transform.parent = cardContainer.transform;
        submitCard.transform.localPosition = new Vector3(xOffset, 0, 0);
        SubmitButton = submitCard.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>();
        SubmitInfoText = submitCard.GetComponentInChildren<TextMeshProUGUI>();

        // Add Button Listeners 
        SubmitButton.onClick.AddListener( delegate { submitButtonFunc(); } );
        PreviousButton.onClick.AddListener(onClickPrevious);
        NextButton.onClick.AddListener(onClickNext);
    } 

    void OnApplicationQuit()
    {
        //StartCoroutine("sendSurveyData");
    }

    public void onClickPrevious()
    {
        if (currentCardIndex > 0)
        {
            Vector3 destination = cardContainer.transform.position + new Vector3(4000, 0, 0);
            StartCoroutine(MoveOverSeconds(cardContainer, destination, 1));

            dots[currentCardIndex].GetComponent<Image>().sprite = ring;
            currentCardIndex -= 1;
            dots[currentCardIndex].GetComponent<Image>().sprite = dot;
        }
    }

    public void onClickNext()
    {
        if (currentCardIndex < data.surveyData.Count)
        {
            Vector3 destination = cardContainer.transform.position - new Vector3(4000, 0, 0);
            StartCoroutine(MoveOverSeconds(cardContainer, destination, 1));


            dots[currentCardIndex].GetComponent<Image>().sprite = ring;
            currentCardIndex += 1;
            dots[currentCardIndex].GetComponent<Image>().sprite = dot;
        }
    }

    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        PreviousButton.enabled = false;
        NextButton.enabled = false;
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        PreviousButton.enabled = true;
        NextButton.enabled = true;
    }


    public void submitButtonFunc() {
        if (sentDataOnce) {
            SceneManager.LoadScene("PrototypeSelect");
            return;
        }

        List<string> questions = getQuestionsFromSurvey(data);
        List<string> responses = getAnswersFromSurvey(data);
        Text[] texts = cardWarningPopup.GetComponentsInChildren<Text>();
        Button[] buttons = cardWarningPopup.GetComponentsInChildren<Button>();

        int counter = 0;

        foreach (string response in responses)
        {
            string trimmed = response;
            if (trimmed != null)
                trimmed = Regex.Replace(trimmed, "\\s+", "");

            if (trimmed == "" || trimmed == null)
            {
                counter += 1;
            }
            else
            {
                Debug.Log("Question with a non-empty non-null response: " + response + " t " + trimmed);
            }
        }

        if (counter >= questions.Count)
        {
            Debug.Log("No responses found.");
            texts[0].text = "You have not answered any questions in the survey. Please answer at least one question.";
            cardWarningPopup.SetActive(true);
            warningSubmit.SetActive(false);
        }
        else if (counter > 0)
        {
            int dis = questions.Count - counter;
            Debug.Log("Some questions have not been answered." + dis);
            texts[0].text = "Some questions have not been answered. Submit survey responses anyways?";
            cardWarningPopup.SetActive(true);
            warningSubmit.SetActive(true);
        }
        else
        {
            Debug.Log("All responses recorded! Sending data...");
            sendSurveyData();
        }

    }

    public void deactivateWarning()
    {
        Debug.Log("Deactivating warning popup");
        cardWarningPopup.SetActive(false);
    }

    // Send the survey data to the server
    public void sendSurveyData()
    {
        cardWarningPopup.SetActive(false);
        SubmitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Main Menu";
        SubmitInfoText.text = "Thanks for your feedback!";

        if (sentDataOnce == false)
        {
            List<string> questions = getQuestionsFromSurvey(data);
            List<string> responses = getAnswersFromSurvey(data);

            if (responses.Count < questions.Count)
            {
                for (int i = responses.Count - 1; i < questions.Count - 1; i++)
                {
                    responses.Add(".");
                }
            }

            sentDataOnce = true;
            StartCoroutine(sqSurveyInterface.postRequest(questions, responses, serverUrl));
        }
        else {
            SceneManager.LoadScene("PrototypeSelect");
        }
    }

    // Get the questions from the scriptable object
    public List<string> getQuestionsFromSurvey(SurveyData data)
    {
        List<string> questions = new List<string>();
        for (int i = 0; i < data.surveyData.Count; i++)
        {
            if (data.surveyData[i].questions.Length > 0)
            {
                // add all questions
                for (int j = 0; j < data.surveyData[i].questions.Length; j++)
                {
                    string comboQuestion = data.surveyData[i].question + "-" + data.surveyData[i].questions[j];
                    questions.Add(comboQuestion);
                }
            }
            else
            {
                questions.Add(data.surveyData[i].question);
            }
        }
        return questions;
    }

    // to do - front end is still in progress?
    public List<string> getAnswersFromSurvey(SurveyData data)
    {
        List<string> responses = new List<string>();

        for (int i = 0; i < data.surveyData.Count; i++)
        {
            if (data.surveyData[i].answers.Length > 0)
            {
                // add all questions
                for (int j = 0; j < data.surveyData[i].answers.Length; j++)
                {
                    string answer = data.surveyData[i].answers[j];
                    responses.Add(answer);
                }
            }
            else
            {
                responses.Add(data.surveyData[i].answer);
            }
        }
        return responses;
    }
}
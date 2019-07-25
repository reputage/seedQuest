using System.Collections;
using System.Collections.Generic;
using SeedQuest.Interactables;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScenePauseMenu : MonoBehaviour
{
    private Button returnButton;
    private Button replayButton;
    private Button exitButton;
    private Button quitButton;
    private Button promptCancel;
    private Button promptExit;

    private Text interactable1;
    private Text interactable2;
    private Text interactable3;
    private Text interactable4;

    private Text state1;
    private Text state2;
    private Text state3;
    private Text state4;

    private Text promptTitle;
    private Text promptBody;
    private Text promptExitButtonText;

    private Image img1;
    private Image img2;
    private Image img3;
    private Image img4;

    public GameObject promptPopup;
    public GameObject container;

    static private ScenePauseMenu instance = null;
    static private ScenePauseMenu setInstance() { instance = HUDManager.Instance.GetComponentInChildren<ScenePauseMenu>(true); return instance; }
    static public ScenePauseMenu Instance { get { return instance == null ? setInstance() : instance; } }

    void Start()
    {
        getReferences();
        delegateButtons();
        setPreview();
    }

    // Toggle the menu on
    static public void ToggleOn()
    {
        if (Instance.gameObject.activeSelf)
            return;

        Instance.gameObject.SetActive(true);
        GameManager.State = GameState.Menu;
    }

    // Get the references to all the necessary components
    private void getReferences()
    {
        promptPopup.SetActive(true);

        Button[] buttons = GetComponentsInChildren<Button>(true);
        Text[] texts = GetComponentsInChildren<Text>(true);
        Image[] images = GetComponentsInChildren<Image>(true);

        returnButton = buttons[0];
        replayButton = buttons[1];
        exitButton = buttons[2];
        quitButton = buttons[3];
        promptCancel = buttons[4];
        promptExit = buttons[5];

        interactable1 = texts[5];
        interactable2 = texts[7];
        interactable3 = texts[9];
        interactable4 = texts[11];

        state1 = texts[6];
        state2 = texts[8];
        state3 = texts[10];
        state4 = texts[12];

        img1 = images[6];
        img2 = images[7];
        img3 = images[8];
        img4 = images[9];

        promptTitle = texts[13];
        promptBody = texts[14];
        promptExitButtonText = texts[16];

        promptPopup.SetActive(false);
    }

    // Set the functions used by each button
    public void delegateButtons()
    {
        returnButton.onClick.AddListener(returnToGame);
        replayButton.onClick.AddListener(replayScene);
        exitButton.onClick.AddListener(setPromptForMenu);
        quitButton.onClick.AddListener(setPromptForQuit);

        promptPopup.SetActive(true);
        promptCancel.onClick.AddListener(deactivatePrompt);
        promptExit.onClick.AddListener(quitGame);
        promptPopup.SetActive(false);
    }

    // Set up the interactable previews for the scene
    public void setPreview()
    {
        // not finished
        //setInteractableTexts();
        //setInteractableImages();
    }

    // Set the images for the interactable action previews
    public void setInteractableImages(Image im1, Image im2, Image im3, Image im4)
    {
        img1 = im1;
        img2 = im2;
        img3 = im3;
        img4 = im4;
    }

    // Set the text for the interactable action previews
    public void setInteractableTexts(string inter1, string inter2, string inter3, string inter4, 
                                    string st1, string st2, string st3, string st4)
    {
        interactable1.text = inter1;
        interactable2.text = inter2;
        interactable3.text = inter3;
        interactable4.text = inter4;

        state1.text = st1;
        state2.text = st2;
        state3.text = st3;
        state4.text = st4;
    }

    public void deactivatePrompt()
    {
        promptPopup.SetActive(false);
    }

    public void returnToGame()
    {
        GameManager.State = GameManager.PrevState;
        gameObject.SetActive(false);
    }

    public void replayScene()
    {
        // remove any actions performed in the current scene from the interactable log
        // calculate # of actions to undo
        int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
        //Debug.Log("Found " + actionsThisScene + " actions to undo");

        if (actionsThisScene > 0)
        {
            for (int i = 0; i < actionsThisScene; i++)
            {
                InteractablePathManager.UndoLastAction();
                Debug.Log("Undo number " + (i + 1));
            }

            InteractablePathManager.ShowLevelComplete = false;
        }

        GameManager.State = GameManager.PrevState;
        gameObject.SetActive(false);
    }

    public void exitToMenu() {
        SeedQuest.Level.LevelManager.Instance.StopLevelMusic();
        MenuScreenManager.ActivateStart();
        gameObject.SetActive(false);
        GameManager.GraduatedMode = false;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    private void changeTextToWhite(Button button)
    {
        Color white = new Color(1, 1, 1, 1);
        changeButtonTextColor(button, white);
    }

    private void changeTextToBlack(Button button)
    {
        Color black = new Color(0, 0, 0, 1);
        changeButtonTextColor(button, black);
    }

    private void changeButtonTextColor(Button button, Color color)
    {
        button.GetComponentInChildren<Text>().color = color;
    }

    private void setPromptForQuit()
    {
        promptPopup.SetActive(true);
        promptTitle.text = "Exit Game";
        promptBody.text = "Are you sure you want to quit the game? You will lose progress on your Seedquest.";
        promptExitButtonText.text = "Exit Game";

        promptExit.onClick.RemoveAllListeners();
        promptExit.onClick.AddListener(quitGame);
    }

    private void setPromptForMenu()
    {
        promptPopup.SetActive(true);
        promptTitle.text = "Exit to Menu";
        promptBody.text = "Are you sure you want to exit to the Main Menu? You will lose progress on your Seedquest.";
        promptExitButtonText.text = "Exit to Menu";

        promptExit.onClick.RemoveAllListeners();
        promptExit.onClick.AddListener(exitToMenu);
    }

}
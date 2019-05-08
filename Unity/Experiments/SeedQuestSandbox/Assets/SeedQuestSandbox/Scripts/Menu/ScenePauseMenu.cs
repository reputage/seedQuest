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
    }

    static public void ToggleOn()
    {
        if (Instance.gameObject.activeSelf)
            return;

        Instance.gameObject.SetActive(true);
        GameManager.State = GameState.Menu;
    }

    private void getReferences()
    {
        promptPopup.SetActive(true);

        Button[] buttons = GetComponentsInChildren<Button>();
        Text[] texts = GetComponentsInChildren<Text>();
        Image[] images = GetComponentsInChildren<Image>();

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

    public void setInteractableImages(Image im1, Image im2, Image im3, Image im4)
    {
        img1 = im1;
        img2 = im2;
        img3 = im3;
        img4 = im4;
    }

    public void deactivatePrompt()
    {
        promptPopup.SetActive(false);
    }

    public void returnToGame()
    {
        gameObject.SetActive(false);
    }

    public void replayScene()
    {
        // remove any actions performed in the current scene from the interactable log
        // calculate # of actions to undo
        int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
        if (actionsThisScene > 0)
        {
            // undo actions in interactable log
            // undo actions in interactable path
            // if actionsThisScene == 0, then no actions have been performed in this scene
            // should prevent this menu from appearing if the scene has been completed
        }
        // reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exitToMenu()
    {
        // remove all progress from the interactable log
        // go to start scene
        SceneManager.LoadScene("_StartMenu");
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

    private void changeButtonTextColor(Button button, Color color)
    {
        button.GetComponentInChildren<Text>().color = color;
    }

}

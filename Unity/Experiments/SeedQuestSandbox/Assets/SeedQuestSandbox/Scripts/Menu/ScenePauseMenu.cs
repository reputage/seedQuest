using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenePauseMenu : MonoBehaviour
{
    private Button returnButton;
    private Button replayButton;
    private Button exitButton;
    private Button quitButton; // not included in the mockup, using this as a placeholder

    private Text interactable1;
    private Text interactable2;
    private Text interactable3;
    private Text interactable4;

    private Text state1;
    private Text state2;
    private Text state3;
    private Text state4;

    public GameObject promptPopup;

    static private ScenePauseMenu instance = null;
    static private ScenePauseMenu setInstance() { instance = HUDManager.Instance.GetComponentInChildren<ScenePauseMenu>(true); return instance; }
    static public ScenePauseMenu Instance { get { return instance == null ? setInstance() : instance; } }


    void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        Text[] texts = GetComponentsInChildren<Text>();
        returnButton = buttons[0];
        replayButton = buttons[1];
        exitButton = buttons[2];
        quitButton = buttons[3];

        /*
        Debug.Log("Number of texts: " + texts.Length);
        for (int i = 0; i < texts.Length; i++) 
        {
            Debug.Log(i + " " + texts[i]);
        }
        */

        interactable1 = texts[5];
        interactable2 = texts[7];
        interactable3 = texts[9];
        interactable4 = texts[11];

        state1 = texts[6];
        state2 = texts[8];
        state3 = texts[10];
        state4 = texts[12];



    }


    void Update()
    {
        
    }

    public void returnToGame()
    {
        // 
    }

    public void replayScene()
    {
        // remove any actions performed in the current scene
        // reload the current scene
    }

    public void exitToMenu()
    {
        // go to start scene
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


}

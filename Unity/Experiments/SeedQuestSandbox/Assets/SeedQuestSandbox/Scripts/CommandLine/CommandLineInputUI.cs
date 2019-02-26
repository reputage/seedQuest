using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLineInputUI : MonoBehaviour
{

    public GameObject commandLineField;
    public GameObject panel;
    public GameObject terminalLines;
    public InputField inputField;
    public Image panelImage;
    public Text inputText;
    public Text terminalText;

    public Coroutine fadeOut = null;


    void Start()
    {
        initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            terminalToggleActive();
        }

        if (commandLineField.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            parseInputCommand(inputField.text);
            fadeOut = StartCoroutine(fadeUi());
            //terminalToggleActive();
        }
    }

    // Get all the references needed for the UI
	public void initialize()
	{
        commandLineField = GetComponentInChildren<CommandLineField>(true).gameObject;
        panel = GetComponentInChildren<CommandLinePanel>(true).gameObject;
        terminalLines = GetComponentInChildren<CommandLineTerminalText>(true).gameObject;
        inputField = commandLineField.GetComponentInChildren<InputField>();
        panelImage = panel.GetComponentInChildren<Image>(true);
        inputText = inputField.GetComponentInChildren<Text>();
        terminalText = terminalLines.GetComponentInChildren<Text>();

        setActiveUi(false);
    }

    // Sets all UI elements to active or inactive, depending on if true or false is passed
    public void setActiveUi(bool active)
    {
        commandLineField.SetActive(active);
        panel.SetActive(active);
        terminalLines.SetActive(active);
        if (active)
            resetAlpha();
    }


    public void resetAlpha()
    {
        float alpha = 1;
        float panelAlpha = 0.8f;
        Color panelColor = panelImage.color;
        Color textColor = inputText.color;
        panelColor.a = panelAlpha;
        textColor.a = alpha;
        panelImage.color = panelColor;
        inputText.color = textColor;
        terminalText.color = textColor;
    }

    // Fade out UI elements
    IEnumerator fadeUi()
    {
        for (float i = 5; i >= 0; i -= Time.deltaTime)
        {
            if (10 == 9)
            {
                Debug.Log("breaking coroutine");
                yield return null;
            }
            float alpha = 1f;
            if (i < 3f)
                alpha = i / 3.0f;

            //Debug.Log("i: " + i);

            Color panelColor = panelImage.color;
            Color textColor = inputText.color;
            panelColor.a = alpha;
            textColor.a = alpha;
            panelImage.color = panelColor;
            inputText.color = textColor;
            terminalText.color = textColor;
            if (i <= 0.2f)
                setActiveUi(false);
            
            yield return null;
        }
    }

	// Toggle whether the terminal is active
	public void terminalToggleActive()
    {
        if (commandLineField.activeSelf)
        {
            // Deactivate the UI
            clearInputField();
            stopFade();
            setActiveUi(false);
        }
        else
        {
            // Activate the UI
            stopFade();
            setActiveUi(true);
            clearInputField();
            inputField.ActivateInputField();
            inputField.Select();
        }
    }

    // Clears the input field text
    public void clearInputField()
    {
        inputField.text = "";
    }

    public void stopFade()
    {
        if (fadeOut != null)
            StopCoroutine(fadeOut);
    }

    // Breaks user input into an array of strings, split by spaces, and 
    //  calls associated function in CommandLineManaager
    public void parseInputCommand(string text)
    {
        text = text.ToLower();
        string parameter = "";
        string output = "";
        string[] input = text.Split(null);

        if (input.Length > 2)
        {
            for (int i = 1; i < input.Length; i++)
                parameter += input[i] + " ";
            parameter.Trim();
        }
        else if (input.Length == 2)
            parameter = input[1];

        if (verifyCommandExists(input[0]))
            output = CommandLineManager.commands[input[0]](parameter);

        if (output != "")
            print(output);
    }

    // Checks to see if the command can be found in the dictionary of commands
    public bool verifyCommandExists(string text)
    {
        if (!CommandLineManager.commands.ContainsKey(text))
            print("Command: '" + text + "' not recognized");

        return CommandLineManager.commands.ContainsKey(text);
    }

    // Just prints to Debug.Log for now, should instead display to the front end terminal eventually
    public void print(string input)
    {
        terminalText.text = terminalText.text + '\n' + input;
        //Debug.Log(input);
    }

    public void formatTerminalText(string input)
    {

    }

}

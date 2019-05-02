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
    public List<string> previousCommands;
    public int previousCommandIndex = 0;

    public bool ready;

    // Initialize needs to be called at the start of a scene, but it could be in awake() instead
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
            clearInputField();
            //fadeOut = StartCoroutine(fadeUi());
            inputField.ActivateInputField();
            inputField.Select();
            //ready = true;
        }

        // If the user pushes the 'up' key, set input to the last command
        if(commandLineField.activeSelf && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (previousCommandIndex > 0)
                previousCommandIndex -= 1;
            inputField.text = previousCommands[previousCommandIndex];
        }

        // If the user pushes the 'down' key, set input to more recent command
        if(commandLineField.activeSelf && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (previousCommandIndex < previousCommands.Count)
                previousCommandIndex += 1;
            if (previousCommandIndex == previousCommands.Count)
                inputField.text = "";
            else
            {
                inputField.text = previousCommands[previousCommandIndex];
            }
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

        ready = true;
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

    // Used to reset the appearance of the command line terminal
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
        commandLineField.SetActive(false);

        for (float i = 5; i >= 0; i -= Time.deltaTime)
        {
            float alpha = 1f;
            float panelAlpha = panelImage.color.a;
            if (i < 2.0f)
            {
                alpha = i / 2.0f;
                panelAlpha = Mathf.Min(panelAlpha, i / 2.0f);
            }

            Color panelColor = panelImage.color;
            Color textColor = inputText.color;
            panelColor.a = panelAlpha;
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
        if (!ready)
        {
            // Deactivate the UI
            GameManager.State = GameManager.PrevState;
            clearInputField();
            stopFade();
            ready = true;
            setActiveUi(false);
            CommandLineManager.isInUse = false;
        }
        else
        {
            // Activate the UI
            GameManager.State = GameState.Menu;
            stopFade();
            ready = false;
            setActiveUi(true);
            clearInputField();
            inputField.ActivateInputField();
            inputField.Select();
            CommandLineManager.isInUse = true;
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

        if (input[0] == "clear")
        {
            terminalText.text = "";
            return;
        }

        if (CommandLineManager.commands.ContainsKey(input[0]))
            output = CommandLineManager.commands[input[0]](parameter);
        else if (CommandLineManager.fluffCommands.ContainsKey(input[0]))
            output = CommandLineManager.fluffCommands[input[0]](parameter);
        else if (output != "")
            output = ("Command: '" + input[0] + "' not recognized");

        previousCommands.Add(text);
        previousCommandIndex = previousCommands.Count;
        print("> " + text);
        if(output != "")
            print(output);
    }

    // Just prints to Debug.Log for now, should instead display to the front end terminal eventually
    public void print(string input)
    {
        terminalText.text = terminalText.text + '\n' + input;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLineInputUI : MonoBehaviour
{

    public GameObject commandLineField;
    public GameObject panel;
    public GameObject terminalText;
    public InputField inputField;

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
            terminalToggleActive();
        }
    }

	public void initialize()
	{
        commandLineField = GetComponentInChildren<CommandLineField>(true).gameObject;
        panel = GetComponentInChildren<CommandLinePanel>(true).gameObject;
        terminalText = GetComponentInChildren<CommandLineTerminalText>(true).gameObject;
        inputField = commandLineField.GetComponentInChildren<InputField>();
        setActiveUi(false);
    }

    // Sets all UI elements to active or inactive, depending on if true or false is passed
    public void setActiveUi(bool active)
    {
        commandLineField.SetActive(active);
        panel.SetActive(active);
        terminalText.SetActive(active);
    }


	// Toggle whether the terminal is active
	public void terminalToggleActive()
    {
        if (commandLineField.activeSelf)
        {
            clearInputField();
            setActiveUi(false);
        }
        else
        {
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

    // Breaks user input into an array of strings, split by spaces, and 
    //  calls associated function in CommandLineManaager
    public void parseInputCommand(string text)
    {
        text = text.ToLower();
        string[] input = text.Split(null);
        string output = "";

        if (input.Length > 1)
        {
            if (verifyCommandExists(input[0]))
                output = CommandLineManager.commands[input[0]](input[1]);
        }
        else if (input.Length > 0)
        {
            if (verifyCommandExists(input[0]))
                output = CommandLineManager.commands[input[0]]("");
        }

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
        Debug.Log(input);
    }

}

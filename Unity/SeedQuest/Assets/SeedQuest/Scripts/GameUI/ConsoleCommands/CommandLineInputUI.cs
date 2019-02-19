using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLineInputUI : MonoBehaviour {

    public GameObject terminal;
    public InputField inputField;

    void Start()
    {
        terminal = GetComponentInChildren<Terminal>(true).gameObject;
        inputField = terminal.GetComponentInChildren<InputField>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            terminalToggleActive();
        }
        if (terminal.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            parseInputCommand(inputField.text);
            terminalToggleActive();
        }

    }

    // Toggle whether the terminal is active
    public void terminalToggleActive()
    {
        if (terminal.activeSelf)
            terminal.SetActive(false);
        else
        {
            terminal.SetActive(true);
            inputField.Select();
        }
    }

    // Breaks user input into an array of strings, split by spaces, and 
    //  calls associated function in CommandLineManaager
    public void parseInputCommand(string text)
    {
        Debug.Log("Input command recieved: " + text);
        string[] input = text.Split(null);

        if (input.Length > 1)
        {
            if (verifyCommandExists(input[0]))
                CommandLineManager.commands[input[0]](input[1]);
        }
        else if (input.Length > 0)
        {
            if (verifyCommandExists(input[0]))
                CommandLineManager.commands[input[0]]("");
        }
        else
            Debug.Log("No text entered");
    }

    // Checks to see if the command can be found in the dictionary of commands
    public bool verifyCommandExists(string text)
    {
        if (!CommandLineManager.commands.ContainsKey(text))
            Debug.Log("Command: " + text + " not recognized");
        
        return CommandLineManager.commands.ContainsKey(text);
    }

}

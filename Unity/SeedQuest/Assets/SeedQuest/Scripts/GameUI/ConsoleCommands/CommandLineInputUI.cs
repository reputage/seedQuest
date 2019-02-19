using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLineInputUI : MonoBehaviour {

    public GameObject terminal;
    public InputField inputField;

    public Dictionary<string, System.Action> commands;

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

    public void parseInputCommand(string text)
    {
        Debug.Log("Input command recieved: " + text);
    }

}

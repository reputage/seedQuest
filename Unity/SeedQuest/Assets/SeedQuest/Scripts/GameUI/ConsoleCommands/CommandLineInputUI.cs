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
        CommandLineManager.initialize();
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
        string[] input = text.Split(null);
        if (input.Length > 1)
        {
            if (verifyCommandExists(input[0]))
            {
                Debug.Log("text array longer than 1 word");
                CommandLineManager.commands[input[0]](input[1]);
            }
        }
        else if (input.Length > 0)
        {
            if (verifyCommandExists(input[0]))
            {
                Debug.Log("text array only one word in length");
                CommandLineManager.commands[input[0]]("");
            }
        }
        else
        {
            Debug.Log("No text entered");
        }
    }

    public bool verifyCommandExists(string text)
    {
        if (!CommandLineManager.commands.ContainsKey(text))
            Debug.Log("Command: " + text + " not recognized");
        return CommandLineManager.commands.ContainsKey(text);
    }

}

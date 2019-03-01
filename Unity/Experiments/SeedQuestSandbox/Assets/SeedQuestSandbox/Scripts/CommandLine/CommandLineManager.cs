using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SeedQuest.SeedEncoder;

public static class CommandLineManager
{

    // Initialize the dictionary 
    public static Dictionary<string, Func<string, string>> commands =
        new Dictionary<string, Func<string, string>>
    {
        {"help", help},
        {"print", print},
        {"gamestate", setGameState},
        {"loadscene", loadScene},
        {"seedtests", seedTests},
        {"moveplayer", movePlayer}
    };

    public static Dictionary<string, Func<string, string>> fluffCommands =
        new Dictionary<string, Func<string, string>>
    {
        {"hello", hello},
        {"jello", jello}

    };

    // Here's a template for an example of command. 
    //  For a command to work, it needs to be added to the above dictionary,
    //  and the dictionary key for the function needs to be all lowercase
    public static string templateCommand(string input)
    {
        // Put code here
        return input;
    }

    // Prints out a list of available command line commands
    public static string help(string input)
    {
        string returnString = "Available commands:";
        foreach (string key in commands.Keys)
        {
            returnString += "\n" + key;
        }
        return returnString;
    }

    // Just used for displaying information to the user
    public static string print(string input)
    {
        return input;
    }

    // Loads the scene specified by input, if it exists. A scene must be in the build settings
    //  for this command to work
    public static string loadScene(string input)
    {
        if (input == "")
            return "No scene specified";

        SceneManager.LoadScene(input);
        return "Loading scene: " + input;
    }

    // Example of running tests in command line, not actually funcitonal yet.
    public static string seedTests(string input)
    {
        MonoBehaviour seedBehavior = new SeedToByteTests();

        // TO DO: this can potentially cause memory problems, since Destroy() can't be used here
        string passedString = seedBehavior.GetComponent<SeedToByteTests>().runAllTests();
        return passedString;
    }

    // Placeholder function to move the player when playerManager gets imported into seedquest-sandbox
    public static string movePlayer(string input)
    {
        // Replace this line with the reference to the player object to move
        //GameObject player = new GameObject();

        string[] stringInputs = input.Split(null);
        int[] intInput = new int[3];
        bool validInts = false;
        for (int i = 0; i < intInput.Length; i++)
        {
            validInts = int.TryParse(stringInputs[i], out intInput[i]);
            Debug.Log("int " + i + ": " + intInput[i]);
        }

        Vector3 coordinates = new Vector3(intInput[0], intInput[1], intInput[2]);

        if (!validInts)
        {
            return "Invalid coordinates entered";
        }

        // Replace this with code relevant to changing the player position
        //else
        //{ player.transform.position = coordinates; }

        return "Moving player to " + intInput[0] + " " + intInput[1] + " " + intInput[2];
    }

    // Set the gamestate. string.StartsWith() is used so that the user input doesn't need to be
    //  perfectly correct to set some states (ex: 'rehears' will work with either 'rehearsal' 
    //  or 'rehearse' as the user input.
    public static string setGameState(string input)
    {
        if (input.StartsWith("recal") || input.StartsWith("recover"))
        {
            //GameManager.State = GameState.Recall;
            return "Game state set to Recall.";
        }

        if (input.StartsWith("rehears") || input.StartsWith("learn"))
        {
            //GameManager.State = GameState.Rehearsal;
            return "Game state set to Rehearsal.";
        }

        if (input.StartsWith("prev"))
        {
            GameManager.State = GameManager.PrevState;
            return "Game state set to previous state.";
        }

        if (input.StartsWith("gamest") || input.StartsWith("start"))
        {
            //GameManager.State = GameState.GameStart;
            return "Game state set to Recall.";
        }

        if (input.StartsWith("gameend") || input.StartsWith("end"))
        {
            //GameManager.State = GameState.GameEnd;
            return "Game state set to GameEnd.";
        }

        if (input.StartsWith("pause"))
        {
            GameManager.State = GameState.Pause;
            return "Game state set to Pause.";
        }

        if (input.StartsWith("inter"))
        {
            GameManager.State = GameState.Interact;
            return "Game state set to Interact.";
        }

        if (input.StartsWith("loadingrecall"))
        {
            //GameManager.State = GameState.LoadingRecall;
            return "Game state set to Loading Recall.";
        }

        if (input.StartsWith("loadingrehearse"))
        {
            //GameManager.State = GameState.LoadingRehersal;
            return "Game state set to Loading Rehearsal.";
        }

        return "Game state by name of '" + input + "' not found.";
    }

    // Say hello to the user - fluff function
    public static string hello(string input)
    {
        return "Hello world!";
    }

    // Say jello to the user - fluff funciton
    public static string jello(string input)
    {
        return "Well jello to you too! Nice to meet you!";
    }

}

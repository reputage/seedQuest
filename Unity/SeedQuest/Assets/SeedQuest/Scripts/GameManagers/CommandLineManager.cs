using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandLineManager
{

    // Initialize the dictionary 
    public static Dictionary<string, Func<string, string>> commands =
        new Dictionary<string, Func<string, string>>
    {
        {"help", help},
        {"print", print}
    };

    // Dictionary containing information on various topics for the user
    public static Dictionary<string, string> helpInformation = new Dictionary<string, string>
    {
        {"seedquest", "SeedQuest is a mnemonic application to help a user recover a private key"},
        {"rehearse", "Rehearsal mode is a practice mode to help a user remember the mnemonic path to recover their private key. Also called Learn mode"},
        {"learn", "Learn mode is a practice mode to help a user remember the mnemonic path to recover their private key. Also called rehearsal mode"},
        {"recall", "Recall mode is the mode used to recover a private key. Also called Recover mode"},
        {"recover", "Recover mode is the mode used to recover a private key. Also called Recall mode"},

    };

    // Displays some information to the user. If parameter string isn't found in helpInformation,
    //  then prints out a list of available commands
    public static string help(string input)
    {
        if (helpInformation.ContainsKey(input))
            return helpInformation[input];
        else
        {
            print("Available command line commands: ");
            foreach (string key in commands.Keys)
                print(key);
        }
        return "help";
    }

    // Just used for displaying information to the user
    public static string print(string input)
    {
        return input;
    }

    // This prints to the debug log for now, but should display text to the front end terminal eventually
    private static void privatePrint(string input)
    {
        Debug.Log(input);
    }

}

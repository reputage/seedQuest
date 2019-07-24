using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SeedQuest.SeedEncoder;
using SeedQuest.Interactables;
using SeedQuest.Debugger;


public static class CommandLineManager
{
    public static bool isInUse = false;

    // Initialize the dictionary. All key strings must be lowercase.
    public static Dictionary<string, Func<string, string>> commands =
        new Dictionary<string, Func<string, string>>
    {
        {"help", help},
        {"print", print},
        {"quit", quit},
        {"get", getValue},
        {"moveplayer", movePlayer},
        {"loadscene", loadScene},
        {"gamestate", setGameState},
        {"gamemode", setGameMode},
        {"showcolliders", showBoxColliders},
        {"nextaction", doNextAction},
        {"skip", skipScene},
        {"finduierrors", findUiErrors},
        {"customlearn", learnCustom},
        {"custombreak", learnBreak}
        //{"resetitem", resetInteractable},
        //{"selectaction", selectAction},

    };

    // Initialize the help dictionary. All key strings must be lowercase.
    public static Dictionary<string, string> helpDetails = new Dictionary<string, string>
    {
        {"help", "Displays a list of commands"},
        {"print", "Prints a string to the console"},
        {"quit", "Quits the application"},
        {"get", "Prints an available value.\nParameters:\n string valueName\n" + getHelp("")},
        {"moveplayer", "Moves the player to the specified location.\nParameters:\n int x, int y, int z"},
        {"loadscene", "Loads the specified scene. 'help scenes' returns a list of available scene names. \nParameters:\n string sceneName"},
        {"scenes", getSceneNames("")},
        {"gamestate", "Sets the gamestate.\nAccepted parameters:\n previous, pause, play, end, interact, menu"},
        {"gamemode", "Sets the gamemode in GameManager.\nParameters:\n Learn, recall, sandbox"},
        {"showcolliders", "Shows box colliders for interactables.\nUse 'showcolliders b' to show colliders for non-interctable objects"},
        {"nextaction", "Performs the next action in the interactable path list, only works in learn mode."},
        {"skip", "Skip the current scene."},
        {"finduierrors", "Finds collisions between interactable objects and their interactableUIs"},
        {"customlearn", "Creates a custom seed from input scene names and starts learn mode using that seed. \nParameters:\nSceneName1 SceneName2 SceneName3..."}
        //{"resetitem", "Finds and resets an item at the given site ID and spot ID.\nParameters:\n int siteID, int spotID"},
        //{"selectaction", "Performs an action using the specified interactable.\nParameters:\nint siteID, int spotID, int action"},

    };

    // Initialize fuzzy scene name dictionary.
    public static Dictionary<string, string> fuzzySceneNames = new Dictionary<string, string>
    {
        {"arabian", "arabianday"}, {"arab", "arabianday"},
        {"café", "cafe"},
        {"campground", "campground iso"}, {"camping", "campground iso"}, {"camp", "campground iso"},
        {"castle", "castlebeach"}, {"beach", "castlebeach"},
        {"cliff", "cliffsideiso"}, {"cliffs", "cliffsideiso"}, {"cliffside", "cliffsideiso"},
        {"dino", "dinosafari"}, {"dinosaur", "dinosafari"}, {"safari", "dinosafari"},
        {"farms", "farm"},
        {"haunted", "hauntedhouse"},
        {"lab", "lab_iso"}, {"science", "lab_iso"},
        {"restaurant", "nonnabig_iso"}, {"nonna", "nonnabig_iso"},
        {"pirate", "pirateship_wreck"}, {"pirates", "pirateship_wreck"}, {"pirateship", "pirateship_wreck"},
        {"saloon", "saloonbiggeriso"}, {"sallon", "saloonbiggeriso"}, {"western", "saloonbiggeriso"},
        {"snow", "snowland"},
        {"sorcerer", "sorcerertower"}, {"wizard", "sorcerertower"}, {"wizardtower", "sorcerertower"},
        {"moon", "space"},
        {"gym", "sports"}, {"workout", "sports"}
    };

    // Initialize fuzzy scene name dictionary.
    public static Dictionary<string, int> sceneIndeces = new Dictionary<string, int>
    {
        {"farm", 0},
        {"campground iso", 1}, 
        {"castlebeach", 2},
        {"cliffsideiso", 3},
        {"dinosafari", 4},
        {"hauntedhouse", 5},
        {"sports", 6}, 
        {"lab_iso", 7},
        {"arabianday", 8},
        {"nonnabig_iso", 9},
        {"pirateship_wreck", 10}, 
        {"saloonbiggeriso", 11},  
        {"snowland", 12},
        {"space", 13},
        {"sorcerertower", 14}, 
        {"cafe", 15}
    };

    // Here's a template for an example of a command. 
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
        string returnString = "";

        if (input == "")
        {
            returnString = "Available commands:";
            foreach (string key in commands.Keys)
            {
                returnString += "\n" + key;
            }
        }

        else if (helpDetails.ContainsKey(input))
            returnString = helpDetails[input];
        else
            returnString = "Command not found";

        return returnString;
    }

    // Just used for displaying information to the user
    public static string print(string input)
    {
        return input;
    }

    public static string quit(string input)
    {
        Application.Quit();
        return "";
    }

    // Loads the scene specified by input, if it exists. A scene must be in the build settings
    //  for this command to work
    public static string loadScene(string input)
    {
        if (input == "")
            return "No scene specified";

        if(fuzzySceneNames.ContainsKey(input))
        {
            SceneManager.LoadScene(fuzzySceneNames[input]);
            return "Loading scene: " + fuzzySceneNames[input];
        }

        SceneManager.LoadScene(input);
        return "Loading scene: " + input;
    }

    // Skip the current scene. Only works in Learn/reheasal mode.
    public static string skipScene(string input)
    {
        if (GameManager.Mode == GameMode.Rehearsal)
        {
            int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
            for (int i = 0 + actionsThisScene; i < InteractableConfig.ActionsPerSite; i++)
            {
                Debug.Log("Auto-performing action " + (i + 1));
                InteractableManager.SetActiveInteractable(InteractablePath.NextInteractable, InteractablePath.NextAction);
                InteractableLog.Add(InteractablePath.NextInteractable, InteractablePath.NextAction);
                InteractablePath.GoToNextInteractable();
            }
            return "Skiping scene";
        }
        else if (GameManager.Mode == GameMode.Recall)
        {
            int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
            for (int i = 0 + actionsThisScene; i < InteractableConfig.ActionsPerSite; i++)
            {
                Debug.Log("Auto-performing action " + (i + 1));
                InteractableLog.Add(InteractableManager.InteractableList[0], 0);
            }

            return "Skiping scene";
        }

        return "Skip scene only supports Learn and Recal modes.";
    }

    // Creates a custom learn mode path with scenes dtermined by the user.
    public static string learnCustom(string input)
    {
        return learnTest(input, false);
    }

    // Creates a custom learn mode that intentionaly tests whether a scene has all 16 interactables
    public static string learnBreak(string input)
    {
        return learnTest(input, true);
    }

    // Creates a custom learn mode - does the heavy lifting for the above two functions
    public static string learnTest(string input, bool tryToBreak)
    {
        string[] stringInputs = input.Split(null);
        int[] scenes = new int[InteractableConfig.SitesPerGame];
        int[] actions = new int[InteractableConfig.SitesPerGame + (InteractableConfig.SitesPerGame * InteractableConfig.ActionsPerSite * 2)];
        if (stringInputs.Length <= 0 || input == null || input == "")
        {
            return "Please enter at least one scene name for the custom learn path.";
        }
        Debug.Log("Input: ." + input + ".");
        Debug.Log("String input length: " + stringInputs.Length);

        for (int i = 0; i < InteractableConfig.SitesPerGame; i++)
        {
            // queue up the scenes entered by user into a seed

            if (i < stringInputs.Length)
            {
                if (sceneIndeces.ContainsKey(stringInputs[i]))
                {
                    scenes[i] = sceneIndeces[stringInputs[i]];
                }
                else if (fuzzySceneNames.ContainsKey(stringInputs[i]))
                {
                    scenes[i] = sceneIndeces[fuzzySceneNames[stringInputs[i]]];
                }
                else
                {
                    Debug.Log("String input length: " + stringInputs.Length);
                    return "Do not recognize scene name: " + stringInputs[i];
                }
            }
            else
            {
                scenes[i] = 2;
            }

        }

        // Create custom seed using the chosen scenes
        for (int i = 0; i < InteractableConfig.SitesPerGame; i++)
        {
            actions[i + i * InteractableConfig.ActionsPerSite * 2] = scenes[i];

            for (int j = 0; j < InteractableConfig.ActionsPerSite; j++)
            {
                if (tryToBreak)
                    actions[j * 2 + 1 + i + i * InteractableConfig.ActionsPerSite * 2] = j + 13;
                else
                    actions[j * 2 + 1 + i + i * InteractableConfig.ActionsPerSite * 2] = j % 16;
                
                actions[j * 2 + 2 + i + i * InteractableConfig.ActionsPerSite * 2] = j % 4;
            }
        }

        SeedToByte seeds = new SeedToByte();
        string seed = seeds.getSeed(actions);
        Debug.Log("Artificial seed: " + seed);

        InteractablePathManager.SeedString = seed;
        InteractablePath.ResetPath();
        InteractablePathManager.Reset();
        LevelSetManager.ResetCurrentLevels();

        for (int i = 0; i < scenes.Length; i++)
        {
            LevelSetManager.AddLevel(scenes[i]);
        }

        GameManager.Mode = GameMode.Rehearsal;
        GameManager.State = GameState.Play;
        InteractablePathManager.Initalize();

        if (fuzzySceneNames.ContainsKey(stringInputs[0]))
        {
            SceneManager.LoadScene(fuzzySceneNames[stringInputs[0]]);
            return "Loading custom seed. Scene: " + fuzzySceneNames[stringInputs[0]];
        }

        SceneManager.LoadScene(stringInputs[0]);

        return "Loading custom seed. Scene: " + stringInputs[0];
    }

    // Returns a list of all the available scenes in the build by name
    public static string getSceneNames(string input)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string returnString = "Available scenes:";

        for (int i = 0; i < sceneCount; i++)
            returnString += "\n" + System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

        return returnString;
    }

    // Example of running tests in command line, not actually funcitonal yet.
    public static string seedTests(string input)
    {
        MonoBehaviour seedBehavior = new SeedToByteTests();

        // TO DO: this can potentially cause memory problems, since Destroy() can't be used here
        string passedString = seedBehavior.GetComponent<SeedToByteTests>().runAllTests();
        return passedString;
    }

    // Show box colliders on all interactables
    public static string showBoxColliders(string input)
    {
        if (input == "true")
        {
            DebugManager.Instance.showBoundingBoxes = true;
            return "Activating box colliders";
        }
        else if (input == "false")
        {
            DebugManager.Instance.showBoundingBoxes = false;
            return "Deactivating box colliders";
        }
        else if (input == "b")
        {
            DebugManager.Instance.showOtherBoxes = !DebugManager.Instance.showOtherBoxes;
            return "Toggling other boxes";
        }
        else
            DebugManager.Instance.showBoundingBoxes = !DebugManager.Instance.showBoundingBoxes;

        return "Toggling box colliders";
    }

    // Placeholder function to move the player when playerManager gets imported into seedquest-sandbox
    public static string movePlayer(string input)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
            return "Cannot find a 'Player' object.";

        string[] stringInputs = input.Split(null);
        if (stringInputs.Length <= 3)
        {
            return "Invalid parameters: three integers required";
        }

        int[] intInput = new int[3];
        bool validInts = true;
        for (int i = 0; i < intInput.Length; i++)
        {
            validInts = int.TryParse(stringInputs[i], out intInput[i]) && validInts;
        }

        Vector3 coordinates = new Vector3(intInput[0], intInput[1], intInput[2]);

        if (!validInts)
            return "Invalid coordinates entered";
        else
            player.transform.position = coordinates;

        return "Moving player to " + intInput[0] + " " + intInput[1] + " " + intInput[2];
    }

    // Generates random number between 1 and 100
    public static string random(string input)
    {
        float rand = UnityEngine.Random.Range(1.0f, 100.0f);
        int randI = (int)rand;
        return "Your random number is: " + randI;
    }

    // In learn mode, perform the next queued action
    public static string doNextAction(string input)
    {
        if (InteractablePath.NextInteractable != null && GameManager.Mode == GameMode.Rehearsal)
        {
            InteractableManager.SetActiveInteractable(InteractablePath.NextInteractable, InteractablePath.NextAction);
            InteractableLog.Add(InteractablePath.NextInteractable, InteractablePath.NextAction);
            InteractablePath.GoToNextInteractable();
        }
        return "Performing next queued action";
    }

    // Input paramters: interactableID, action #
    // Incompatible with the current version of SeedQuest
    public static string selectAction(string input)
    {
        string[] stringInputs = input.Split(null);
        if (stringInputs.Length <= 3)
        {
            return "Invalid parameters: please enter a siteID, spotID, and action.";
        }
        int[] intInput = new int[3];
        bool validInts = true;
        for (int i = 0; i < intInput.Length; i++)
        {
            validInts = int.TryParse(stringInputs[i], out intInput[i]) && validInts;
        }

        if (!validInts)
            return "Invalid parameters: please use integers";

        foreach (Interactable item in InteractableManager.InteractableList)
        {
            if (item.ID.siteID == intInput[0] && item.ID.spotID == intInput[1])
            {
                item.SelectAction(intInput[2]);
                return "Preforming action with interactable at site: " + intInput[0] + " spot: " + intInput[1] + " and action: " + intInput[2];
            }
        }
        
        return "Could not find interactable with site ID " + intInput[0] + " and spot ID " + intInput[1];
    }

    // Reset the state of interactable with given siteID and spotID
    // Incompatible with current build of SeedQuest
    public static string resetInteractable(string input)
    {
        string[] stringInputs = input.Split(null);
        if (stringInputs.Length <= 2)
        {
            return "Invalid parameters: please enter a siteID and spotID.";
        }
        int[] intInput = new int[2];
        bool validInts = true;
        for (int i = 0; i < intInput.Length; i++)
        {
            validInts = int.TryParse(stringInputs[i], out intInput[i]) && validInts;
        }

        if (!validInts)
            return "Invalid parameters: please use integers";
        
        foreach (Interactable item in InteractableManager.InteractableList)
        {
            if (item.ID.siteID == intInput[0] && item.ID.spotID == intInput[1])
            {
                item.ActionIndex = 0;
                InteractableState state = item.stateData.states[item.ActionIndex];
                state.enterState(item);
                //item.HighlightInteractable(true, true);
                item.interactableUI.SetActionUI(item.ActionIndex);
                return "resetting interactable at site: " + intInput[0] + " and spot: " + intInput[1];
            }
        }

        return "Could not find interactable at site: " + intInput[0] + " and spot: " + intInput[1];
    }

    // Returns values from various manager scripts, for example 'get gamestate' returns the gamestate
    public static string getValue(string input)
    {
        string returnStr = "";
        string parameter = "";

        string[] splitText = input.Split(null);

        if (splitText.Length > 2)
        {
            for (int i = 1; i < splitText.Length; i++)
                parameter += splitText[i] + " ";
            parameter = parameter.Trim();
        }
        else if (splitText.Length == 2)
            parameter = splitText[1];

        if (CommandLineGetValues.values.ContainsKey(splitText[0]))
            returnStr = CommandLineGetValues.values[splitText[0]](parameter);
        else
            returnStr = "Value not found";

        return returnStr;
    }

    // Returns a list of the values available through the 'get' command
    public static string getHelp(string input)
    {
        string returnString = "Available values:";
        foreach (string key in CommandLineGetValues.values.Keys)
        {
            returnString += "\n" + key;
        }
        return returnString;
    }

    public static string findUiErrors(string input)
    {
        string returnStr = "Errors found for these interactables:";
        int lacksCollider = 0;

        foreach (Interactable item in InteractableManager.InteractableList)
        {
            BoxCollider[] boxes = item.GetComponentsInChildren<BoxCollider>();
            bool checkedOnce = false;
            if (boxes != null && boxes.Length > 1)
            {
                for (int i = 0; i < boxes.Length; i++)
                {
                    if (i < boxes.Length - 1)
                    {
                        for (int j = i + 1; j < boxes.Length; j++)
                        {
                            if (boxes[i].bounds.Intersects(boxes[j].bounds) && !checkedOnce)
                            {
                                Debug.Log("Intersection between item: " + item.name + " and it's UI.");
                                returnStr += "\nItem: " + item.name + " ";
                                checkedOnce = true;
                            }
                        }
                    }
                }
            }
            else if (boxes.Length <= 1)
            {
                lacksCollider += 1;
            }
        }
        // Collision errors were found, and also some UI elements had no colliders
        if (lacksCollider > 0 && returnStr.Length > 40)
        {
            returnStr = returnStr + "\nCould not find collider objects for " + lacksCollider + " interactable UIs.";
        }

        // If no collisions were found, but also there were some UI elements with no colliders
        else if (lacksCollider > 0 && returnStr.Length <= 39)
        {
            returnStr = "No collisions found between interactables and UI.\nCould not find collider objects for " + lacksCollider + " interactable UIs.";
        }

        // If no collisions were found, and all interactables had UI elements with colliders
        else if (lacksCollider == 0 && returnStr.Length <= 39)
        {
            Debug.Log("No collisions found.");
            returnStr = "No collisions found between interactables and UI";
        }

        return returnStr;
    }

    // Set the gamestate. string.StartsWith() is used so that the user input doesn't need to be
    //  perfectly correct to set some states (ex: 'rehears' will work with either 'rehearsal' 
    //  or 'rehearse' as the user input. Some states are commented out because they don't exist
    //  in this build.
    public static string setGameState(string input)
    {
        if (input.StartsWith("prev"))
        {
            GameManager.State = GameManager.PrevState;
            return "Game state set to previous state.";
        }
        if (input.StartsWith("paus"))
        {
            GameManager.State = GameState.Pause;
            return "Game state set to Pause.";
        }
        if (input.StartsWith("inter"))
        {
            GameManager.State = GameState.Interact;
            return "Game state set to Interact.";
        }
        if (input.StartsWith("end"))
        {
            GameManager.State = GameState.End;
            return "Game state set to End.";
        }
        if (input.StartsWith("menu"))
        {
            GameManager.State = GameState.Menu;
            return "Game state set to Menu.";
        }
        if (input.StartsWith("play"))
        {
            GameManager.State = GameState.Play;
            return "Game state set to Play.";
        }

        return "Game state by name of '" + input + "' not found.";
    }

    // Set the game mode
    public static string setGameMode(string input)
    {
        if (input.StartsWith("rec"))
        {
            GameManager.Mode = GameMode.Recall;
            return "Game mode set to recall.";
        }
        if (input.StartsWith("learn"))
        {
            GameManager.Mode = GameMode.Rehearsal;
            return "Game mode set to learn.";
        }
        if (input.StartsWith("rehea"))
        {
            GameManager.Mode = GameMode.Rehearsal;
            return "Game mode set to rehearsal.";
        }
        if (input.StartsWith("sand"))
        {
            GameManager.Mode = GameMode.Sandbox;
            return "Game mode set to sandbox.";
        }

        return "Game mode by name of '" + input + "' not found.";
    }

    // From here all functions are 'fluff' functions - they are just here for fun, and 
    //  are not necessary for debug purposes

    // Dictionary for 'fluff' functions. They are not necessary, and are just for fun
    //  These are in a separate dictionary so that they won't be displayed to the 
    //  user from the 'help' command output.
    public static Dictionary<string, Func<string, string>> fluffCommands =
        new Dictionary<string, Func<string, string>>
    {
        {"helpfluff", helpFluff},
        {"hello", hello},
        {"jello", jello},
        {"hi", hi},
        {"quick", quickBrown},
        {"lorem", loremIpsum},
        {"dog", asciiDog}
    };

    // Help - for fluff functions
    public static string helpFluff(string input)
    {
        string returnString = "Available commands:";
        foreach (string key in fluffCommands.Keys)
        {
            returnString += "\n" + key;
        }
        return returnString;
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

    // Ask them how they're doing - fluff funciton
    public static string hi(string input)
    {
        return "Hi! How are you today?";
    }

    // Prints the quick brown fox line - fluff funciton
    public static string quickBrown(string input)
    {
        return "The quick brown fox jumps over the lazy dog.";
    }

    // Prints out lorem ipsum - fluff funciton
    public static string loremIpsum(string input)
    {
        return "Lorem ipsum dolor sit amet";
    }

    // This ascii dog is actually pretty helpful in debugging the UI part of the command line
    public static string asciiDog(string input)
    {
        string ascii = "      ,";
        ascii += "\n      |`-.__ ";
        ascii += "\n      /   '  _/ ";
        ascii += "\n      ****`";
        ascii += "\n     /       } ";
        ascii += "\n    /    \\  / ";
        ascii += "\n \\ /`     \\\\\\ ";
        ascii += "\n  `\\      /_\\\\ ";
        ascii += "\n   `~~~~``~`";
        return ascii;
    }

}

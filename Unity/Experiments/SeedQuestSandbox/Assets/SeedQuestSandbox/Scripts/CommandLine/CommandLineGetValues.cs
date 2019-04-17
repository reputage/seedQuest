using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using SeedQuest.Interactables;
using SeedQuest.Debugger;
using SeedQuest.Level;

public static class CommandLineGetValues
{

    // Initialize the dictionary. All key strings must be lowercase.
    public static Dictionary<string, Func<string, string>> values =
        new Dictionary<string, Func<string, string>>
    {
        {"count", interactableCount},
        {"gamestate", gameState},
        {"gamemode", gameMode},
        {"interactable", getInteractableData},
        {"list", interactableList},
        {"log", getLogData},
        {"path", getPathData},
        {"prevstate", prevState},
        {"statics", statics}
    };

    // Initialize the dictionary of references to static classes. All key strings must be lowercase.
    public static Dictionary<string, object> staticsDict = new Dictionary<string, object>
    {
        {"debugmanager", DebugManager.Instance},
        {"effectsmanager", EffectsManager.instance},
        {"gamemanager", GameManager.Instance},
        {"hudmanager", HUDManager.Instance},
        {"interactablelog", InteractableLog.Instance},
        {"interactablemanager", InteractableManager.Instance},
        {"interactablepath", InteractablePath.Instance},
        {"interactablepathmanager", InteractablePathManager.Instance}, 
        {"levelmanager", LevelManager.Instance},
        {"markermanager", MarkerManager.instance}, 
        {"settingsmanager", SettingsManager.Instance}, 
    };

    public static string interactableCount(string input)
    {
        int counter = 0;
        foreach (Interactable item in InteractableManager.InteractableList)
            counter += 1;

        string result = counter + " interactables found in this scene.";
        return result;
    }

    public static string interactableList(string input)
    {
        string returnString = "Interactables in this scene: ";
        returnString += "\nName:   Site: Spot: Action: ObjectName:";
        int length = returnString.Length;
        foreach (Interactable item in InteractableManager.InteractableList)
        {
            returnString += "\n" + item.Name + " " + item.ID.siteID + " " + item.ID.spotID + 
                                       " " + item.ID.actionID + " " + item.name;
        }

        if (returnString.Length <= length)
        {
            returnString = "No interactables found in this scene";
        }

        return returnString;
    }

    public static string gameState(string input)
    {
        if (GameManager.Instance == null)       // Unfortunately this doesn't seem to work right
            return "No gamemanager exists";
        if (GameManager.State == GameState.End)
            return "End";
        else if (GameManager.State == GameState.Interact)
            return "Interact";
        else if (GameManager.State == GameState.Menu)
            return "Menu";
        else if (GameManager.State == GameState.Pause)
            return "Pause";
        else if (GameManager.State == GameState.Play)
            return "Play";
        else
            return "Cannot determine game mode";
    }

    public static string gameMode(string input)
    {
        if (GameManager.Mode == GameMode.Recall)
            return "Recall";
        else if (GameManager.Mode == GameMode.Rehearsal)
            return "Learn";
        else if (GameManager.Mode == GameMode.Sandbox)
            return "Sandbox";
        else
            return "Cannot determine game mode";
    }

    public static string prevState(string input)
    {
        if (GameManager.PrevState == GameState.End)
            return "End";
        else if (GameManager.PrevState == GameState.Interact)
            return "Interact";
        else if (GameManager.PrevState == GameState.Menu)
            return "Menu";
        else if (GameManager.PrevState == GameState.Pause)
            return "Pause";
        else if (GameManager.PrevState == GameState.Play)
            return "Play";
        else
            return "Cannot determine game mode";
    }

    public static string statics(string input)
    {
        object obj = null;

        string[] splitText = input.Split(null);

        if (staticsDict.ContainsKey(input))
        {
            obj = staticsDict[input];
        }
        else if (staticsDict.ContainsKey(splitText[0]))
        {
            obj = staticsDict[splitText[0]];
        }
        else
            return "Could not find an object, class, or manager by that name.\n" + getAvailableStatics("");

        string returnString = getStaticFields(obj, input);
        return returnString;
    }

    // Returns a list of the values available through the 'get statics' command
    public static string getAvailableStatics(string input)
    {
        string returnString = "Available classes:";
        foreach (string key in staticsDict.Keys)
        {
            returnString += "\n" + key;
        }
        return returnString;
    }

    public static string getStaticFields(object obj, string objName)
    {
        string returnString = "Variables for object: " + objName;
        int length = returnString.Length;
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            returnString += "\nField: " + field.Name + " Value: " + field.GetValue(null).ToString();
        }
        if (returnString.Length <= length)
        {
            returnString = "Could not find any available variables";
        }

        return returnString;
    }

    // Returns the path data from the existing interactable path instance
    public static string getPathData(string input)
    {
        return formatPathData(InteractablePath.Instance);
    }

    //Formats the path data into a more readable format
    public static string formatPathData(InteractablePath pathObject)
    {
        if (pathObject == null)
        {
            return "Could not find instance of InteractablePath object";
        }
        string data = "Interactable path data:";
        foreach (Interactable item in pathObject.path)
        {
            data += "\n" + stringifyInteractable(item);
        }
        return data;
    }

    // This function parses out the input to determine if the user is searching
    //  by interactable name or by ID, and returns data for the found interactable.
    public static string getInteractableData(string input)
    {
        string[] stringInputs = input.Split(null);
        int[] intInput = new int[stringInputs.Length];
        bool validInts = true;

        if (stringInputs.Length > 1)
            for (int i = 0; i < stringInputs.Length; i++)
                validInts = int.TryParse(stringInputs[i], out intInput[i]) && validInts;
        else
            validInts = false;

        foreach (Interactable item in InteractableManager.InteractableList)
        {
            // if integers were input for the command, check for site and spot ID matches
            if (validInts)
                if (item.ID.siteID == intInput[0] && item.ID.spotID == intInput[1])
                    return stringifyInteractable(item);

            if (item.Name.ToLower() == input) // first checks the interactable's 'Name' variable
                return stringifyInteractable(item);
            else if (item.Name.ToLower() == stringInputs[0])
                return stringifyInteractable(item);
            else if (item.name.ToLower() == input) // checks the interactable's unity object name
                return stringifyInteractable(item);
            else if (item.Name.ToLower() == stringInputs[0])
                return stringifyInteractable(item);
        }

        return "Could not find any interactable by that name or ID.";
    }

    // Returns the log data from the existing interactable log instance
    public static string getLogData(string input)
    {
        return formatLogData(InteractableLog.Instance);
    }

    // Formats the log data into a more readable form
    public static string formatLogData(InteractableLog logObject)
    {
        if (logObject == null)
        {
            return "Could not find instance of InteractableLog object";
        }

        string data = "Interactable log data:";
        foreach (InteractableLogItem item in logObject.log)
        {
            data += "\n" + stringifyLogItem(item);
        }
        return data;
    }

    // Returns a string of an Interactable Log item's data
    public static string stringifyLogItem(InteractableLogItem item)
    {
        string data = "Site: " + item.SiteIndex + " Index: " + item.InteractableIndex + " Action: " + item.ActionIndex;
        return data;
    }

    // Returns a string of an Interactable's data
    public static string stringifyInteractable(Interactable item)
    {
        string data = "Name: " + item.Name + " Site: " + item.ID.siteID + " Index: " + item.ID.spotID + " Action: " + item.ID.actionID;
        return data;
    }

    // Overload function for the above function. Accepts Interactable ID instead of Interactable
    public static string stringifyInteractable(InteractableID item)
    {
        string data = "Site: " + item.siteID + " Index: " + item.spotID + " Action: " + item.actionID;
        return data;
    }

    //coneinteractable

}

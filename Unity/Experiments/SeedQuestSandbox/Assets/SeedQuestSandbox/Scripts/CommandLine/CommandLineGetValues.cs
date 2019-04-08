using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using SeedQuest.Interactables;

public static class CommandLineGetValues
{

    // Initialize the dictionary. All key strings must be lowercase.
    public static Dictionary<string, Func<string, string>> values =
        new Dictionary<string, Func<string, string>>
    {
        {"gamestate", gameState},
        {"gamemode", gameMode},
        {"prevstate", prevState},
        {"statics", statics},
        {"log", getLogData},
        {"path", getPathData}
    };

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
        if (splitText[0] == "path")
            obj = InteractablePathManager.Instance;

        string returnString = getFieldValues(obj, input);
        return returnString;
    }

    public static string getFieldValues(object obj, string objName)
    {
        string returnString = "Variables for object: " + objName;

        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
        foreach (FieldInfo field in fields)
        {
            returnString += "\nField: " + field.Name + " Value: " + field.GetValue(null).ToString();
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
        foreach(InteractableLogItem item in logObject.log)
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
        string data = "Site: " + item.ID.siteID + " Index: " + item.ID.spotID + " Action: " + item.ID.actionID;
        return data;
    }

    // Overload function for the above function. Accepts Interactable ID instead of Interactable
    public static string stringifyInteractable(InteractableID item)
    {
        string data = "Site: " + item.siteID + " Index: " + item.spotID + " Action: " + item.actionID;
        return data;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CommandLineGetValues
{

    // Initialize the dictionary 
    public static Dictionary<string, Func<string, string>> values =
        new Dictionary<string, Func<string, string>>
    {
        {"gamestate", gameState},
        {"gamemode", gameMode},
        {"prevstate", prevState}
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

}

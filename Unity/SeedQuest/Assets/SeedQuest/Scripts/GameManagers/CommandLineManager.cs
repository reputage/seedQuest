using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandLineManager {

    public static Dictionary<string, Func<string, string>> commands = new Dictionary<string, Func<string, string>>();



    public static void initialize()
    {
        commands.Add("test", test);
    }

    public static string test(string testText)
    {
        Debug.Log("testText");
        return testText;
    }

}

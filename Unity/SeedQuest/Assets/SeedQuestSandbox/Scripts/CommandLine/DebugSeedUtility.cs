using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugSeedUtility
{

    public static bool debugLearnRun = false;
    public static bool debugLearnRand = false;

    public static Dictionary<int, string> sceneIndeces = new Dictionary<int, string>
    {
        {0, "farm"},
        {1, "campground iso"},
        {2, "castlebeach"},
        {3, "cliffsideiso"},
        {4, "dinosafari"},
        {5, "hauntedhouse"},
        {6, "sports"},
        {7, "lab_iso"},
        {8, "arabianday"},
        {9, "nonnabig_iso"},
        {10, "pirateship_wreck"},
        {11, "saloonbiggeriso"},
        {12, "snowland"},
        {13, "space"},
        {14, "sorcerertower"},
        {15, "cafe"}
    };

    public static Dictionary<string, string> sceneCasualNames = new Dictionary<string, string>
    {
        {"Farm", "farm"},
        {"Campground Iso", "camp"},
        {"CastleBeach", "castlebeach"},
        {"CliffSideIso", "cliffsideiso"},
        {"DinoSafari", "dinosafari"},
        {"HauntedHouse", "hauntedhouse"},
        {"Sports", "sports"},
        {"Lab_ISO", "lab_iso"},
        {"ArabianDay", "arabianday"},
        {"NonnaBIG_ISO", "nonnabig_iso"},
        {"PirateShip_Wreck", "pirateship_wreck"},
        {"SaloonBiggerISO", "saloonbiggeriso"},
        {"SnowLand", "snowland"},
        {"Space", "space"},
        {"SorcererTower", "sorcerertower"},
        {"Cafe", "cafe"}
    };

    public static string convertLevelSet()
    {
        string levelNames = "";
        if (GameManager.V2Menus)
        {
            for (int i = 0; i < WorldManager.CurrentSceneList.Length; i++)
                levelNames += sceneCasualNames[WorldManager.CurrentSceneList[i].sceneName] + " ";
        }
        else
        {
            for (int i = 0; i < LevelSetManager.CurrentLevels.Count; i++)
                levelNames += sceneIndeces[LevelSetManager.CurrentLevels[i].index] + " ";
        }

        Debug.Log("Level name string: " + levelNames);
        return levelNames;
    }

    public static void startIterative()
    {
        Debug.Log("Beginning iterative debug seed!");
        CommandLineManager.learnTest(convertLevelSet(), false, true, false);
    }

    public static void startRandom()
    {
        Debug.Log("Beginning random debug seed!");
        CommandLineManager.learnTest(convertLevelSet(), true, false, false);
    }



}

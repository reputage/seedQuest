using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

public class LevelSetManager : MonoBehaviour
{
    static private LevelSetManager instance = null;
    static private LevelSetManager setInstance() { instance = GameManager.Instance.GetComponentInChildren<LevelSetManager>(true); return instance; }
    static public LevelSetManager Instance { get { return instance == null ? setInstance() : instance; } }
    static public bool isActive = true;

    public List<LevelProps> allLevels;
    static public List<LevelProps> AllLevels { get => Instance.allLevels; }
    public List<LevelProps> currentLevels;
    static public List<LevelProps> CurrentLevels { get => Instance.currentLevels; }
    static public LevelProps CurrentLevel { get => Instance.currentLevels.Count > 0 ? Instance.currentLevels[InteractableLog.CurrentLevelIndex] : null; }

    [SerializeField]
    private List<LevelProps> levels;

    void Start() {
        levels = new List<LevelProps>();

        foreach (LevelProps prop in allLevels)
            prop.index = allLevels.IndexOf(prop);
    } 

    static public void AddLevel(int levelIndex) {
        CurrentLevels.Add(AllLevels[levelIndex]);
    }

    /// <summary>  Gets the index of the current level based on log count </summary>
    static public int CurrentLevelIndex
    {
        get { return (int)Mathf.Floor(InteractableLog.Count / InteractableConfig.ActionsPerSite); }
    }

    static public void ResetCurrentLevels() {
        Instance.currentLevels.Clear();
    }

}
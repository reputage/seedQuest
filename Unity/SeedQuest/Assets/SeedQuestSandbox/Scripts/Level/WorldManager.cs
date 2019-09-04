using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

public class WorldManager : MonoBehaviour
{
    static private WorldManager instance = null;
    static private WorldManager setInstance() { instance = GameManager.Instance.GetComponentInChildren<WorldManager>(true); return instance; }
    static public WorldManager Instance { get { return instance == null ? setInstance() : instance; } }

    public WorldSceneList worldSceneList;    
    public List<WorldSceneProps> currentScenes;

    static public WorldSceneProps[] WorldScenes { get => Instance.worldSceneList != null ? Instance.worldSceneList.worldScenes : null; }
    static public WorldSceneProps[] CurrentSceneList { get => Instance.currentScenes.ToArray(); }
    static public WorldSceneProps CurrentWorldScene { get => Instance.currentScenes.Count > 0 ? Instance.currentScenes[InteractableLog.CurrentLevelIndex] : null; }

    /// <summary> Adds world scene with index </summary>
    static public void Add(int index) {
        Debug.Log("Adding to current scenes");
        Instance.currentScenes.Add(Instance.worldSceneList.worldScenes[index]);
    }

    /// <summary>  Removes last world scene added </summary>
    static public void RemoveLast() {
        Instance.currentScenes.RemoveAt(Instance.currentScenes.Count - 1);
    }

    /// <summary>  Resets current world scenes </summary>
    static public void Reset() {
        Instance.currentScenes.Clear();
    }

    static public int GetSiteIndexForCurrentWorldScene() {
        int index = Array.FindIndex(WorldScenes, row => row.name == CurrentWorldScene.name);
        return index;
    }
}
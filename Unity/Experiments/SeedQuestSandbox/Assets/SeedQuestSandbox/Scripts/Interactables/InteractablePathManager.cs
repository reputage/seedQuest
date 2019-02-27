using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.SeedEncoder;
using SeedQuest.Utils;
using SeedQuest.Level;

namespace SeedQuest.Interactables {
    
    public class InteractablePathManager : MonoBehaviour
    {
        static InteractablePathManager instance = null;

        public static InteractablePathManager Instance {
            get {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<InteractablePathManager>();

                if (instance == null)
                    instance = GameManager.Instance.gameObject.AddComponent<InteractablePathManager>();

                return instance;
            }
        }

        public List<Interactable> path;

        public List<InteractableLogItem> log;

        public string seedString;

        public static string SeedString { 
            get { return Instance.seedString; }
        }

        private bool isNextHighlighted = false;

        private void Awake() {
            seedString = "EB204654C9";
            //seedString = RandomUtils.GetRandomHexNumber(10);

            if (InteractableManager.InteractableList.Length == 0)
                return;

            SetupInteractablePathIDs();
            InitalizePathAndLog();
        }

        private void Update() {
            if (InteractableManager.InteractableList.Length == 0)
                return;

            if (GameManager.Mode == GameMode.Rehearsal) {

                if (!isNextHighlighted) {  
                    InteractablePath.InitializeNextInteractable();
                    isNextHighlighted = true;
                }

                if(InteractablePath.PathComplete) {
                    GameManager.State = GameState.End;
                    EndGameUI.ToggleOn();
                }
                else if(LevelManager.IsMultiLevelGame && InteractablePath.PathLevelComplete) {
                    GameManager.State = GameState.Menu;
                    LevelClearUI.ToggleOn();
                }
            }
            else if(GameManager.Mode == GameMode.Recall) {
                if(InteractableLog.PathComplete) {
                    SeedConverter converter = new SeedConverter();
                    seedString = converter.DecodeSeed();
                    GameManager.State = GameState.End;
                    EndGameUI.ToggleOn();
                }
                else if(LevelManager.IsMultiLevelGame && InteractableLog.PathLevelComplete) {
                    GameManager.State = GameState.Menu;
                    LevelClearUI.ToggleOn();
                }
            }
        }

        static public void InitalizePathAndLog() {
            Instance.isNextHighlighted = false;

            InteractablePath.GeneratePathFromSeed(Instance.seedString);
            Instance.path = InteractablePath.Path;

            InteractableLog.Clear();
            Instance.log = InteractableLog.Log;
        }

        static public void SetupInteractablePathIDs() {
            Interactable[] list = InteractableManager.InteractableList;

            int[] indices = RandomUtils.GetRandomIndexArray(InteractableConfig.InteractableCount);

            int siteCount = LevelManager.LevelIndex;
            foreach (BoundingBox bounds in LevelManager.Bounds) {
                
                // Create a subset of interactables in bounds
                List<Interactable> subset = new List<Interactable>();
                foreach(Interactable item in list) {
                    if(BoundingBox.InBounds(item.transform, bounds)) {
                        subset.Add(item);
                    }
                }

                // Update siteID and spot ID for in-bounds subset
                for (int i = 0; i < subset.Count; i++) {
                    subset[i].ID.siteID = siteCount;
                    if (i < indices.Length)
                        subset[i].ID.spotID = indices[i];
                    else
                        subset[i].ID.spotID = -1;
                }

                if (subset.Count < InteractableConfig.InteractableCount)
                    Debug.Log("WARNING: SiteBounds does not contain sufficent interactables.");

                siteCount++;
            }
        }
    }
}
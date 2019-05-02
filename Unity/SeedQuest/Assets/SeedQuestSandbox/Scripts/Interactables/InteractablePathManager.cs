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

        /// <summary> Reference to Interactable Path </summary>
        public InteractablePath path;

        /// <summary> Reference to Interactable Log </summary>
        public InteractableLog log;

        /// <summary> Seed String </summary>
        public static string SeedString = "76101B07DC633F955696D7664C2B"; //"EBE0AC8C"; //"EB204654C9";

        /// <summary> Has a Level been compleleted for MultiLevel Game </summary>
        static public bool ShowLevelComplete = false;

        /// <summary> Flag for Interactabled Initlized first time </summary>
        static private bool isNextHighlighted = false;

        private void Awake() {
            path = InteractablePath.Instance;
            log = InteractableLog.Instance;

            //InitializeSeed();
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
                else if(LevelManager.IsMultiLevelGame && ShowLevelComplete) {
                    GameManager.State = GameState.Menu;
                    LevelClearUI.ToggleOn();
                }
            }
            else if(GameManager.Mode == GameMode.Recall) {
                if(InteractableLog.PathComplete) {
                    GameManager.State = GameState.End;
                    EndGameUI.ToggleOn();
                }
                else if(LevelManager.IsMultiLevelGame && ShowLevelComplete) {
                    GameManager.State = GameState.Menu;
                    LevelClearUI.ToggleOn();
                }
            }
        }

        static public void UndoLastAction() {
            if (GameManager.Mode == GameMode.Rehearsal)
                InteractablePath.UndoLastAction();
            else if (GameManager.Mode == GameMode.Recall)
                InteractableLog.UndoLastAction();
        }

        static public int LevelsComplete {
            get => (int) Mathf.Floor((float)InteractableLog.Count / (float)InteractableConfig.ActionsPerSite);
        }

        static public void InitializeSeed() {
            SeedString = "EBE0AC8C";
        }

        static public void SetRandomSeed() {
            SeedString = RandomUtils.GetRandomHexNumber(InteractableConfig.SeedHexLength);
        }

        /// <summary>  Reset InteractablePathManager State </summary>
        static public void Reset() {
            ShowLevelComplete = false;
            InteractablePath.ResetPath();
            InteractableLog.Clear();
        }

        /// <summary> Initalizes Interactable Path and Log </summary>
        static public void Initalize() {
            ShowLevelComplete = false;
            isNextHighlighted = false;

            InteractablePath.GeneratePathFromSeed(SeedString);
            InteractablePath.InitializeNextInteractable();
        }

        static public InteractableID[] GetPathIDs() {
            return InteractablePath.GetPathIDsFromSeed(SeedString);
        }

        static public int[] GetPathSiteIDs() {
            InteractableID[] iDs = GetPathIDs();
            List<int> siteIDs = new List<int>();
            for (int i = 0; i < InteractableConfig.SitesPerGame; i++) {
                siteIDs.Add(iDs[InteractableConfig.ActionsPerSite * i].siteID);
            }
            return siteIDs.ToArray();
        } 

        static public int NextInteractableSiteID() {
            return GetPathIDs()[InteractablePath.Instance.nextIndex].siteID;
        }

        /// <summary> Initializes Interactable Path Site and Interactable IDs </summary>
        static public void SetupInteractablePathIDs() {
            Interactable[] list = InteractableManager.InteractableList;

            //int[] indices = RandomUtils.GetRandomIndexArray(InteractableConfig.InteractableCount);
            int[] indices = NumberUtils.GetIndexArray(InteractableConfig.InteractableCount);

            // Set InteractablePath IDs based on BoundingBoxes in Scene 
            int siteCount = LevelManager.LevelIndex;
            if (LevelSetManager.isActive && MenuScreenManager.Instance.state != MenuScreenStates.Debug )
                siteCount = LevelSetManager.CurrentLevel.index;
            
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

                // Throw Error for not enough interactables in a Site
                if (GameManager.Mode != GameMode.Sandbox && subset.Count < InteractableConfig.InteractableCount)
                    Debug.Log("WARNING: SiteBounds does not contain sufficent interactables.");

                siteCount++;
            }
        }
    }
}
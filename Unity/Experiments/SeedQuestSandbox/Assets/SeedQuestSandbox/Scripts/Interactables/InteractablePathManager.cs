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
        public static string SeedString; 

        /// <summary> Has the Interactabled Path been initialized before important for MultiLevel Games </summary>
        static public bool IsPathInitialized = false;

        /// <summary> Has a Level been compleleted for MultiLevel Game </summary>
        static public bool ShowLevelComplete = false;

        /// <summary> Flag for Interactabled Initlized first time </summary>
        static private bool isNextHighlighted = false;

        private void Awake() {
            path = InteractablePath.Instance;
            log = InteractableLog.Instance;

            InitializeSeed();
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
                    SeedConverter converter = new SeedConverter();
                    SeedString = converter.DecodeSeed();
                    GameManager.State = GameState.End;
                    EndGameUI.ToggleOn();
                }
                else if(LevelManager.IsMultiLevelGame &&  ShowLevelComplete) {
                    GameManager.State = GameState.Menu;
                    LevelClearUI.ToggleOn();
                }
            }
        }

        static public void InitializeSeed() {
            SeedString = "EB204654C9";
            //seedString = RandomUtils.GetRandomHexNumber(10);
        }


        static public void Reset() {
            IsPathInitialized = false;
            ShowLevelComplete = false;
        }

        /// <summary> Reset Interactable Path and Log </summary>
        static public void InitalizePathAndLog() {
            InteractablePath.ResetPath();
            InteractablePath.GeneratePathFromSeed(SeedString);
            InteractableLog.Clear();

            isNextHighlighted = false;
            IsPathInitialized = true;
            ShowLevelComplete = false;
        }

        /// <summary> Intialize Interactable Path and Log for MultiLevel Game </summary>
        static public void InitalizePathAndLogForMultiLevelGame() {
            InteractablePath.GeneratePathFromSeed(SeedString);
            InteractablePath.InitializeNextInteractable();

            isNextHighlighted = false;
            ShowLevelComplete = false;
        }

        static public InteractableID[] GetPathIDs() {
            return InteractablePath.GetPathIDsFromSeed(SeedString);
        }

        static public int NextInteractableSiteID() {
            return GetPathIDs()[InteractablePath.Instance.nextIndex].siteID;
        }

        /// <summary> Initializes Interactable Path Site and Interactable IDs </summary>
        static public void SetupInteractablePathIDs() {
            Interactable[] list = InteractableManager.InteractableList;

            int[] indices = RandomUtils.GetRandomIndexArray(InteractableConfig.InteractableCount);

            // Set InteractablePath IDs based on BoundingBoxes in Scene 
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

                // Throw Error for not enough interactables in a Site
                if (GameManager.Mode != GameMode.Sandbox && subset.Count < InteractableConfig.InteractableCount)
                    Debug.Log("WARNING: SiteBounds does not contain sufficent interactables.");

                siteCount++;
            }
        }
    }
}
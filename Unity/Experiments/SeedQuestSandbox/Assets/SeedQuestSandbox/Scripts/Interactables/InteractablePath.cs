using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.SeedEncoder;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractablePath {

        private static InteractablePath instance = null;

        public static InteractablePath Instance {
            get {
                if (instance == null)
                    instance = new InteractablePath();
                return instance;
            }
        }

        /// <summary> Path index for next interactable to complete on Path </summary>
        public int nextIndex = 0;

        /// <summary> List of Interactables which represent the Path of Interactables to Complete </summary>
        public List<Interactable> path = null;

        /// <summary> List of Interactables which represent the Path </summary>
        static public List<Interactable> Path {
            get { return Instance.path; }
        }

        /// <summary> Path Percent Complete based on ActionsPerGame </summary>
        static public float PercentComplete {
            get { return 100.0f * Instance.nextIndex / InteractableConfig.ActionsPerGame; }
        }

        /// <summary> Path Complete Status based on ActionsPerGame </summary>
        static public bool PathComplete  {
            get { return Instance.nextIndex >= InteractableConfig.ActionsPerGame; }
        }

        /// <summary> Path Level Complete based on based on ActionsPerSite </summary>
        static public bool PathLevelComplete {
            get { return Instance.nextIndex != 0 && Instance.nextIndex % InteractableConfig.ActionsPerSite == 0; }
        }

        /// <summary> Next Interactable to complete on Path </summary>
        static public Interactable NextInteractable
        {
            get
            {
                if (Instance.nextIndex < Instance.path.Count)
                    return Instance.path[Instance.nextIndex];
                else
                    return null;
            }
        }

        /// <summary> Checks if interactable is the NextInteractable to be completed for the Path </summary>
        static public bool isNextInteractable(Interactable interactable)
        {
            return interactable == NextInteractable;
        }

        /// <summary> Generated Path of Interactables from a Seed string </summary>
        static public void GeneratePathFromSeed(string seed) {
            SeedConverter converter = new SeedConverter();
            Instance.path = new List<Interactable>(converter.encodeSeed(seed));
        }

        static public InteractableID[] GetPathIDsFromSeed(string seed) {
            SeedConverter converter = new SeedConverter();
            return converter.getPathIDs(seed);
        }

        /// <summary> Add an interactable to Path </summary>
        static public void Add(Interactable interactable)
        {
            Instance.path.Add(interactable);
        }

        /// <summary> Clear Interactables from Path </summary>
        static public void Clear() {
            Instance.path.Clear();
        }

        /// <summary> Reset Path Index for NextInteractable to First Interactable in Path </summary>
        static public void ResetPath() {
            Instance.nextIndex = 0;
        }

        /// <summary> Increament NextIndex and Initialize NextInteractable in Path </summary>
        static public void GoToNextInteractable()
        {
            if (GameManager.Mode == GameMode.Rehearsal && NextInteractable == InteractableManager.ActiveInteractable) {
                InteractableLog.Add(NextInteractable, NextInteractable.ID.actionID);

                Instance.nextIndex++;

                if (PathLevelComplete)
                    InteractablePathManager.ShowLevelComplete = true;

                if(NextInteractable != null)
                    InitializeNextInteractable();
            }
        }

        /// <summary> Initialize Next Interactable with Hightlights and Setup PreviewUI </summary>
        static public void InitializeNextInteractable() {
            if (GameManager.Mode == GameMode.Rehearsal) {
                InteractableManager.UnHighlightAllInteractables();
                NextInteractable.HighlightInteractableDynamically(true);
                InteractablePreviewUI.SetPreviewObject(NextInteractable);
            }
        }
    }
}
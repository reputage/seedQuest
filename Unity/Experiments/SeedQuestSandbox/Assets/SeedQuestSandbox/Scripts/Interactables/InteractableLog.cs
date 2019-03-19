using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractableLogItem {
        public int siteIndex;
        public int interactableIndex;
        public int actionIndex; 

        public InteractableLogItem() {
            siteIndex = 0;
            interactableIndex = 0;
            actionIndex = 0;
        }

        public InteractableLogItem(Interactable _interactable, int _actionIndex) {
            siteIndex = _interactable.ID.siteID;
            interactableIndex = _interactable.ID.spotID;
            actionIndex = _actionIndex;
        }

        public int SiteIndex { get => siteIndex; }

        public int InteractableIndex { get => interactableIndex;  }

        public int ActionIndex { get => actionIndex; }
    }

    [System.Serializable]
    public class InteractableLog {

        private static InteractableLog instance = null;

        public static InteractableLog Instance {
            get {
                if (instance == null)
                    instance = new InteractableLog();
                return instance;
            }
        }

        /// <summary> List of Interactables Completed in Recall Mode </summary>
        public List<InteractableLogItem> log = new List<InteractableLogItem>();

        /// <summary>  List of Interactables Completed in Recall Mode </summary>
        static public List<InteractableLogItem> Log {
            get { return Instance.log; }
        }

        /// <summary> Path Percent Complete based on ActionsPerGame </summary>
        static public float PercentComplete {
            get { return 100.0f * Instance.log.Count / InteractableConfig.ActionsPerGame; }
        }

        /// <summary> Path Complete Status based on ActionsPerGame </summary>
        static public bool PathComplete {
            get { return Instance.log.Count >= InteractableConfig.ActionsPerGame; }
        }

        /// <summary> Path Level Complete based on based on ActionsPerSite </summary>
        static public bool PathLevelComplete {
            get { return Instance.log.Count != 0 && Instance.log.Count % InteractableConfig.ActionsPerSite == 0; }
        }

        /// <summary> Add an Interactable to Log </summary>
        static public void Add(Interactable interactable, int actionIndex) {
            Instance.log.Add(new InteractableLogItem(interactable, actionIndex));

            if (GameManager.Mode == GameMode.Recall && PathLevelComplete)
                InteractablePathManager.ShowLevelComplete = true;
        }

        /// <summary> Removes the last action from log </summary>
        static public void UndoLastAction()
        {
            int count = Instance.log.Count;
            if (count == 0)
                return;

            Instance.log.RemoveAt(count - 1);
        }

        /// <summary> Clear all Interactables from Log </summary>
        static public void Clear () {
            Instance.log.Clear();
        }
    }
} 
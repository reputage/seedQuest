using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractableLogItem {
        public Interactable interactable;
        public int actionIndex; 

        public InteractableLogItem(Interactable _interactable, int _actionIndex) {
            interactable = _interactable;
            actionIndex = _actionIndex;
        }

        public int SiteIndex { get { return interactable.ID.siteID; } }

        public int InteractableIndex { get { return interactable.ID.spotID; } }

        public int ActionIndex { get { return actionIndex; } }
    }

    public class InteractableLog {

        private static InteractableLog instance = null;
        public static InteractableLog Instance
        {
            get
            {
                if (instance == null)
                    instance = new InteractableLog();

                return instance;
            }
        }

        public List<InteractableLogItem> log = new List<InteractableLogItem>();

        static public List<InteractableLogItem> Log {
            get { return Instance.log; }
        }

        static public void Add(Interactable interactable, int actionIndex) {
            Instance.log.Add(new InteractableLogItem(interactable, actionIndex));
        }

        static public void Clear () {
            Instance.log.Clear();
        }
    }
} 
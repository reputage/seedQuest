using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public List<Interactable> path = null;
        public int nextIndex = 0;

        static public List<Interactable> Path {
            get { return Instance.path;  }
        }
        
        static public void Add(Interactable interactable)
        {
            Instance.path.Add(interactable);
        }

        static public void Clear() {
            Instance.path.Clear();
        }

        static public Interactable NextInteractable {
            get {
                if (Instance.nextIndex < Instance.path.Count)
                    return Instance.path[Instance.nextIndex];
                else
                    return null;
            }
        }
    }
}
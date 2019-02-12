using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Interactables {

    public class InteractablePathManager : MonoBehaviour
    {
        private static InteractablePathManager instance = null;
        public static InteractablePathManager Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<InteractablePathManager>();

                if (instance == null)
                    instance = GameManager.Instance.gameObject.AddComponent<InteractablePathManager>();

                return instance;
            }
        }

        public List<Interactable> path;
        public Interactable next;
        private bool isNextHighlighted = false;

        private void Awake() {
            InteractablePath.GeneratePathFromSeed("9876543210");
            path = InteractablePath.Path;
        }

        private void Update() {
            if (GameManager.Mode == GameMode.Rehearsal) {

                if (!isNextHighlighted) {  
                    InteractablePath.InitializeNextInteractable();
                    isNextHighlighted = true;
                }

                next = InteractablePath.NextInteractable;
                if(next == null) {
                    GameManager.State = GameState.End;
                }
            }
        } 

        private List<InteractableID> getRandomPathIDs() {
            List<InteractableID> ids = new List<InteractableID>();

            for(int i = 0; i < InteractableConfig.SiteCount; i++)  {
                int levelIndex = Random.Range(0, InteractableConfig.SiteCount);

                for (int j = 0; j < InteractableConfig.ActionCount; i++)  {
                    int interactableIndex = Random.Range(0, InteractableConfig.InteractableCount);
                    int actionIndex = Random.Range(0, InteractableConfig.ActionCount);
                    InteractableID id = new InteractableID(levelIndex, interactableIndex, actionIndex);
                    ids.Add(id);
                }
            }

            return ids;
        }
    }
}
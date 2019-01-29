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

        private void Awake() { 
           
        }
        
        private List<InteractableID> getRandomPathIDs()
        {
            List<InteractableID> ids = new List<InteractableID>();

            for(int i = 0; i < InteractableConfig.LevelCount; i++)  {
                int levelIndex = Random.Range(0, InteractableConfig.LevelCount);

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
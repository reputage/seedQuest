using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.SeedEncoder;
using SeedQuest.Utils;

namespace SeedQuest.Interactables {

    [System.Serializable]
    public class InteractableSiteBounds {
        public Vector3 center;
        public Vector3 size;
        public bool debugShow = false;
    }

    public class InteractablePathManager : MonoBehaviour
    {
        static InteractablePathManager instance = null;

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

        public List<InteractableLogItem> log;

        public string seedString;

        public static string SeedString { 
            get { return Instance.seedString; }
        }

        private bool isNextHighlighted = false;

        public int siteIDOffset = 0;
        public List<InteractableSiteBounds> siteBounds; 

        private void Awake() {
            seedString = "EB204654C9";
            //seedString = RandomUtils.GetRandomHexNumber(10);

            InitalizePathAndLog();
            SetupInteractableIDs();
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
                    EndGameUI.ToggleOn();
                }
            }
            else if(GameManager.Mode == GameMode.Recall) {
                if(InteractableLog.Log.Count == InteractableConfig.SitesPerGame * InteractableConfig.ActionsPerSite) {
                    SeedConverter converter = new SeedConverter();
                    seedString = converter.DecodeSeed();
                    GameManager.State = GameState.End;
                    EndGameUI.ToggleOn();
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

        static public void SetupInteractableIDs() {
            Interactable[] list = InteractableManager.InteractableList;

            int[] indices = RandomUtils.GetRandomIndexArray(InteractableConfig.InteractableCount);

            int siteCount = Instance.siteIDOffset;
            foreach (InteractableSiteBounds bounds in Instance.siteBounds) {

                // Create a subset of interactables in bounds
                List<Interactable> subset = new List<Interactable>();
                foreach(Interactable item in list) {
                    if(InteractableInBounds(item, bounds)) {
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

                siteCount++;
            }
        }

        static bool InteractableInBounds(Interactable item, InteractableSiteBounds bounds) {
            Vector3 pos = item.transform.position;
            float x0 = bounds.center.x - (bounds.size.x / 2.0f);
            float x1 = bounds.center.x + (bounds.size.x / 2.0f);
            float z0 = bounds.center.z - (bounds.size.z / 2.0f);
            float z1 = bounds.center.z + (bounds.size.z / 2.0f);
            return x0 <= pos.x && pos.x <= x1 && z0 <= pos.z && pos.z <= z1;
        }

        private void OnDrawGizmos() {
            Color[] colors = new Color[6];
            colors[0] = Color.red;
            colors[1] = Color.cyan;
            colors[2] = Color.green;
            colors[3] = new Color(255, 165, 0);
            colors[4] = Color.yellow;
            colors[5] = Color.magenta;

            int count = 0; 
            foreach (InteractableSiteBounds bounds in siteBounds) {
                Gizmos.color = colors[count];
                Gizmos.DrawWireCube(bounds.center, bounds.size);
                count++;
            }
        }
    }
}
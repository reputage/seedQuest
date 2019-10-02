using UnityEngine;
using System.Linq;

namespace SeedQuest.Interactables
{
    public class InteractableManager : MonoBehaviour
    {
        static private InteractableManager instance = null;

        static public InteractableManager Instance {
            get {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<InteractableManager>();
                return instance;
            }
        }

        public float nearDistance = 4.0f;
        public GameObject interactableLabelUI;
        public GameObject[] actionSpotIcons; // InteractableUI Prefab Templates
        public Interactable activeInteractable = null;

        public bool useSingleTracker = true;
        public bool useInteractableNames = true;
        public bool useSeparatedUI = false;

        static public Interactable ActiveInteractable {
            get { return Instance.activeInteractable; }
        }

        private void Awake() {
            //InitalizeLookUp();
        }

        private void Update() {
            ListenForKey();
        }

        private void ListenForKey() {
            if (InputManager.GetKeyDown(KeyCode.P)) {
                InteractablePreviewUI.ToggleShow();
            }
        }

        static public void SetActiveInteractable(Interactable interactable) {
            Instance.activeInteractable = interactable;

            if ((GameManager.Mode == GameMode.Sandbox || GameManager.Mode == GameMode.Recall) && interactable != null)
                InteractablePreviewUI.SetPreviewObject(interactable, InteractablePath.Instance.actionIds[InteractablePath.Instance.nextIndex]); 
        }

        private Interactable[,] interactableLUT;

        static public Interactable[] InteractableList {
            get { return FindAllInteractables(); }
        }

        static int CompareInteractableName(Interactable inter1, Interactable inter2) {
            return inter1.gameObject.name.CompareTo(inter1.gameObject.name);
        }

        static Interactable[] FindAllInteractables()
        {
            Interactable[] items = GameObject.FindObjectsOfType<Interactable>();
            System.Array.Sort(items, CompareInteractableName);
            return items;
        }

        static public void ResetLabelTrackers()
        {
            Interactable[] interactables = GameObject.FindObjectsOfType<Interactable>();
            foreach (Interactable interactable in interactables)
            {
                interactable.ResetInteractableLabelTrackerIcon();
            }
        }

        static public void Reset() {
            destroyInteractables();
        }

        static public void destroyInteractables() {
            foreach (Interactable interactable in FindAllInteractables()) {
                interactable.Delete();
            }
        }

        /// <summary> Initalize LookUp Table for querying interactable based on siteID and spotID  </summary>
        static public void InitalizeLookUp()
        {
            Interactable[] interactables = InteractableManager.InteractableList;

            Instance.interactableLUT = new Interactable[InteractableConfig.SiteCount, InteractableConfig.InteractableCount];
            for (int i = 0; i < interactables.Length; i++)
            {
                int row = interactables[i].ID.siteID;
                int col = interactables[i].ID.spotID;
                Instance.interactableLUT[row, col] = interactables[i];
            }
        }

        static public Interactable IDtoInteractable(InteractableID id)
        { 
            return Instance.interactableLUT[id.siteID, id.spotID];
        }

    }
}
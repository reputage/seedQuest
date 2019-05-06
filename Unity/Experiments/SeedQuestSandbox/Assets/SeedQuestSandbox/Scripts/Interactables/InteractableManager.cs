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

        public float nearDistance = 2.0f;

        public GameObject[] actionSpotIcons; // InteractableUI Prefab Templates

        public Interactable activeInteractable = null;

        static public Interactable ActiveInteractable {
            get { return Instance.activeInteractable; }
        }

        private void Awake()
        {
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

        static public void SetActiveInteractable(Interactable interactable)
        {
            Instance.activeInteractable = interactable;
            interactable.HighlightInteractable(true);

            if ((GameManager.Mode == GameMode.Sandbox || GameManager.Mode == GameMode.Recall) && interactable != null)
                InteractablePreviewUI.SetPreviewObject(interactable); 
        }

        private Interactable[,] interactableLUT;

        static public Interactable[] InteractableList
        {
            get { return FindAllInteractables(); }
        }

        static int CompareInteractableName(Interactable inter1, Interactable inter2)
        {
            return inter1.gameObject.name.CompareTo(inter1.gameObject.name);
        }

        static Interactable[] FindAllInteractables()
        {
            Interactable[] items = GameObject.FindObjectsOfType<Interactable>();
            System.Array.Sort(items, CompareInteractableName);
            return items;
        }

        static public void Reset() {
            destroyInteractables();
        }

        static public void destroyInteractables() {
            foreach (Interactable interactable in FindAllInteractables()) {
                interactable.Delete();
            }
        }

        static public void deleteAllInteractableUI() {
            foreach (Interactable interactable in FindAllInteractables())
                interactable.DeleteUI(); 
        }

        static void findNearInteractables()
        {
            Interactable[] list = FindObjectsOfType<Interactable>();

            foreach (Interactable item in list)
            {
                Vector3 playerPosition = PlayerCtrl.PlayerTransform.position;
                float dist = (item.transform.position - playerPosition).magnitude;
                if (dist < Instance.nearDistance)
                    doNearInteractable(true);
                else
                    doNearInteractable(false);
            }
        }

        static public void resetInteractableUIText() {
            foreach (Interactable interactable in FindAllInteractables())
                interactable.interactableUI.SetText(interactable.Name);
        }

        /// <summary> Hides all UI Canvas for Interactables </summary>
        static public void hideAllInteractableUI()
        {
            foreach(Interactable interactable in FindAllInteractables()) {
                interactable.interactableUI.hideActions();
            }
        }

        static void doNearInteractable(bool isNear)
        {

        }

        static public void HighlightAllInteractables() {
            foreach(Interactable interactable in FindAllInteractables()) 
                interactable.HighlightInteractable(true);
        }

        static public void UnHighlightAllInteractables()
        {
            foreach (Interactable interactable in FindAllInteractables())
                interactable.HighlightInteractable(false);
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
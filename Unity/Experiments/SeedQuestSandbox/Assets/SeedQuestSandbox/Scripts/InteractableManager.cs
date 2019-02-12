using UnityEngine;

namespace SeedQuest.Interactables
{
    public class InteractableManager : MonoBehaviour
    {
        static private InteractableManager instance = null;

        static public InteractableManager Instance
        {
            get
            {
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
            InitalizeLookUp();
        }

        static public void SetActiveInteractable(Interactable interactable)
        {
            Instance.activeInteractable = interactable;
            if(GameManager.Mode == GameMode.Sandbox && interactable != null)
                InteractablePreviewUI.SetPreviewObject(interactable); 
           
        }

        private Interactable[,] interactableLUT;

        static public Interactable[] InteractableList
        {
            get { return FindAllInteractables(); }
        }

        static Interactable[] FindAllInteractables()
        {
            return GameObject.FindObjectsOfType<Interactable>();
        }

        static public void destroyInteractables()
        {
            foreach (Interactable interactable in FindAllInteractables())
                GameObject.Destroy(interactable.gameObject);

            foreach (GameObject interactableUI in GameObject.FindGameObjectsWithTag("InteractableUI"))
                GameObject.Destroy(interactableUI);
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

        /// <summary> Hides all UI Canvas for Interactables </summary>
        static public void hideAllInteractableUI()
        {
            foreach(Interactable interactable in FindAllInteractables())
                interactable.interactableUI.hideActions();
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

        static public ParticleSystem getEffect()
        {
            ParticleSystem effect;

            InteractableStateData data = Instance.activeInteractable.stateData;
            if (data == null)
                effect = EffectsManager.createEffect(Instance.activeInteractable.transform);
            else if (data.effect == null)
                effect = EffectsManager.createEffect(Instance.activeInteractable.transform);
            else
                effect = EffectsManager.createEffect(Instance.activeInteractable.transform, data.effect);

            return effect;
        }

        /// <summary> Do Interaction - Does Action, actives effect, logs action, updates path (for rehersal) and exits interactable ui dialog </summary>
        static public void doInteractableAction(int actionIndex)
        {
            /*
            ParticleSystem effect = getEffect();
            effect.Play();

            Instance.activeItem.doAction(actionIndex);
            InteractableManager.Log.Add(Instance.activeItem, actionIndex);
            Instance.activeItem = null;
            GameManager.State = GameManager.PrevState;
            PathManager.NextPathSegment();
            */
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
using UnityEngine;

namespace SeedQuest.Interactables
{
    public class InteractableManager : MonoBehaviour
    {

        public float nearDistance = 2.0f;
        public GameObject[] actionSpotIcons;
        private Interactable activeItem = null;
        public Interactable[] list = null;

        static public Interactable[] InteractableList
        {
            get { return findAllInteractables(); }
        }

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

        static Interactable[] findAllInteractables()
        {
            return GameObject.FindObjectsOfType<Interactable>();
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

        /// <summary>
        /// Hides all UI Canvas for Interactables 
        /// </summary>
        static public void hideAllInteractableUI()
        {
            foreach(Interactable interactable in findAllInteractables())
                interactable.interactableUI.hideActions();
        }

        static void doNearInteractable(bool isNear)
        {

        }

        static public ParticleSystem getEffect()
        {
            ParticleSystem effect;

            InteractableStateData data = Instance.activeItem.stateData;
            if (data == null)
                effect = EffectsManager.createEffect(Instance.activeItem.transform);
            else if (data.effect == null)
                effect = EffectsManager.createEffect(Instance.activeItem.transform);
            else
                effect = EffectsManager.createEffect(Instance.activeItem.transform, data.effect);

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

    }
}
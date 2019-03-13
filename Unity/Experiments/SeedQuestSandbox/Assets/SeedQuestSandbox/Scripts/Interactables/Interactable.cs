using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractableHighlightsProps {
        public bool useHighlightsShader = true;
    }

    //[ExecuteInEditMode] 
    [RequireComponent(typeof(BoxCollider))]
    public class Interactable : MonoBehaviour {

        public InteractableStateData stateData = null;
        public InteractableUI interactableUI;
        public InteractableTrackerProps interactableTracker;
        public InteractablePreviewInfo interactablePreview;
        public InteractableHighlightsProps interactableHighlights;
        public InteractableID ID;
        public int currentStateID = 0;

        [HideInInspector]
        public float interactDistance = 2.0f;
        private bool isOnHover = false;
        [HideInInspector]
        public bool flagDeleteUI = false;

        void Start() {
            interactableUI.Initialize(this);
        }

        void Update()  {
            if (interactableUI.isReady()) {
                interactableUI.Update();
                HoverOnInteractable();
                ClickOnInteractable();
            }
            else {
                interactableUI.Initialize(this);
            }
        }

        void OnDestroy() {
            DeleteUI();
        }
        
        public string Name {
            get {
                if (interactableUI.name != "")
                    return interactableUI.name;
                else if (stateData != null)
                    return stateData.interactableName;
                else
                    return "Error: Missing StateData/Name";
            }
        }

        public string RehearsalActionName  {
            get {
                return (stateData == null) ? "Action #" + ID.actionID : this.stateData.getStateName(ID.actionID);
            }
        }

        public bool IsNextInteractable { get => InteractablePath.NextInteractable == this; }

        int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public void Delete() {
            flagDeleteUI = true;
            interactableUI.DeleteUI();
            GameObject.Destroy(gameObject);
        }

        public void DeleteUI() {
            flagDeleteUI = true;
            interactableUI.DeleteUI();
        }

        public void NextAction()
        {
            currentStateID = Mod(currentStateID + 1, 4);
            InteractableState state = stateData.states[currentStateID];
            state.enterState(this);
            HighlightInteractable(true, true);
            interactableUI.SetActionUI(currentStateID);
        }

        public void PrevAction()
        {
            currentStateID = Mod(currentStateID - 1, 4);
            InteractableState state = stateData.states[currentStateID];
            state.enterState(this);
            HighlightInteractable(true, true);
            interactableUI.SetActionUI(currentStateID);
        }

        public void DoAction(int actionIndex)
        {
            currentStateID = actionIndex;
            InteractableState state = stateData.states[actionIndex];
            state.enterState(this);
            HighlightInteractable(true, true);
            interactableUI.SetActionUI(actionIndex);
        }

        public void SelectAction(int actionIndex)
        {
            InteractableLog.Add(this, actionIndex);
        }

        private bool PlayerIsNear()
        {
            Vector3 playerPosition = PlayerCtrl.PlayerTransform.position;
            float dist = (transform.position - playerPosition).magnitude;
            if (dist < interactDistance)
                return true;
            else
                return false;
        }

        public void HoverOnInteractable()
        {
            if (PauseManager.isPaused == true)
                return;

            Camera c = Camera.main;
            RaycastHit hit;
            Ray ray = c.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();
                
                if (hitThisInteractable)
                { 
                    interactableUI.showCurrentActions();

                    if (!isOnHover)  {
                        GameManager.State = GameState.Interact;
                        InteractableManager.SetActiveInteractable(this);
                    } 

                    isOnHover = true;
                }
                else {
                    if (isOnHover) {
                        GameManager.State = GameState.Play;

                        if (IsNextInteractable)
                            HighlightInteractable(true, true);
                        else
                            HighlightInteractable(false);
                    }

                    isOnHover = false;
                }
            }
        }

        public void ClickOnInteractable() {
            if (PauseManager.isPaused == true)
                return;

            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();

                    if (hitThisInteractable)
                    {
                        NextAction();
                    }
                }
            }
        }

        public void HighlightInteractable(bool useHighlight) {
            HighlightInteractable(useHighlight, false);
        }

        public void HighlightInteractable(bool useHighlight, bool useDynamicRim) {
            if (!interactableHighlights.useHighlightsShader)
                return;

            Shader defaultShader = Shader.Find("Standard");
            Shader highlightShader = Shader.Find("SeedQuest/RimOutline");

            Renderer[] rendererList = transform.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in rendererList) {

                if (renderer.GetComponent<ParticleSystem>() != null)
                    continue;

                foreach (Material material in renderer.materials) {
                    if (useHighlight) {
                        material.shader = highlightShader;

                        material.SetFloat("_RimPower", 2.0f);

                        if(useDynamicRim)
                            material.SetFloat("_UseDynamicRim", 1.0f);
                        else
                            material.SetFloat("_UseDynamicRim", 0.0f);
                    }
                    else
                        material.shader = defaultShader;
                }
            }
        }

        public void HighlightInteractableWithEffect(bool useHighlight) {
            HighlightInteractable(true, true);

            if (useHighlight)
                EffectsManager.PlayEffect("highlight", this.transform);
            else
                EffectsManager.StopEffect(this.transform);
        }
    }
}
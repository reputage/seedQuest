using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SeedQuest.Interactables
{
    //[ExecuteInEditMode] 
    [RequireComponent(typeof(BoxCollider))]
    public class Interactable : MonoBehaviour {

        public InteractableStateData stateData = null;
        public InteractableUI interactableUI;
        public InteractablePreviewInfo interactablePreview;  
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

            interactableUI.SetActionUI(currentStateID);
            //interactableUI.SetText(state.actionName);
        }

        public void PrevAction()
        {
            currentStateID = Mod(currentStateID - 1, 4);
            InteractableState state = stateData.states[currentStateID];
            state.enterState(this);

            interactableUI.SetActionUI(currentStateID);
            //interactableUI.SetText(state.actionName);
        }

        public void DoAction(int actionIndex)
        {
            currentStateID = actionIndex;
            InteractableState state = stateData.states[actionIndex];
            state.enterState(this);

            interactableUI.SetActionUI(actionIndex);
            //interactableUI.SetText(state.actionName);
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
                    }

                    isOnHover = false;
                }
            }
        }

        public void ClickOnInteractable()
        {
            if (PauseManager.isPaused == true)
                return;

            if (Input.GetMouseButtonDown(0))
            {
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

        public void HighlightInteractableDynamically(bool useHighlight) {
            Shader defultShader = Shader.Find("Standard");
            Shader highlightShader = Shader.Find("SeedQuest/RimOutline");

            Renderer rend = transform.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                foreach (Material material in rend.materials)
                {
                    if (useHighlight)
                        material.shader = highlightShader;
                    else
                        material.shader = defultShader;
                }
            }

            EffectsManager.PlayEffect("highlight", this.transform);
        }

        public void HighlightInteractable(bool useHighlight) {
            Shader defultShader = Shader.Find("Standard");
            Shader highlightShader = Shader.Find("SeedQuest/RimOutline");

            Renderer rend = transform.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                foreach (Material material in rend.materials)
                {
                    if (useHighlight)
                    {
                        material.shader = highlightShader;
                        material.SetFloat("_UseDynamicRim", 0.0f);
                    }
                    else
                        material.shader = defultShader;
                }
            }
        }

    }
}
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

        // Use this for initialization
        void Start()
        {
            interactableUI.Initialize(this);
        }

        void Update()
        {
            if (interactableUI.isReady())
            {
                interactableUI.Update();
                HoverOnInteractable();
                ClickOnInteractable();
            }
        }

        int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public void NextAction()
        {
            currentStateID = Mod(currentStateID + 1, 4);
            InteractableState state = stateData.states[currentStateID];
            state.enterState(this);
            interactableUI.SetText(state.actionName);
        }

        public void PrevAction()
        {
            currentStateID = Mod(currentStateID - 1, 4);
            InteractableState state = stateData.states[currentStateID];
            state.enterState(this);
            interactableUI.SetText(state.actionName);
        }

        public void DoAction(int actionIndex)
        {
            InteractableState state = stateData.states[actionIndex];
            state.enterState(this);
            interactableUI.SetText(state.actionName);
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

                        //toggleHighlight(true);
                    }
                    isOnHover = true;

                    /*
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                        InteractableManager.showActions(this);
                        */
                }
                else {
                    if (isOnHover) {
                        GameManager.State = GameState.Sandbox;
                        //toggleHighlight(false);
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

        public void startEffect()
        {
            ParticleSystem effect = InteractableManager.getEffect();
            effect.Play();
        }

        public void toggleHighlight(bool highlight)
        {
            Renderer rend = transform.GetComponent<Renderer>();
            if (rend == null)
                return;

            Shader shaderDefault = Shader.Find("Standard");
            Shader shader = Shader.Find("Custom/Outline + Rim");

            Material[] materials = rend.materials;
            for (int i = 0; i < materials.Length; i++)
            {

                if (highlight)
                    rend.materials[i].shader = shader;
                else
                    rend.materials[i].shader = shaderDefault;
            }
        }

        /*
        public void HighlightPathTarget() {
            if (GameManager.State != GameState.Rehearsal)
                return;

            if (PathManager.PathTarget == this)
                toggleHighlight(true);
            else if(!isOnHover)
                toggleHighlight(false);
        }
        */

        public string getInteractableName()
        {
            if (stateData == null)
                return "Interactable Name";
            else
                return this.stateData.interactableName;
        }

        public string Name
        {
            get { return getInteractableName(); }
        }

        public string getStateName(int index)
        {
            if (stateData == null)
                return "Action #" + index;
            else
                return this.stateData.getStateName(index);
        }

        public string RehersalActionName
        {
            get { return getStateName(ID.actionID); }
        }

        public int getStateCount()
        {
            if (stateData == null)
                return 0;
            else
                return this.stateData.states.Count;
        }

    }
}
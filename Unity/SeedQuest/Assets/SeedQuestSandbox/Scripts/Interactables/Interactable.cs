using UnityEngine;
using UnityEngine.EventSystems;

namespace SeedQuest.Interactables 
{
    [System.Serializable]
    public class InteractableCameraProps {
        public Vector3 lookAtOffset = Vector3.zero;
        public Vector3 positionOffset = Vector3.zero;
        public float zoomDistance = 10.0f;
    }

    [RequireComponent(typeof(BoxCollider))]
    public class Interactable : MonoBehaviour {

        public InteractableStateData stateData = null;
        public InteractableUI interactableUI;
        public InteractableCameraProps interactableCamera;
        public InteractablePreviewInfo interactablePreview;
        public InteractableID ID;

        private InteractableLabelUI interactableLabel;

        private int actionIndex = -1;
        public int ActionIndex { get => actionIndex; set => actionIndex = value; } // Current Action State 
        private float interactDistance = 16.0f;  

        private bool isOnHover = false;
        public bool IsOnHover { get => isOnHover; } 

        void Start() {
            interactableLabel = new InteractableLabelUI();
            interactableLabel.Initialize(this);
        }

        void Update()  {
            interactableLabel.Update();
            ClickOnInteractable();
            HoverOnInteractable();
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

        public Vector3 LookAtPosition { get => transform.position + GetComponent<BoxCollider>().center + interactableCamera.lookAtOffset; }

        public Vector3 LabelPosition { get => interactableLabel.LabelPosition; }

        public string GetActionName(int actionIndex) {
            return this.stateData.getStateName(actionIndex);
        }

        public void Delete() {
            GameObject.Destroy(gameObject);
        }

        /// <summary> Shows previews interactable action. </summary>
        /// <param name="actionIndex">Action index</param>
        public void PreviewAction(int actionIndex)  {
            this.actionIndex = actionIndex;
            InteractableState state = stateData.states[actionIndex];
            stateData.stopAudio();
            state.enterState(this);

            if (GameManager.Mode == GameMode.Sandbox || GameManager.Mode == GameMode.Recall)
                InteractablePreviewUI.SetPreviewAction(this.actionIndex);
        }

        /// <summary>
        /// Selects the action based on <paramref name="actionIndex"/> and goes to next interactable if 
        /// in rehersal mode. 
        /// </summary>
        /// <param name="actionIndex">Action index.</param>
        public void SelectAction(int actionIndex) {
            if (GameManager.Mode == GameMode.Rehearsal) {
                bool isNextAction = this.ID == InteractablePath.NextInteractable.ID && actionIndex == InteractablePath.NextAction;

                if (isNextAction) { 
                    InteractableManager.SetActiveInteractable(this);
                    InteractableLog.Add(this, actionIndex);
                    InteractablePath.GoToNextInteractable();
                }
            }
            else
                InteractableLog.Add(this, actionIndex);
        }

        public bool PlayerIsNear() {
            Vector3 playerPosition = IsometricCamera.instance.playerTransform.position;
            float dist = (transform.position - playerPosition).magnitude;
            if (dist < interactDistance)
                return true;
            else
                return false;
        }

        public void SetInteractableLabelTrackerIcon() {
            foreach (Interactable i in InteractableManager.InteractableList) {
                if (i.interactableLabel != null)
                    i.interactableLabel.ToggleTrackerIcon(false);
            }

            if (interactableLabel != null)
                interactableLabel.ToggleTrackerIcon(true);
        }

        public void ResetInteractableLabelTrackerIcon()
        {
            interactableLabel.ToggleTrackerIcon(false);
        }

        public void HoverOnInteractable() {
            if (PauseManager.isPaused == true)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f)) {
                bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();
                
                if (hitThisInteractable) { 
                    if (!isOnHover)  {
                        interactableLabel.OnHoverEnter();
                    } 

                    isOnHover = true;
                }
                else {
                    if (isOnHover) {
                        interactableLabel.OnHoverExit();
                    }

                    isOnHover = false;
                }
            }
        }

        public void ClickOnInteractable() {
            if (PauseManager.isPaused == true) return;

            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f)) {
                    bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();

                    if (hitThisInteractable) {
                        interactableLabel.ActivateInteractable();
                    }
                }
            }

        }

        void OnDrawGizmos() {
            
            // Display the interactable label show radius when player enters radius
            if(PlayerIsNear()) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(LookAtPosition, interactDistance);
            }
        }
    }
}
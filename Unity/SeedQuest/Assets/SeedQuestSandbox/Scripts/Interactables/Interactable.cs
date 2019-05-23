using UnityEngine;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractableHighlightsProps {
        public bool useHighlightsShader = true;
        public Color highlightColor = Color.white;
        public float highlightPower = 0.2f;
        public Color rimColor = Color.white;
        public float rimExponent = 3.0f;
        public float rimPower = 0.6f;
        public Color outlineColor = Color.white;
        public float outlineWidth = 0.02f;
        public float outlinePower = 0.2f;
        public float dynamicFlashSpeed = 0.5f;
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
        
        private int actionIndex = -1;
        public int ActionIndex { get => actionIndex; set => actionIndex = value; } // Current Action State 

        [HideInInspector]
        public float interactDistance = 2.0f;

        private bool isOnHover = false;
        public bool IsOnHover { get => isOnHover; } 

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

        int Mod(int x, int m) {
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

        public void NextAction() {
            actionIndex = (actionIndex == -1) ? 0 : Mod(actionIndex + 1, 4);
            DoAction(actionIndex);
        }

        public void PrevAction() {
            actionIndex = (actionIndex == -1) ? (4-1) : Mod(actionIndex - 1, 4);
            DoAction(actionIndex);
        }

        public void DoAction(int actionIndex)  {
            this.actionIndex = actionIndex;
            InteractableState state = stateData.states[actionIndex];
            state.enterState(this);
            HighlightInteractable(true, true);
            interactableUI.SetActionUI(actionIndex);

            if (GameManager.Mode == GameMode.Sandbox || GameManager.Mode == GameMode.Recall)
                InteractablePreviewUI.SetPreviewAction(this.actionIndex);
        }

        public void SelectAction(int actionIndex) {
            InteractableLog.Add(this, actionIndex);
        }

        private bool PlayerIsNear() {
            Vector3 playerPosition = PlayerCtrl.PlayerTransform.position;
            float dist = (transform.position - playerPosition).magnitude;
            if (dist < interactDistance)
                return true;
            else
                return false;
        }

        public void HoverOnInteractable() {
            if (PauseManager.isPaused == true)
                return;

            Camera c = Camera.main;
            RaycastHit hit;
            Ray ray = c.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f)) {
                bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();
                
                if (hitThisInteractable) { 
                    interactableUI.showCurrentActions();

                    if (!isOnHover)  {
                        GameManager.State = GameState.Interact;
                        AudioManager.Play("UI_Hover");
                        InteractableManager.SetActiveInteractable(this, this.ActionIndex);
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

        int mouseDownICount = 0;
        public void ClickOnInteractable() {
            if (PauseManager.isPaused == true)
                return;

            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f)) {
                    bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();

                    if (hitThisInteractable) {
                        interactableUI.StartProgress();
                        AudioManager.Play("UI_Click");
                        mouseDownICount = InteractableLog.Count;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    bool hitThis = hit.transform.GetInstanceID() == transform.GetInstanceID();
                    if (!hitThis)
                        return;

                    bool progressIsSmall = interactableUI.ProgressTime < 0.25f;
                    interactableUI.CheckProgress();

                    if(mouseDownICount == InteractableLog.Count && progressIsSmall)
                        NextAction();
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

                        material.SetFloat("_HighlightPower", interactableHighlights.highlightPower);
                        material.SetFloat("_RimExponent", interactableHighlights.rimExponent);
                        material.SetFloat("_RimPower", interactableHighlights.rimPower);
                        material.SetFloat("_OutlineWidth", interactableHighlights.outlineWidth);
                        material.SetFloat("_OutlinePower", interactableHighlights.outlinePower);
                        material.SetFloat("_DynamicColorSpeed", interactableHighlights.dynamicFlashSpeed);

                        if(useDynamicRim)
                            material.SetFloat("_UseDynamicColor", 1.0f);
                        else
                            material.SetFloat("_UseDynamicColor", 0.0f);
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
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SeedQuest.Interactables
{
    public enum InteractableUIMode { NextPrevSelect, GridSelect, ListSelect, Dialogue };

    [System.Serializable]
    public class InteractableUI
    { 
        public string name = "";
        public int fontSize = 36;
        public float scaleSize = 1;
        public InteractableUIMode mode;
        public bool useRotateToCamera = true;
        public Vector3 rotationOffset = new Vector3(0, 0, 0);
        public Vector3 positionOffset = new Vector3(0, 0, 0);
        public GameObject debugActionUI = null;

        private Interactable parent;
        private GameObject actionUI = null;
        private Button labelButton;
        private Button[] actionButtons;
        private Button checkButton;
        private Image[] checkImages;
        private TMPro.TextMeshProUGUI actionUITextMesh;

        private ProgressButton progressButton;

        private bool dialogueSelected = false;

        public void Update() {
            if (isReady()) {
                SetScale();
                SetPosition();
                SetRotation();
            }
        }

        /// <summary> Initialize Interactable UI with Prompt Text and Buttons </summary>
        /// <param name="interactable">Parent Interactable Object</param>
        public void Initialize(Interactable interactable) {
            parent = interactable;

            if (interactable.flagDeleteUI)
                return;

            int modeIndex = 0;
            modeIndex = mode == InteractableUIMode.GridSelect ? 1 : modeIndex;
            modeIndex = mode == InteractableUIMode.ListSelect ? 2 : modeIndex;
            modeIndex = mode == InteractableUIMode.Dialogue ? 3 : modeIndex;

            Transform UIContainer;
            if (!GameObject.Find("InteractableUIContainer")) {
                UIContainer = new GameObject("InteractableUIContainer").transform;
                UIContainer.parent = InteractableManager.Instance.transform;
            }
            else  {
                UIContainer = GameObject.Find("InteractableUIContainer").transform;
            }

            actionUI = GameObject.Instantiate(InteractableManager.Instance.actionSpotIcons[modeIndex], UIContainer);
            debugActionUI = actionUI;

            initializeComponentRefs();
            SetScale();
            SetPosition();
            SetupLabel();
            SetupActionButtons();
            SetupCheckButton();

            if (parent == InteractablePath.NextInteractable)
                ToggleTracker(true);
        }

        /// <summary> Ready Status of InteractableUI </summary>
        public bool isReady() {
            return actionUI != null;
        }

        /// <summary> Delete UI Object </summary>
        public void DeleteUI() {
            GameObject.Destroy(actionUI);
        }

        public void initializeComponentRefs()
        {
            progressButton = actionUI.GetComponentInChildren<ProgressButton>();
            labelButton = actionUI.GetComponentInChildren<Button>();
            actionUITextMesh = actionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        }

        /// <summary> Intialize and Setupt Label Button </summary>
        public void SetupLabel() {
            var textList = actionUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            textList[0].text = parent.Name;

            labelButton.onClick.AddListener(delegate { onClickLabel(); });

            foreach (TMPro.TextMeshProUGUI text in textList)
                text.fontSize = fontSize;
        }

        /// <summary> Intialize and Setup Action Buttons </summary>
        public void SetupActionButtons() {
            Button[] buttons = actionUI.GetComponentsInChildren<Button>();

            if (mode == InteractableUIMode.NextPrevSelect)  {
                actionButtons = new Button[buttons.Length - 2];
            }
            else if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect || mode == InteractableUIMode.Dialogue) {
                actionButtons = new Button[buttons.Length - 1];
                checkImages = new Image[buttons.Length - 1];
            }

            System.Array.Copy(buttons, 1, actionButtons, 0, actionButtons.Length);

            if (mode == InteractableUIMode.NextPrevSelect) {
                actionButtons[0].onClick.AddListener(parent.NextAction);
                actionButtons[1].onClick.AddListener(parent.PrevAction);
            }

            else if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect || mode == InteractableUIMode.Dialogue) {
                for (int i = 0; i < 4; i++) {
                    var actionText = actionButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    actionText.text = parent.stateData.getStateName(i);
                    checkImages[i] = actionButtons[i].gameObject.GetComponentsInChildren<Image>()[1];
                }

                if (mode == InteractableUIMode.Dialogue) {
                    for (int i = 0; i < 4; i++) {
                        checkImages[i].gameObject.SetActive(false);
                    }
                    actionButtons[4].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = parent.stateData.getPrompt();
                }
                else {
                    foreach (Image image in checkImages) {
                        image.gameObject.SetActive(false);
                    }
                }

                actionButtons[0].onClick.AddListener(delegate { ClickActionButton(0); });
                actionButtons[1].onClick.AddListener(delegate { ClickActionButton(1); });
                actionButtons[2].onClick.AddListener(delegate { ClickActionButton(2); });
                actionButtons[3].onClick.AddListener(delegate { ClickActionButton(3); });
            }

            hideActions();
        }

        /// <summary> Setup Checkmark Button for use with NextPrevSelect Button only </summary>
        public void SetupCheckButton() {
            if (mode == InteractableUIMode.NextPrevSelect) {
                Button[] buttons = actionUI.GetComponentsInChildren<Button>();
                checkButton = buttons[1];
                checkButton.onClick.AddListener(onClickCheck);
                checkButton.gameObject.SetActive(false);
            }
        }
        
        /// <summary> Handles Clicking the Label Button </summary>
        public void onClickLabel() {
            //parent.NextAction();
        }

        /// <summary> Handles Clicking an Action Button </summary>
        public void ClickActionButton(int actionIndex) {
            parent.DoAction(actionIndex);

            if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect)
                hideActions();

            else if (mode == InteractableUIMode.Dialogue) {
                actionButtons[5].transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = actionButtons[actionIndex].GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
                dialogueSelected = true;
                hideActions();
            }

            if(GameManager.Mode == GameMode.Rehearsal) {
                if (actionIndex == InteractablePath.NextInteractable.ID.actionID)
                {
                    InteractableLog.Add(parent, parent.ActionIndex);
                    InteractablePath.GoToNextInteractable();
                }
            }
            else if (GameManager.Mode == GameMode.Recall)
                InteractableLog.Add(parent, parent.ActionIndex);
        }

        /// <summary> Handles Clicking a Checkmark Button </summary>
        public void onClickCheck() {
            SetCheckButtonActive(false);

            if (GameManager.Mode == GameMode.Rehearsal && 
                parent == InteractablePath.NextInteractable && 
                parent.ActionIndex == InteractablePath.NextAction) 
            {
                InteractableLog.Add(parent, parent.ActionIndex);
                InteractablePath.GoToNextInteractable();

                if (mode == InteractableUIMode.NextPrevSelect) {
                    progressButton.SetActive(false);
                } 
            }
            else if (GameManager.Mode == GameMode.Rehearsal){
                // put 'incorrect interactable' code here
            }
            else if (GameManager.Mode == GameMode.Recall)
                InteractableLog.Add(parent, parent.ActionIndex);

            /*
            string values = "";
            foreach (InteractableLogItem item in InteractableLog.Log)
            {
                values += item.siteIndex.ToString() + " " + item.interactableIndex.ToString() + " " + item.actionIndex.ToString() + " ";
            }
            Debug.Log("Current Log values: " + values);
            */
        }

        /// <summary> Sets Label Text to Current Action and Activates Checkmark if necessary </summary>
        public void SetActionUI(int actionIndex) {
            InteractableState state = parent.stateData.states[actionIndex];
            //SetText(parent.Name + ":\n "+ state.actionName);
            SetText(state.actionName);
            SetCheckmark(actionIndex);
        }

        /// <summary> Sets Label Text </summary>
        public void SetText(string text) {
            if (actionUI == null) return;
            actionUITextMesh.text = text;
        }

        /// <summary> Gets Label Text </summary>
        public string GetText() {
            return actionUITextMesh.text;
        }

        /// <summary> Show Action Button UI and Set Checkmark for Rehearsal Mode for ListSelect and GridSelect UI  </summary>
        public void showCurrentActions() {
            InteractableManager.hideAllInteractableUI();
            showActions();
            SetCheckImageActive();

            string label = GetText();
            InteractableManager.resetInteractableUIText();
            SetText(label);
        }

        /// <summary> Toogles Action Buttons </summary>
        public void toggleActions()  {
            bool isShown = actionButtons[0].gameObject.activeSelf;
            if (isShown)
                hideActions();
            else
                showActions();
        }

        /// <summary> Hide Action Button UI </summary>
        public void hideActions() {
            if (actionButtons == null) return;

            if (mode == InteractableUIMode.Dialogue) {
                for (int i = 0; i < 6; i++) {
                    if (i == 4) {
                        if (dialogueSelected) {
                            actionButtons[i].transform.localPosition = new Vector3(actionButtons[4].transform.localPosition.x, 125, 0);
                            actionButtons[i].transform.GetComponent<Image>().color = Color.gray;
                            actionButtons[5].transform.gameObject.SetActive(true);
                            return;
                        }
                        continue;
                    }
                    actionButtons[i].transform.gameObject.SetActive(false);
                }
            }

            else {
                foreach (Button button in actionButtons)
                    button.transform.gameObject.SetActive(false);
            }
        }

        /// <summary> Show Action Button UI </summary>
        public void showActions()  {
            if (actionButtons == null) return;

            if (mode == InteractableUIMode.Dialogue) {
                for (int i=0; i < 5; i++) {
                    actionButtons[i].transform.gameObject.SetActive(true);
                }

                if (dialogueSelected) {
                    dialogueSelected = false;
                    actionButtons[4].transform.localPosition = new Vector3(actionButtons[4].transform.localPosition.x, 70, 0);
                    actionButtons[4].transform.GetComponent<Image>().color = Color.white;
                    actionButtons[5].transform.gameObject.SetActive(false);
                }
            }
            else{
                foreach (Button button in actionButtons)
                    button.transform.gameObject.SetActive(true);
            }

        }

        /// <summary> Handles hovering over UI </summary>
        public void onHoverUI() {
            GameManager.State = GameState.Interact;
            showCurrentActions();
            InteractableManager.SetActiveInteractable(parent, parent.ActionIndex);
        }

        /// <summary> Handles exiting hovering UI </summary>
        public void offHoverUI() {
            GameManager.State = GameState.Play;
        }

        /// <summary> Sets UI Size Scale </summary>
        public void SetScale() {
            actionUI.GetComponent<RectTransform>().localScale = new Vector3(-0.01f * scaleSize, 0.01f * scaleSize, 0.01f * scaleSize);
        }


        public void SetFontSize() {
            var textList = actionUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            foreach (TMPro.TextMeshProUGUI text in textList)
                text.fontSize = fontSize;
        }

        /// <summary> Sets UI Position </summary>
        public void SetPosition() {
            Vector3 labelPositionOffset = Vector3.zero;
            if (parent.stateData != null) labelPositionOffset = parent.stateData.labelPosOffset;
            Vector3 position = parent.transform.position + labelPositionOffset + positionOffset;
            actionUI.GetComponent<RectTransform>().position = position;
        }

        /// <summary> Sets UI Rotation </summary>
        public void SetRotation()  {
            if (useRotateToCamera) {
                BillboardInteractable();
                actionUI.GetComponent<RectTransform>().Rotate(rotationOffset);
            }
            else {
                actionUI.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotationOffset);
            }
        }

        /// <summary> Sets Billboarding for UI i.e. so UI follows camera </summary>
        public void BillboardInteractable() {
            Vector3 targetPosition = Camera.main.transform.position - (100 * Camera.main.transform.forward ) ;
            Vector3 interactablePosition = actionUI.transform.position;
            Vector3 lookAtDir = targetPosition - interactablePosition;

            Quaternion rotate = Quaternion.LookRotation(lookAtDir);
            actionUI.transform.rotation = rotate;
        }

        /// <summary> Activates Checkmark Button for use with NextPrevSelect </summary>
        private void SetCheckButtonActive(bool active) {
            if (mode == InteractableUIMode.NextPrevSelect) {
                //checkButton.gameObject.SetActive(active);
                progressButton.SetShow(true, 3.0f);
                progressButton.ProgressCompleteAction = onClickCheck;
            }
        }

        public void StartProgress() {
            if (mode == InteractableUIMode.NextPrevSelect) {
                if(progressButton.IsActive) {
                    progressButton.startProgress();
                }
            }
        }

        public void CheckProgress() {
            if (mode == InteractableUIMode.NextPrevSelect)  {
                if (progressButton.IsActive) {
                    progressButton.checkProgress();
                }
            }
        }

        public float ProgressTime {
            get {
                if (mode == InteractableUIMode.NextPrevSelect) {
                    return progressButton.ProgressTime;
                }
                else
                    return 0;
            }
        }

        public bool ProgressComplete { 
            get {
                if (mode == InteractableUIMode.NextPrevSelect)
                    return progressButton.ProgressComplete;
                else
                    return false;
            }
        }

        /// <summary> Activates Checkmark on GridSelect and ListSelect Buttons </summary>
        private void SetCheckImageActive() {
            if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect || mode == InteractableUIMode.Dialogue) {
                if (InteractablePath.isNextInteractable(parent))
                    checkImages[InteractablePath.NextInteractable.ID.actionID].gameObject.SetActive(true);
                else {
                    checkImages[0].gameObject.SetActive(false);
                    checkImages[1].gameObject.SetActive(false);
                    checkImages[2].gameObject.SetActive(false);
                    checkImages[3].gameObject.SetActive(false);
                }
            }
        }

        /// <summary> Activates Checkmarks for Rehearal Mode </summary>
        public void SetCheckmark(int actionIndex) {
            if (GameManager.Mode == GameMode.Rehearsal) {
                SetCheckImageActive();
                
                if (InteractablePath.isNextInteractable(parent) && actionIndex == InteractablePath.NextInteractable.ID.actionID) 
                    SetCheckButtonActive(true);
                else
                    SetCheckButtonActive(false);
            }
            else if (GameManager.Mode == GameMode.Recall) {
                SetCheckButtonActive(true);
            }
        }

        public Bounds actionUiBox()
        {
            if (actionUI != null)
            {
                if (actionUI.GetComponent<Collider>() != null)
                    return actionUI.GetComponent<Collider>().bounds;
                else if (actionUI.GetComponent<Mesh>() != null)
                    return actionUI.GetComponent<Mesh>().bounds;
                else if (actionUI.GetComponent<Renderer>() != null)
                    return actionUI.GetComponent<Renderer>().bounds;
            }
            Debug.Log("Action UI is null or has some other issue");
            Bounds returnBounds = new Bounds(new Vector3(-997, -997, -997), new Vector3(0,0,0));
            return returnBounds;
        }

        public void ToggleTracker(bool active) {
            if (actionUI == null) return;

            Canvas tracker = actionUI.gameObject.GetComponentsInChildren<Canvas>(true)[2];
            tracker.gameObject.SetActive(active);

            RectTransform rect = actionUI.GetComponentsInChildren<Button>(true)[2].GetComponentInChildren<RectTransform>();
            rect.localPosition = new Vector3(-169, 0, 0);
        }

        public bool IsOnHover() {
            bool hover = false;

            if(actionButtons != null) {
                foreach (Button button in actionButtons) {
                    if (button.GetComponentInChildren<InteractButton>() != null) {
                        if (button.GetComponentInChildren<InteractButton>().IsOnHover)
                            hover = true;
                    }
                }                
            }

            if(labelButton != null) {
                if (labelButton.GetComponentInChildren<ProgressButton>() != null) {
                    if (labelButton.GetComponentInChildren<ProgressButton>().IsOnHover)
                        hover = true;
                }                
            }

            return hover;
        }

    }
}
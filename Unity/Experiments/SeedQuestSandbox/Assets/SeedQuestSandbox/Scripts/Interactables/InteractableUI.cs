using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SeedQuest.Interactables
{
    public enum InteractableUIMode { NextPrevSelect, GridSelect, ListSelect };

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

        private Interactable parent;
        private GameObject actionUI = null;
        private Button labelButton;
        private Button[] actionButtons;
        private Button checkButton;
        private Image[] checkImages;

        public void Update()
        {
            if (isReady())
            {
                SetPosition();
                SetRotation();
            }
        }

        /// <summary> Initialize Interactable UI with Prompt Text and Buttons </summary>
        /// <param name="interactable">Parent Interactable Object</param>
        public void Initialize(Interactable interactable)
        {
            parent = interactable;

            if (interactable.flagDeleteUI)
                return;

            int modeIndex = 0;
            modeIndex = mode == InteractableUIMode.GridSelect ? 1 : modeIndex;
            modeIndex = mode == InteractableUIMode.ListSelect ? 2 : modeIndex;

            actionUI = GameObject.Instantiate(InteractableManager.Instance.actionSpotIcons[modeIndex], InteractableManager.Instance.transform);
            SetScale();
            SetPosition();
            SetupLabelButton();
            SetupActionButtons();
            SetupCheckButton();
        }

        /// <summary> Ready Status of InteractableUI </summary>
        public bool isReady() {
            return actionUI != null;
        }

        /// <summary> Delete UI Object </summary>
        public void DeleteUI() {
            GameObject.Destroy(actionUI);
        }

        /// <summary> Intialize and Setupt Label Button </summary>
        public void SetupLabelButton() {

            var textList = actionUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            textList[0].text = parent.Name;

            Button[] buttons = actionUI.GetComponentsInChildren<Button>();
            labelButton = buttons[0];
            labelButton.onClick.AddListener(onClickLabel);

            setButtonHoverEvents(labelButton);
        }

        /// <summary> Intialize and Setup Action Buttons </summary>
        public void SetupActionButtons()
        {
            var textList = actionUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            foreach (TMPro.TextMeshProUGUI text in textList)
                text.fontSize = fontSize;

            Button[] buttons = actionUI.GetComponentsInChildren<Button>();

            if (mode == InteractableUIMode.NextPrevSelect)  {
                actionButtons = new Button[buttons.Length - 2];
            }
            else if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect) {
                actionButtons = new Button[buttons.Length - 1];
                checkImages = new Image[buttons.Length - 1];
            }

            System.Array.Copy(buttons, 1, actionButtons, 0, actionButtons.Length);

            if (mode == InteractableUIMode.NextPrevSelect) {
                actionButtons[0].onClick.AddListener(parent.NextAction);
                actionButtons[1].onClick.AddListener(parent.PrevAction);
            }
            else if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect) {
                for (int i = 0; i < 4; i++) {
                    var actionText = actionButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    actionText.text = parent.stateData.getStateName(i);

                    checkImages[i] = actionButtons[i].gameObject.GetComponentsInChildren<Image>()[1];
                }

                foreach (Image image in checkImages) {
                    image.gameObject.SetActive(false);
                }

                actionButtons[0].onClick.AddListener(delegate { ClickActionButton(0); });
                actionButtons[1].onClick.AddListener(delegate { ClickActionButton(1); });
                actionButtons[2].onClick.AddListener(delegate { ClickActionButton(2); });
                actionButtons[3].onClick.AddListener(delegate { ClickActionButton(3); });
            }

            hideActions();

            // Create Triggers for HoverEvents
            foreach (Button button in actionButtons)
                setButtonHoverEvents(button);
        }

        /// <summary> Setup Checkmark Button for use with NextPrevSelect Button only </summary>
        public void SetupCheckButton() {
            if (mode == InteractableUIMode.NextPrevSelect) {
                Button[] buttons = actionUI.GetComponentsInChildren<Button>();
                checkButton = buttons[1];
                checkButton.onClick.AddListener(onClickCheck);
                checkButton.gameObject.SetActive(false);
                setButtonHoverEvents(checkButton);
            }
        }

        /// <summary> Set up button hover events for use in ThirdPerson and FirstPerson scenes </summary>
        public void setButtonHoverEvents(Button button) {
            EventTrigger trigger = button.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => {
                if (PauseManager.isPaused == true) return;
                GameManager.State = GameState.Interact;
            });
            trigger.triggers.Add(entry);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((data) => {
                if (PauseManager.isPaused == true) return;
                GameManager.State = GameState.Play;
            });
            trigger.triggers.Add(exit);
        }

        /// <summary> Handles Clicking the Label Button </summary>
        public void onClickLabel() {
            parent.NextAction();
        }

        /// <summary> Handles Clicking an Action Button </summary>
        public void ClickActionButton(int actionIndex) {
            parent.DoAction(actionIndex);

            if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect)
                hideActions();

            if (GameManager.Mode == GameMode.Rehearsal) {
                if (actionIndex == InteractablePath.NextInteractable.ID.actionID)
                    InteractablePath.GoToNextInteractable();
            }
            else if (GameManager.Mode == GameMode.Recall)
                InteractableLog.Add(parent, parent.currentStateID);
        }

        /// <summary> Handles Clicking a Checkmark Button </summary>
        public void onClickCheck() {
            SetCheckButtonActive(false);

            if (GameManager.Mode == GameMode.Rehearsal)
                InteractablePath.GoToNextInteractable();
            else if (GameManager.Mode == GameMode.Recall)
                InteractableLog.Add(parent, parent.currentStateID);
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
            var textMesh = actionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textMesh.text = text;
        }

        /// <summary> Gets Label Text </summary>
        public string GetText() {
            return actionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
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

            foreach (Button button in actionButtons)
                button.transform.gameObject.SetActive(false);
        }

        /// <summary> Show Action Button UI </summary>
        public void showActions()  {
            if (actionButtons == null) return;

            foreach (Button button in actionButtons)
                button.transform.gameObject.SetActive(true);
        }

        /// <summary> Handles hovering over UI </summary>
        public void onHoverUI() {
            GameManager.State = GameState.Interact;
            showCurrentActions();
            InteractableManager.SetActiveInteractable(parent);
        }

        /// <summary> Handles exiting hovering UI </summary>
        public void offHoverUI() {
            GameManager.State = GameState.Play;
        }


        /// <summary> Sets UI Size Scale </summary>
        public void SetScale() {
            actionUI.GetComponent<RectTransform>().localScale = new Vector3(-0.01f * scaleSize, 0.01f * scaleSize, 0.01f * scaleSize);
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
            if (mode == InteractableUIMode.NextPrevSelect)
                checkButton.gameObject.SetActive(active);
        }

        /// <summary> Activates Checkmark on GridSelect and ListSelect Buttons </summary>
        private void SetCheckImageActive() {
            if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect) {
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
    }
}
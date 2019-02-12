using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
        private Image checkmark;

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
        }

        public void Update()
        {
            if (isReady())
            {
                SetPosition();
                SetRotation();
            }
        }

        public bool isReady()
        {
            return actionUI != null;
        }

        public void DeleteUI() {
            GameObject.Destroy(actionUI);
        }

        public void SetupLabelButton() {

            var textList = actionUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            textList[0].text = parent.Name;

            Button[] buttons = actionUI.GetComponentsInChildren<Button>();
            labelButton = buttons[0];
            labelButton.onClick.AddListener(onClickLabel);

            BoxCollider collider = labelButton.gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(200, 40, 10);

            checkmark = labelButton.gameObject.GetComponentsInChildren<Image>()[1];
            if(checkmark != null)
                checkmark.gameObject.SetActive(false);

            setLabelHoverEvents();
        }

        public void SetupActionButtons()
        {
            var textList = actionUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            foreach (TMPro.TextMeshProUGUI text in textList)
                text.fontSize = fontSize;

            Button[] buttons = actionUI.GetComponentsInChildren<Button>();
            actionButtons = new Button[buttons.Length - 1];
            System.Array.Copy(buttons, 1, actionButtons, 0, actionButtons.Length);

            if (mode == InteractableUIMode.NextPrevSelect) {
                actionButtons[0].onClick.AddListener(parent.NextAction);
                actionButtons[1].onClick.AddListener(parent.PrevAction);
            }
            else if (mode == InteractableUIMode.GridSelect || mode == InteractableUIMode.ListSelect) {
                for (int i = 0; i < 4; i++) {
                    var actionText = actionButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    actionText.text = parent.stateData.getStateName(i);
                }

                actionButtons[0].onClick.AddListener(delegate { parent.DoAction(0); });
                actionButtons[1].onClick.AddListener(delegate { parent.DoAction(1); });
                actionButtons[2].onClick.AddListener(delegate { parent.DoAction(2); });
                actionButtons[3].onClick.AddListener(delegate { parent.DoAction(3); });
            }

            hideActions();

            // Create Triggers for HoverEvents
            foreach (Button button in actionButtons) {
                setButtonHoverEvents(button);
            }

            foreach (Button button in actionButtons) {
                BoxCollider collider = button.gameObject.AddComponent<BoxCollider>();
                collider.size = new Vector3(40, 40, 10);
            }
        }

        public void setButtonHoverEvents(Button button) {
            EventTrigger trigger = button.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => {
                GameManager.State = GameState.Interact;
            });
            trigger.triggers.Add(entry);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((data) => {
                GameManager.State = GameState.Play;
            });
            trigger.triggers.Add(exit);
        }

        public void setLabelHoverEvents() {
            EventTrigger trigger = labelButton.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => {
                GameManager.State = GameState.Interact;
                if (checkmark != null && InteractablePath.isNextInteractable(parent))
                    checkmark.gameObject.SetActive(true);
            });
            trigger.triggers.Add(entry);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((data) => {
                GameManager.State = GameState.Play;
                if (checkmark != null && InteractablePath.isNextInteractable(parent))
                    checkmark.gameObject.SetActive(false);
            });
            trigger.triggers.Add(exit);
        }

        public void hideActions() {
            if (actionButtons == null) return;

            foreach (Button button in actionButtons)
            {
                button.transform.gameObject.SetActive(false);
            }
        }

        public void showActions() {
            if (actionButtons == null) return;

            foreach (Button button in actionButtons)
            {
                button.transform.gameObject.SetActive(true);
            }
        }

        public void showCurrentActions()
        {
            InteractableManager.hideAllInteractableUI();
            showActions();
        }

        public void onClickLabel()
        {
            InteractablePath.GoToNextInteractable();
        }

        public void onHoverUI()
        {
            GameManager.State = GameState.Interact;
            showCurrentActions();
            InteractableManager.SetActiveInteractable(parent);
        }

        public void offHoverUI()
        {
            GameManager.State = GameState.Play;
        }

        public void toggleActions()
        {
            bool isShown = actionButtons[0].gameObject.activeSelf;
            if (isShown)
                hideActions();
            else
                showActions();
        }

        public void SetScale()
        {
            actionUI.GetComponent<RectTransform>().localScale = new Vector3(-0.01f * scaleSize, 0.01f * scaleSize, 0.01f * scaleSize);
        }

        public void SetPosition()
        {
            Vector3 labelPositionOffset = Vector3.zero;
            if (parent.stateData != null) labelPositionOffset = parent.stateData.labelPosOffset;
            Vector3 position = parent.transform.position + labelPositionOffset + positionOffset;
            actionUI.GetComponent<RectTransform>().position = position;
        }

        public void SetRotation()
        {
            if (useRotateToCamera)
            {
                BillboardInteractable();
                actionUI.GetComponent<RectTransform>().Rotate(rotationOffset);
            }
            else
            {
                actionUI.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotationOffset);
            }
        }

        public void BillboardInteractable()
        {
            Vector3 targetPosition = Camera.main.transform.position - (100 * Camera.main.transform.forward ) ;
            Vector3 interactablePosition = actionUI.transform.position;
            Vector3 lookAtDir = targetPosition - interactablePosition;

            Quaternion rotate = Quaternion.LookRotation(lookAtDir);
            actionUI.transform.rotation = rotate;
        }

        public void SetText(string text)
        {
            var textMesh = actionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textMesh.text = text;
        }

    }
}
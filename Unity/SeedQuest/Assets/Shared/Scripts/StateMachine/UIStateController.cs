using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStateController : MonoBehaviour {

    public GameObject ActionDisplay;
    public GameObject Tooltip;
    public GameObject TooltipLabel;
    public GameObject TooltipActionOne;
    public GameObject TooltipActionTwo;
    public GameObject StartScreen;
    public GameObject DebugDisplay;
    public GameObject copyButton;
    public GameObject seedCanvas;
    public GameObject seedDisplay;
    public GameStateData gameState;

    private void Start() {
        InitalizeActionDisplay();
        InitializeDebugDisplay();
        InitializeToolTip();
    }

    private void Update() {
        CheckGameStart();
        UpdateActionDisplay();
        UpdateTooltip();
    }

    private void InitalizeActionDisplay() {
        ActionDisplay.SetActive(false);

        int count = gameState.targetList.Length;
        for (int i = 0; i < count; i++) { 
            createActionItem(i, gameState.targetList[i].description);
        }
    } 

    private void InitializeDebugDisplay() {
        DebugDisplay.GetComponentInChildren<Text>().text = "";
    }

    private void InitializeToolTip() {
        gameState.showPathTooltip = false;
    }

    private void CheckGameStart() {
        
        // Start Game after StartScreen
        if(gameState.startPathSearch) {
            ActionDisplay.SetActive(true);
            StartScreen.SetActive(false);

            if(gameState.inRehersalMode)
                DebugDisplay.GetComponentInChildren<Text>().text = "Mode: Rehersal";
            else
                DebugDisplay.GetComponentInChildren<Text>().text = "Mode: Recall";
        } 
    }

    private void UpdateActionDisplay() {
        
        if (gameState.inRehersalMode)
            UpdateActionDisplayRehersalMode();
        else if (!gameState.inRehersalMode) 
            UpdateActionDisplayRecallMode();
    }

    private void UpdateActionDisplayRehersalMode() {
        
        int count = gameState.targetList.Length;
        for (int i = 0; i < count; i++)
        {
            ActionDisplay.transform.GetChild(i + 1).gameObject.SetActive(true);
        }

        ActionLog log = gameState.actionLog;
        for (int i = 0; i < log.ActionCount(); i++)
        {
            GameObject l = ActionDisplay.transform.GetChild(i + 1).gameObject;
            l.GetComponentInChildren<Image>().sprite = gameState.checkedState;
        }
    }

    private void UpdateActionDisplayRecallMode() {

        int count = gameState.targetList.Length;
        for (int i = 0; i < count; i++) {
            ActionDisplay.transform.GetChild(i+1).gameObject.SetActive(false);
        }

        ActionLog log = gameState.actionLog;
        for (int i = 0; i < log.ActionCount(); i++)
        {
            GameObject g = ActionDisplay.transform.GetChild(i + 1).gameObject;
            g.SetActive(true);
            g.GetComponentInChildren<TextMeshProUGUI>().text = log.iLog[i].description;
            g.GetComponentInChildren<Image>().sprite = gameState.checkedState;
        }
    }

    private void UpdateTooltip() {
        if (gameState.pathComplete)
        {
            Tooltip.SetActive(false);
            seedCanvas.SetActive(true);
            seedDisplay.GetComponent<TextMeshProUGUI>().text = gameState.recoveredSeed;
        }
        else if (gameState.showPathTooltip && gameState.currentAction != null)
        {
            TooltipLabel.GetComponent<TextMeshProUGUI>().text = gameState.currentAction.label;
            TooltipActionOne.GetComponent<TextMeshProUGUI>().text = gameState.currentAction.actions[0].label;
            TooltipActionTwo.GetComponent<TextMeshProUGUI>().text = gameState.currentAction.actions[1].label;
            Tooltip.SetActive(true);
        }
        else
        {
            Tooltip.SetActive(false);
        } 
    }

    private GameObject createActionItem(int index, string text) {
        GameObject item = new GameObject();
        item = Instantiate(item, ActionDisplay.transform);
        item.name = "Action Item " + index;

        ActionItem actionItem = item.AddComponent<ActionItem>();
        actionItem.SetItem(gameState, index, text, gameState.uncheckedState);
        return item;
    } 

}

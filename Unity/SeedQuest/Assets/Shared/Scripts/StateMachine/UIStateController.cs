using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour {

    public GameObject ActionDisplay;
    public GameObject Tooltip;
    public GameObject StartScreen;
    public GameObject DebugDisplay;
    public GameObject copyButton;
    public GameStateData gameState;

    private void Start() {
        InitalizeActionDisplay();
        InitializeDebugDisplay();
        InitializeToolTip();
    }

    private void Update() {
        CheckGameStart();
        CheckRehersalMode();
        UpdateTooltip();
    }

    private void InitalizeActionDisplay() {
        ActionDisplay.SetActive(false);

        int count = gameState.targetList.Length;
        for (int i = 0; i < count; i++)
        {
            Debug.Log(gameState.targetList[i].description);
            GameObject g = ActionDisplay.transform.GetChild(i + 1).gameObject;
            g.GetComponentInChildren<Text>().text = gameState.targetList[i].description;
            g.GetComponentInChildren<Image>().sprite = gameState.uncheckedState;
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

	private void CheckRehersalMode() {
        // In Rehersal Mode
        if (gameState.inRehersalMode) {
            
            int index = System.Array.IndexOf(gameState.targetList, gameState.currentAction);
            if (gameState.pathComplete)
                index = gameState.targetList.Length;

            for (int i = 0; i < index; i++)
            {
                GameObject l = ActionDisplay.transform.GetChild(i + 1).gameObject;
                l.GetComponentInChildren<Image>().sprite = gameState.checkedState;
            }
        }

        if (!gameState.inRehersalMode)
            ActionDisplay.SetActive(false);
    }

    private void UpdateTooltip() {
        if (gameState.pathComplete)
        {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = "Recovered Seed:";
            t[1].text = gameState.recoveredSeed;
            Tooltip.SetActive(true);
            copyButton.SetActive(true);
        }
        else if (gameState.showPathTooltip && gameState.currentAction != null)
        {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = gameState.currentAction.label;
            t[1].text = gameState.currentAction.description;
            Tooltip.SetActive(true);
        }
        else
        {
            Tooltip.SetActive(false);
        } 
    }

}

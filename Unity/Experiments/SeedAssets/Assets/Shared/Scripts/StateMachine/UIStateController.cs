using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour {

    public GameObject ActionDisplay;
    public GameObject Tooltip;
    public GameObject StartScreen;
    public GameObject DebugDisplay;
    public GameStateData playerPathData;

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

        int count = playerPathData.targetList.Length;
        for (int i = 0; i < count; i++)
        {
            GameObject g = ActionDisplay.transform.GetChild(i + 1).gameObject;
            g.GetComponentInChildren<Text>().text = playerPathData.targetList[i].description;
        }
    }

    private void InitializeDebugDisplay() {
        DebugDisplay.GetComponentInChildren<Text>().text = "";
    }

    private void InitializeToolTip() {
        playerPathData.showPathTooltip = false;
    }

    private void CheckGameStart() {
        // Start Game after StartScreen
        if (!playerPathData.startPathSearch)
        {
            if (Input.anyKey)
                playerPathData.startPathSearch = true;

            if (Input.GetButtonDown("Jump"))
                playerPathData.inRehersalMode = true;
            else if (Input.anyKey)
                playerPathData.inRehersalMode = false;
        }  

        if(playerPathData.startPathSearch) {
            ActionDisplay.SetActive(true);
            StartScreen.SetActive(false);

            if(playerPathData.inRehersalMode)
                DebugDisplay.GetComponentInChildren<Text>().text = "Mode: Rehersal";
            else
                DebugDisplay.GetComponentInChildren<Text>().text = "Mode: Recall";
        } 
    }

    private void CheckRehersalMode() {
        // In Rehersal Mode
        if (playerPathData.inRehersalMode) {
            
            int index = System.Array.IndexOf(playerPathData.targetList, playerPathData.currentAction);
            if (playerPathData.pathComplete)
                index = playerPathData.targetList.Length;

            for (int i = 0; i < index; i++)
            {
                GameObject g = ActionDisplay.transform.GetChild(i + 1).gameObject;
                g.GetComponentInChildren<Image>().sprite = playerPathData.checkedState;
            }
        }

        if (!playerPathData.inRehersalMode)
            ActionDisplay.SetActive(false);
    }

    private void UpdateTooltip() {
        if (playerPathData.pathComplete)
        {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = "Recovered Seed:";
            t[1].text = "XXXXXXXXXXX";
            Tooltip.SetActive(true);
        }
        else if (playerPathData.showPathTooltip && playerPathData.currentAction != null)
        {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = playerPathData.currentAction.label;
            t[1].text = playerPathData.currentAction.description;
            Tooltip.SetActive(true);
        }
        else
        {
            Tooltip.SetActive(false);
        } 
    }

}

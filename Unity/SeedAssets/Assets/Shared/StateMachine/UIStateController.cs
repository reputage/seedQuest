using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour {

    public GameObject ActionDisplay;
    public GameObject Tooltip;
    public GameObject StartScreen;
    public GameObject DebugDisplay;
    public PathData playerPathData;

    private void Start()
    {
        playerPathData.startPathSearch = false;
        ActionDisplay.SetActive(false);
        DebugDisplay.GetComponentInChildren<Text>().text = "";

        InitalizeActionDisplay();
    }

    private void InitalizeActionDisplay() {
        int count = playerPathData.targetList.Length;
        for (int i = 0; i < count; i++)
        {
            GameObject g = ActionDisplay.transform.GetChild(i + 1).gameObject;
            g.GetComponentInChildren<Text>().text = playerPathData.targetList[i].description;
        }
    }

    private void Update()
    {
        // Start Game after StartScreen
        if (!playerPathData.startPathSearch) {
            if(Input.anyKey)
                playerPathData.startPathSearch = true;

            if (Input.GetButtonDown("Jump"))
                playerPathData.inRehersalMode = true;
            else if(Input.anyKey)
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

        // In Recall Mode
        if(playerPathData.inRehersalMode) {
            
            int index = System.Array.IndexOf(playerPathData.targetList, playerPathData.currentAction);
            Debug.Log(index);

            for (int i = 0; i < index; i++) {
                GameObject g = ActionDisplay.transform.GetChild(i + 1).gameObject;
                g.GetComponentInChildren<Image>().sprite = playerPathData.checkedState;
            }
        }

        if(!playerPathData.inRehersalMode) {
            ActionDisplay.SetActive(false);
        }

        if(playerPathData.pathComplete) {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = "Recovered Seed:";
            t[1].text = "XXXXXXXXXXX";
            Tooltip.SetActive(true);
        }
        else if(playerPathData.showPathTooltip && playerPathData.currentAction != null) {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = playerPathData.currentAction.label;
            t[1].text = playerPathData.currentAction.description;
            Tooltip.SetActive(true);
        }
        else {
            Tooltip.SetActive(false);
        }
    }

}

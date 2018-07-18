using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour {

    public GameObject ActionDisplay;
    public GameObject Tooltip;
    public PathData playerPathData;

    private void Update()
    {
        if(playerPathData.pathComplete) {
            Text[] t = Tooltip.GetComponentsInChildren<UnityEngine.UI.Text>();
            t[0].text = "Recovered Seed:";
            t[1].text = "XXXXXXXXXXX";
            Tooltip.SetActive(true);
        }
        else if(playerPathData.showPathTooltip && playerPathData.currentAction != null) {
            Text[] t = Tooltip.GetComponentsInChildren<UnityEngine.UI.Text>();
            t[0].text = playerPathData.currentAction.label;
            t[1].text = playerPathData.currentAction.description;
            Tooltip.SetActive(true);
        }
        else {
            Tooltip.SetActive(false);
        }
    }

}

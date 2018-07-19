using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/RecallAction")]
public class RecallAction : PlayerAction {

    public override void Act(StateController controller)
    {
        Recall(controller);
    }

    private void Recall(StateController controller) {
        if (controller.isNearInteractable()) {
            Interactable interactable = controller.getNearestInteractable();
            controller.playerPathData.currentAction = interactable;
            controller.playerPathData.showPathTooltip = true;
        }
        else
            controller.playerPathData.showPathTooltip = false;
    }

}

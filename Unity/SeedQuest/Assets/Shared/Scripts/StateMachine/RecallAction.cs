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
            controller.gameState.currentAction = interactable;
            controller.gameState.showPathTooltip = true;
        }
        else
            controller.gameState.showPathTooltip = false;
    }

}

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
            InteractableID interactable = controller.getNearestInteractable();
            controller.gameState.currentAction = interactable;
            controller.gameState.showPathTooltip = true;

            if (Input.GetButtonDown("Jump")) {
                controller.DoActionAtInteractable(0);
            }
            else if (Input.GetKey("1")) {
                controller.DoActionAtInteractable(1);
            }
        }
        else
            controller.gameState.showPathTooltip = false;
    }

}

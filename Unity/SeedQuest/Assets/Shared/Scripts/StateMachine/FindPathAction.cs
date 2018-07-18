using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/FindPathAction")]
public class FindPathAction : PlayerAction {

    public override void Act(StateController controller) {
        Find(controller);
    }

    private void Find(StateController controller) {
        Vector3[] path = controller.FindPath();
        controller.DrawPath(path);
        controller.checkIsNearTarget();          
    }

}

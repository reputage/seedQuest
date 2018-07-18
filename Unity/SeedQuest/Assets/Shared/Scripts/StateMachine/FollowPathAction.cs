using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/FollowPathAction")]
public class FollowPathAction : PlayerAction {

    public override void Act(StateController controller) {
        Follow(controller);
    }

    private void Follow(StateController controller) {
        controller.FindPath(); 
    }

    private void DrawPath(StateController controller) {
        
    }

}

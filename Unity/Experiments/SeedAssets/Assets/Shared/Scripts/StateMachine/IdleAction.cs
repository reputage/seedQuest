using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/IdleAction")]
public class IdleAction : PlayerAction {

    public override void Act(StateController controller) {
        Idle(controller);
    }

    public void Idle(StateController controller) {
        return;
    }
}

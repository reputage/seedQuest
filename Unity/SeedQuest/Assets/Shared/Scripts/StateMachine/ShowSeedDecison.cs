using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ShowSeed")]
public class ShowSeedDecison : Decision {

    public override bool Decide(StateController controller)
    {
        return ShowSeed(controller);
    }

    private bool ShowSeed(StateController controller) {
        return controller.playerPathData.pathComplete;
    }
}

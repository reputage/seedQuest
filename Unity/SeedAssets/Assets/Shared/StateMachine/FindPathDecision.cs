using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/FindPath")]
public class FindPathDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return StartPathSearch(controller);
    }

    private bool StartPathSearch(StateController controller) {
        return controller.playerPathData.startPathSearch;
    }

}

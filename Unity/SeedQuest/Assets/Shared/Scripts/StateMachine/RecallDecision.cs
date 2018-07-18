using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Recall")]
public class RecallDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return StartRecall(controller);
    }

    private bool StartRecall(StateController controller)
    {
        var data = controller.playerPathData;
        return data.startPathSearch && !data.inRehersalMode;
    }

}

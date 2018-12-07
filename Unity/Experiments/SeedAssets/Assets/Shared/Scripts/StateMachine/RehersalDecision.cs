using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Reherse")]
public class RehersalDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return StartRehersal(controller);
    }

    private bool StartRehersal(StateController controller)
    {
        var data = controller.playerPathData;
        return data.startPathSearch && data.inRehersalMode;
    }

}

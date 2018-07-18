using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/ShowSeed")]
public class ShowSeedAction : PlayerAction {

    public override void Act(StateController controller)
    {
        ShowSeed(controller);
    }

    public void ShowSeed(StateController controller) {
        return;
    }

}

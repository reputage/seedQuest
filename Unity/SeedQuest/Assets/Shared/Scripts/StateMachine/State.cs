using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class State : ScriptableObject
{   
    public PlayerAction[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(StateController controller){
        DoActions(controller);
        CheckTransitions(controller);
    }

    public void DoActions(StateController controller){
        for (int i = 0; i < actions.Length; i++) {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(StateController controller) {
        for (int i = 0; i < transitions.Length; i++) {
            bool success = transitions[i].decision.Decide(controller);

            if (success)
                controller.TransitionToState(transitions[i].trueState);
            else
                controller.TransitionToState(transitions[i].falseState);
        }
    }
}
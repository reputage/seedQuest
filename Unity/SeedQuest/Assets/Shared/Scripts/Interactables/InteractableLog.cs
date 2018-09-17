using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLog : MonoBehaviour {

    public List<InteractableID> interactableLog = new List<InteractableID>();
    public List<int> actionLog = new List<int>();

    public GameStateData gameState;

	public void Add(InteractableID interactable, int actionID) {
        interactableLog.Add(interactable);
        actionLog.Add(actionID);
    }

    public bool ActionsComplete() {
        return interactableLog.Count >= gameState.SiteCount * gameState.ActionCount;   
    }

    public int ActionCount() {
        return interactableLog.Count;
    }

    public string getSeed()
    {
        SeedConverter converter = new SeedConverter();
        return converter.DecodeSeed(this);
    }
}

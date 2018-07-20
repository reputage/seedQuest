using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLog  {

    public List<Interactable> iLog = new List<Interactable>();
    public List<InteractableAction> aLog = new List<InteractableAction>();
	
    public void Add(Interactable interactable, InteractableAction action) {
        iLog.Add(interactable);
        aLog.Add(action);
    }

    public int[] EncodeActionLog() {
        return null;
    }

    public string RecoverSeed() {
        return null;
    }
}

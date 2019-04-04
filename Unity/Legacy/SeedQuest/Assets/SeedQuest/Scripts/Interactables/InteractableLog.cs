using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLog {

    /// <summary> Log of Interactables Used </summary>
    public List<Interactable> interactableLog = new List<Interactable>();
    /// <summary> Log of Interactable Actions Used </summary>
    public List<int> actionLog = new List<int>();
    
    /// <summary> Length of Log </summary>
    public int Length { get { return interactableLog.Count; } }
    /// <summary> Log is complete and filled with the required about of entries </summary>
    public bool LogIsComplete { get { return interactableLog.Count >= SeedManager.SiteCount * SeedManager.ActionCount; } }

    /// <summary> Add a log with the interactable and action used </summary>
    public void Add(Interactable interactable, int actionID) {
        interactableLog.Add(interactable);
        actionLog.Add(actionID);
    }

    public void Pop()
    {
        if (interactableLog.Count > 0)
        {
            interactableLog.RemoveAt(interactableLog.Count - 1);
        }
    }

    /// <summary> Decodes Log into a Seed string </summary>
    public string RecoverSeed() {
        SeedConverter converter = new SeedConverter();
        return converter.DecodeSeed(this);
    }
}

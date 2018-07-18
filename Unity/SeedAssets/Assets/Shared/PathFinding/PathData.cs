using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Path Data")]
public class PathData : ScriptableObject {
    public bool inRehersalMode = true; 
    public bool pathComplete = false;
    public bool showPathTooltip = false;

    public Interactable currentAction;
    public float interactionRadius = 4.0f;
}
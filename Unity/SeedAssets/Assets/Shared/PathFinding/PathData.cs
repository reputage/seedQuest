using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Path Data")]
public class PathData : ScriptableObject {
    public bool startPathSearch = false;
    public bool pathComplete = false;
    public bool showPathTooltip = false;
    public bool inRehersalMode = true;

    public Sprite uncheckedState;
    public Sprite checkedState;

    public LayerMask interactableMask;
    public float interactionRadius = 4.0f;

    public Interactable currentAction;
    public Interactable[] targetList;
}
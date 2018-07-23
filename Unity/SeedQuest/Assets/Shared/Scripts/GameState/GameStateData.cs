using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/GameState")]
public class GameStateData : ScriptableObject {
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

    public string SeedString;
    public string recoveredSeed;
    public ActionLog actionLog;

    public int SiteBits = 4;
    public int SpotBits = 4;
    public int ActionBits = 3;
    public int ActionCount = 4;
    public int SiteCount = 4;

}

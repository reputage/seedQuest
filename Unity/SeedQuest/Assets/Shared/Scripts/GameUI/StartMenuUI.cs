using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuUI : MonoBehaviour {
	
    public void SetToRehearsalMode()
    {
        GameManager.State = GameState.Rehearsal;
    }

    public void SetToRecallMode()
    {
        GameManager.State = GameState.Recall;
    }
}

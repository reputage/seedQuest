using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

public class UndoUI : MonoBehaviour
{
    private void Update()
    {
        ListenForKeyDown();
    }

    public void ListenForKeyDown()
    {
        if(InputManager.GetKeyDown(KeyCode.U)) {
            CardPopupUI popup = GetComponentInChildren<CardPopupUI>(true);
            popup.toggleShow();
        }
    }

    public void Undo() {
        InteractablePathManager.UndoLastAction();
    }
}

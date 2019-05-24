using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SeedQuest.Interactables;

public class UndoUI : MonoBehaviour
{
    public Button undoButton;

    private void Start()
    {
        undoButton.onClick.AddListener(ShowPopup);
    }

    private void Update()
    {
        ListenForKeyDown();
    }

    public void ListenForKeyDown()
    {
        if (InputManager.GetKeyDown(KeyCode.U))
        {
            ShowPopup();
        }
    }

    public void ShowPopup()
    {
        CardPopupUI popup = GetComponentInChildren<CardPopupUI>(true);
        popup.toggleShow();
    }

    public void Undo() {
        InteractablePathManager.UndoLastAction();
    }
}

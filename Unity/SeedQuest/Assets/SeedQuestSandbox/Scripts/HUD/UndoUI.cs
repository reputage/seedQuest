using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SeedQuest.Interactables;

public class UndoUI : MonoBehaviour
{
    public Button undoButton;
    public int counter;

    private void Start()
    {
        undoButton.onClick.AddListener(ShowPopup);
        undoButton.gameObject.SetActive(false);
        counter = 0;
    }

    private void Update()
    {
        ListenForKeyDown();
        counter++;
        if (counter > 10)
        {
            counter = 0;
            ListenForActionTaken();
        }
    }

    public void ListenForKeyDown()
    {
        if (InputManager.GetKeyDown(KeyCode.U))
        {
            int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
            if (actionsThisScene > 0)
                ShowPopup();
        }
    }

    public void ListenForActionTaken()
    {
        int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
        if (actionsThisScene > 0)
        {
            undoButton.gameObject.SetActive(true);
        }
        else
        {
            undoButton.gameObject.SetActive(false);
        }
    }

    public void ShowPopup()
    {
        CardPopupUI popup = GetComponentInChildren<CardPopupUI>(true);
        popup.toggleShow();
    }

    public void Undo() {

        int actionsThisScene = InteractableLog.Count % InteractableConfig.ActionsPerSite;
        if (actionsThisScene > 0)
        {
            InteractablePathManager.UndoLastAction();
        }
        else
        {
            Debug.Log("Unable to undo actions from a previous scene.");    
        }
    }
}

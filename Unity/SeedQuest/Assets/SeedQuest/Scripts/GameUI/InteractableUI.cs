using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour {

    static public InteractableUI instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    static public void show(Interactable item) {

        int actionID = item.ID.actionID;
        Button[] buttons = instance.GetComponentsInChildren<Button>();
        var labels = instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        labels[0].text = item.getInteractableName();

        // Default ActionButton Text
        labels[1].text = "Action One";
        labels[2].text = "Action Two";
        labels[3].text = "Action Three";
        labels[4].text = "Action Four";

        if (item.getStateCount() > 0)
            labels[1].text = item.getStateName(0);
        if (item.getStateCount() > 1)
            labels[2].text = item.getStateName(1);
        if (item.getStateCount() > 2)
            labels[3].text = item.getStateName(2);
        if (item.getStateCount() > 3)
            labels[4].text = item.getStateName(3);

        if(GameManager.PrevState == GameState.Rehearsal) {
            for (int i = 0; i < 4; i++) {
                buttons[i].interactable = false;
                labels[i + 1].color = new Color32(255, 255, 255, 128);
                labels[i + 1].outlineColor = new Color32(0, 0, 0, 128);
                labels[i + 1].outlineWidth = 0.15f;
            }

            buttons[actionID].interactable = true;
            labels[actionID + 1].color = new Color32(255, 59, 59, 255);
            labels[actionID + 1].outlineColor = new Color32(255, 255, 255, 255);
            labels[actionID + 1].outlineWidth = 0.25f;
        }
        else if(GameManager.PrevState == GameState.Recall) {
            for (int i = 0; i < 4; i++)
            {
                labels[i + 1].color = new Color32(255, 255, 255, 255);
                labels[i + 1].outlineColor = new Color32(0, 0, 0, 255);
                labels[i + 1].outlineWidth = 0.15f;
            }
        }
    }

    public void doAction(int index) {
        InteractableManager.doInteractableAction(index);
    }
}

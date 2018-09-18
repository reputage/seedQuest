using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionListCanvas : MonoBehaviour {

    public List<ActionItem> actionItemList = new List<ActionItem>();

	void Update () {
        if(GameManager.State == GameState.Rehearsal) {
            CreateRehersalActionList();
            UpdateRehersalActionList();
        }
        else if(GameManager.State == GameState.Recall){

        }
	}

    private void CreateRehersalActionList() {
        if (actionItemList.Count > 0)
            return;

        int count = PathManager.Path.Length;
        for (int i = 0; i < count; i++)
        {
            string name = PathManager.Path[i].Name;
            string action = PathManager.Path[i].RehersalActionName;
            GameObject item = CreateActionItem(i, name + ": " + action);
            actionItemList.Add(item.GetComponent<ActionItem>());
        }
    }

    private GameObject CreateActionItem(int index, string text) {
        GameObject item = new GameObject();
        item = Instantiate(item, transform);
        item.name = "Action Item " + index;

        ActionItem actionItem = item.AddComponent<ActionItem>();
        actionItem.SetItem(index, text, GameManager.GameUI.uncheckedBox);
        return item;
    }

    private void UpdateRehersalActionList() {
        int count = InteractableManager.Log.Length;
        for (int i = 0; i < count; i++)
            actionItemList[i].image.sprite = GameManager.GameUI.checkedBox;
    }
}

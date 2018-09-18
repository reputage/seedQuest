using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionListCanvas : MonoBehaviour {

    public List<ActionItem> ActionItemList = new List<ActionItem>();

	void Update () {
        if(GameManager.State == GameState.Rehearsal) {
            CreateRehersalActionList();
            UpdateRehersalActionList();
        }
	}

    private void CreateRehersalActionList() {
        if (ActionItemList.Count > 0)
            return;

        int count = PathManager.Path.Length;
        for (int i = 0; i < count; i++)
        {
            string name = PathManager.Path[i].Name;
            string action = PathManager.Path[i].RehersalActionName;
            GameObject item = createActionItem(i, name + ": " + action);
            ActionItemList.Add(item.GetComponent<ActionItem>());
        }
    }

    private GameObject createActionItem(int index, string text) {
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
            ActionItemList[i].image.sprite = GameManager.GameUI.checkedBox;
    }
}

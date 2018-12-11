using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionListCanvas : MonoBehaviour {

    /// <summary> List of current UI ActionItems </summary>
    public List<ActionItem> actionItemList = new List<ActionItem>();
    public GameObject actionListContainer;
    public ScrollRect scroll;
    public Button upButton;
    public Button downButton;

    private int currentIndex;

	private void Start()
	{
        actionListContainer = GameObject.FindGameObjectWithTag("Action List Container");
        scroll = GameObject.FindGameObjectWithTag("Scroll View").GetComponent<ScrollRect>();
        upButton = GameObject.FindGameObjectWithTag("Up Button").GetComponent<Button>();
        downButton = GameObject.FindGameObjectWithTag("Down Button").GetComponent<Button>();
        upButton.onClick.AddListener(onClickScrollUp);
        downButton.onClick.AddListener(onClickScrollDown);
        currentIndex = 0;
	}

	private void Update () {
        if(GameManager.State == GameState.GameStart) {
            ClearActionList();
        }
        else if(GameManager.State == GameState.Rehearsal) {
            CreateRehersalActionList();
            UpdateRehersalActionList();
        }
        else if(GameManager.State == GameState.Recall){
            UpdateRecallActionList();
        }
	}

    /// <summary> Creates an action list for rehersal mode </summary>
    private void CreateRehersalActionList() {
        if (actionItemList.Count > 0) // Check if list has been populated
            return;

        if (PathManager.Path == null || PathManager.Path.Length == 0)
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

    /// <summary> Clears the action list </summary>
    private void ClearActionList() {
        actionItemList.Clear();
    }

    /// <summary> Creates an action list item </summary>
    private GameObject CreateActionItem(int index, string text) {
        GameObject item = new GameObject();
        item = Instantiate(item, transform);
        item.name = "Action Item " + index;

        ActionItem actionItem = item.AddComponent<ActionItem>();
        actionItem.SetItem(index, text, GameManager.GameUI.uncheckedBox);
        item.transform.SetParent(actionListContainer.transform);
        return item;
    } 

    /// <summary> Updates the action list for rehersal mode based on the interactable log </summary>
    private void UpdateRehersalActionList() {
        /*if(PathManager.LastPathTarget == null) {
            string name_action = PathManager.PathTarget.Name + " : " + PathManager.PathTarget.RehersalActionName;
            actionItemList[0].text.text = name_action;
            actionItemList[1].transform.gameObject.SetActive(false);
        } 
        else {
            string name_action_last = PathManager.LastPathTarget.Name + " : " + PathManager.LastPathTarget.RehersalActionName;
            actionItemList[0].text.text = name_action_last; 
            actionItemList[1].transform.gameObject.SetActive(true);
            string name_action = PathManager.PathTarget.Name + " : " + PathManager.PathTarget.RehersalActionName;
            actionItemList[1].text.text = name_action; 

        }*/
            
        int count = InteractableManager.Log.Length;
        if(count > 0) {
            actionItemList[count - 1].image.sprite = GameManager.GameUI.checkedBox;
            if (currentIndex != count)
            {
                float decrement = 1 - (0.095f * (count-1));
                if (decrement < 0) {
                    decrement = 0;
                }
                scroll.verticalNormalizedPosition = decrement;
                currentIndex = count;
            }
        }

        
        //for (int i = 0; i < count; i++)
        //    actionItemList[i].image.sprite = GameManager.GameUI.checkedBox;
    }

    /// <summary> Updates the action list for recall mode - Creates new items as interactable log grows </summary>
    private void UpdateRecallActionList()
    {
        int listCount = actionItemList.Count;
        int logCount = InteractableManager.Log.Length;
        for (int i = listCount; listCount < logCount; i++)
        {
            string name = InteractableManager.Log.interactableLog[i].Name;
            int actionID = InteractableManager.Log.actionLog[i];
            string action = InteractableManager.Log.interactableLog[i].stateData.states[actionID].actionName;
            GameObject item = CreateActionItem(i, name + ": " + action);
            item.GetComponent<ActionItem>().image.sprite = GameManager.GameUI.checkedBox;
            actionItemList.Add(item.GetComponent<ActionItem>());
        }

    }

    private void onClickScrollUp()
    {
        scroll.verticalNormalizedPosition -= 0.1f;
    }

    private void onClickScrollDown() {
        scroll.verticalNormalizedPosition += 0.1f;
    }
}

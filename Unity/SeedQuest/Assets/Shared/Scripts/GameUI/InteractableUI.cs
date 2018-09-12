using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour {

    static public InteractableUI instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        //hide();
    }

    static public void show(Interactable item) {
        InteractableManager.ActiveItem = item;
        instance.gameObject.SetActive(true);
        PauseManager.isPaused = true;

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
    }

    static public void hide() {
        instance.gameObject.SetActive(false);
    }

    public void doAction(int index) {
        InteractableManager.doInteractableAction(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeedQuest.Interactables;
using TMPro;

public class InteractableOptionButton : MonoBehaviour
{
    public InteractableStateData data;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
    }

    public void Initialize(InteractableStateData data) {
        this.data = data;
        GetComponentInChildren<TextMeshProUGUI>().text = data.interactableUI.name;
    }

    private void HandleOnClick() {
        Debug.Log(data.interactableUI.name);
        InteractableViewer.SetInteractable(data);
    }
}
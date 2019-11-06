using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableViewerOptions : MonoBehaviour
{
    void Start() {
        Button button = GetComponentInChildren<Button>();
        int iCount = InteractableViewer.interactables.Count;
        for (int i = 1; i < iCount; i++){
            Instantiate(button, Vector3.zero, Quaternion.identity, GetComponentsInChildren<Image>()[1].transform);
        }

        int count = 0;
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach(Button _ in buttons) {
            _.GetComponent<InteractableOptionButton>().Initialize(InteractableViewer.interactables[count]);
            count++;
        }
    }
}
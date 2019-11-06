using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeedQuest.Interactables;

using System.Linq;

public class InteractableViewer : MonoBehaviour
{
    static public List<InteractableStateData> interactables;

    void Awake() {
        GetAssetList();
    }

    void Start() {
        GetComponent<Interactable>().stateData = interactables[0];
    }

    public void GetAssetList() {
        string[] guids = AssetDatabase.FindAssets("t:InteractableStateData", null);
        interactables = new List<InteractableStateData>();

        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            interactables.Add((InteractableStateData)AssetDatabase.LoadAssetAtPath(path, typeof(InteractableStateData)));
        }

        interactables = interactables.Where(x => x.interactableUI.name != "").ToList();

        stopwatch.Stop();
        Debug.Log("Loaded " + interactables.Count + " interactables in " + stopwatch.ElapsedMilliseconds + " ms");
    }

    static public void SetInteractable(InteractableStateData data) {
        InteractableViewer viewer = GameObject.FindObjectOfType<InteractableViewer>();
        viewer.GetComponent<Interactable>().UpdateStateData(data);
    }
}

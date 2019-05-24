using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public static MarkerManager instance;

    public GameObject clickMarker;

    bool destroyOldestMarker = false;
    List<GameObject> activeMarkers = new List<GameObject>();
    Transform effectContainer;

    void Awake() {
        if (!instance) {
            instance = this;
        }

        if (!GameObject.Find("EffectContainer")) {
            effectContainer = new GameObject("EffectContainer").transform;
            effectContainer.parent = transform;
        }
        else {
            effectContainer = GameObject.Find("EffectContainer").transform;
        }
    }

    // Update is called once per frame
    void Update() {
        if(destroyOldestMarker)
        {
            GameObject temp = activeMarkers[0];
            activeMarkers.RemoveAt(0);
            Destroy(temp);
            destroyOldestMarker = false;
        }
    }

    public void GenerateMaker(Vector3 position, Quaternion rotation) {
        GameObject newMarker = (GameObject)Instantiate(clickMarker, position, rotation);
        newMarker.transform.parent = effectContainer;
        activeMarkers.Add(newMarker);
        newMarker.name += "." + activeMarkers.Count;

        if(activeMarkers.Count > 1)
            destroyOldestMarker = true;
    }

    static public void GenerateMarker(Vector3 position, Quaternion rotation) {
        if(instance != null)
            MarkerManager.instance.GenerateMaker(position, rotation);
    }

    static public void DeleteMarker() { 
        if(MarkerManager.instance.activeMarkers.Count > 0)
            MarkerManager.instance.destroyOldestMarker = true;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class AnimationTrigger : MonoBehaviour {
    
    public string name = "";
    public GameObject prefab = null;
    private bool enter = true;

    void Awake() {
        GetComponent<BoxCollider>().isTrigger = true;    
    }

    public void trigger()
    {
        GameObject item = gameObject;

        // Remove Children GameObjects to Remove Old Prefabs
        foreach (Transform child in item.transform) {
            if (child.tag != "Static")
            {
                child.gameObject.SetActive(false);
                GameObject.Destroy(child.gameObject);
            }            
        }

        if (item.GetComponent<MeshFilter>() != null)
            item.GetComponent<MeshFilter>().sharedMesh = null;

        // Update with Prefab
        if (prefab != null) {
            GameObject _prefab = GameObject.Instantiate(prefab, item.transform);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Debug.Log("Entered: " + name);
            trigger();
        }
    }
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

namespace SeedQuest.Debugger
{

    public class DebugManager : MonoBehaviour
    {
        public Material debugMaterial;
        public bool showBoundingBoxes = false;

        private static DebugManager instance = null;
        public static DebugManager Instance
        {
            get {
                if (instance == null) {
                    instance = GameObject.FindObjectOfType<DebugManager>();
                }
                return instance;
            }
        }

        private void Start() {
            if (!debugMaterial)
            {
                Debug.LogError("Please Assign a material");
                return;
            }
        }

        static public void DebugShowBoundingBox() {
            Interactable[] interactables = InteractableManager.InteractableList;
            foreach(Interactable item in interactables) {
                
                BoxCollider[] colliders = item.GetComponentsInChildren<BoxCollider>();
                foreach(BoxCollider box in colliders) {

                    Vector3 position = box.center;
                    Quaternion rotation = box.transform.rotation;
                    Vector3 scale = box.size;

                    WireBox.Render(position, rotation, scale, item.transform, Instance.debugMaterial);
                }
            }

        }

        private void OnRenderObject() {
            if (showBoundingBoxes)
                DebugShowBoundingBox();
        }
    }
}
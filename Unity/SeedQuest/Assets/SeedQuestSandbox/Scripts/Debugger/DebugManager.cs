using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

namespace SeedQuest.Debugger
{

    public class DebugManager : MonoBehaviour
    {
        public Material debugMaterial;
        public Material debugMaterial2;
        public bool showBoundingBoxes = false;
        public bool showOtherBoxes = false;

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
                    Vector3 scale = box.size;

                    WireBox.Render(position, scale, box.transform, Instance.debugMaterial);
                }
            }
        }

        static public void DebugBoxColliders()
        {
            BoxCollider[] boxColliders = FindObjectsOfType<BoxCollider>();
            foreach (BoxCollider item in boxColliders)
            {
                Interactable[] parent = item.GetComponentsInParent<Interactable>();
                if (parent.Length <= 0)
                {
                    Vector3 position = item.center;
                    Vector3 scale = item.size;

                    WireBox.Render(position, scale, item.transform, Instance.debugMaterial2);
                }
            }
        }

        private void OnRenderObject() {
            if(showBoundingBoxes)
                DebugShowBoundingBox();
            if (showOtherBoxes)
                DebugBoxColliders();        
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractablePreview
    {
        public Vector3 scale = Vector3.one;
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public int fontSize = 36;
    }

    public class InteractablePreviewUI : MonoBehaviour
    {
        private static InteractablePreviewUI instance = null;

        public static InteractablePreviewUI Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<InteractablePreviewUI>();
                return instance;
            }
        }

        private GameObject previewObject;
        private TMPro.TextMeshProUGUI previewText;
        private GameObject previewChild;
        private Interactable previewInteractable;

        private void Start()
        {
            previewObject = GameObject.FindGameObjectWithTag("PreviewObject");
            previewText = GameObject.FindGameObjectWithTag("PreviewText").GetComponent<TMPro.TextMeshProUGUI>();
        }

        private void Update()
        {
            InteractablePreview preview = Instance.previewInteractable.interactablePreview; 
            if(preview != null)
            {
                previewChild.transform.localPosition = preview.position;
                previewChild.transform.localRotation = Quaternion.Euler(preview.rotation);
                previewChild.transform.localScale = preview.scale;

                previewText.fontSize = preview.fontSize;
            }
        } 

        static public void SetPreviewObject(Interactable interactable)
        {
            Instance.previewInteractable = interactable;

            foreach (Transform child in Instance.previewObject.transform)
                GameObject.Destroy(child.gameObject);

            Instance.previewChild = Instantiate(interactable.gameObject, Instance.previewObject.transform);
            Instance.previewChild.layer = LayerMask.NameToLayer("InteractablePreview");
            foreach(Transform child in Instance.previewChild.transform)
                child.gameObject.layer = LayerMask.NameToLayer("InteractablePreview");

            Instance.previewText.text = interactable.Name;
        }
    }

}
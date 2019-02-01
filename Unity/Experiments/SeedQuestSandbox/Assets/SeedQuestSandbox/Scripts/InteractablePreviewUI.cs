using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Interactables
{
    public enum InteractablePreviewLocation { topright, bottomright }

    [System.Serializable]
    public class InteractablePreviewInfo
    {
        public Vector3 scale = Vector3.one;
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public int fontSize = 36;
        public bool useOrthographic = true;
        public float fieldOfView = 60;
        public float orthographicSize = 1;
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

        public InteractablePreviewLocation location = InteractablePreviewLocation.bottomright;
        public int depthMax = 10;

        private Camera previewCamera;
        private GameObject previewObject;
        private GameObject previewChild;
        private TMPro.TextMeshProUGUI previewText;
        private Interactable previewInteractable;
        private RectTransform canvasTransform;
        private RectTransform imageTransform;


        private void Start()
        {
            previewObject = GameObject.FindGameObjectWithTag("PreviewObject");
            previewText = GameObject.FindGameObjectWithTag("PreviewText").GetComponent<TMPro.TextMeshProUGUI>();
            previewCamera = GameObject.FindGameObjectWithTag("PreviewCamera").GetComponent<Camera>();
            canvasTransform = GameObject.FindGameObjectWithTag("PreviewCanvas").GetComponent<RectTransform>();
            imageTransform = GameObject.FindGameObjectWithTag("PreviewImage").GetComponent<RectTransform>();

            SetLocationTransform();
        }

        private void Update()
        {
            InteractablePreviewInfo preview = Instance.previewInteractable.interactablePreview; 

            if(preview != null)
            {
                previewChild.transform.localPosition = preview.position;
                previewChild.transform.localRotation = Quaternion.Euler(preview.rotation);
                previewChild.transform.localScale = preview.scale;

                previewText.fontSize = preview.fontSize;

                Instance.previewCamera.orthographic = preview.useOrthographic;
                Instance.previewCamera.fieldOfView = preview.fieldOfView;
                Instance.previewCamera.orthographicSize = preview.orthographicSize;
            }
        } 

        public void SetLocationTransform()
        {
            if (location == InteractablePreviewLocation.bottomright)
            {
                canvasTransform.anchorMax = new Vector2(1, 0);
                canvasTransform.anchorMin = new Vector2(1, 0);
                canvasTransform.anchoredPosition3D = Vector3.zero;

                imageTransform.anchorMax = new Vector2(0, 1);
                imageTransform.anchorMin = new Vector2(0, 1);
                imageTransform.anchoredPosition3D = Vector3.zero;
            }
            else if (location == InteractablePreviewLocation.topright)
            {
                canvasTransform.anchorMax = new Vector2(1, 1);
                canvasTransform.anchorMin = new Vector2(1, 1);
                canvasTransform.anchoredPosition3D = Vector3.zero;

                imageTransform.anchorMax = new Vector2(0, 0);
                imageTransform.anchorMin = new Vector2(0, 0);
                imageTransform.anchoredPosition3D = new Vector3(0, -40, 0);
            }
        }

        static public void SetPreviewObject(Interactable interactable)
        {
            Instance.previewInteractable = interactable;

            foreach (Transform child in Instance.previewObject.transform)
                GameObject.Destroy(child.gameObject);

            Instance.previewChild = Instantiate(interactable.gameObject, Instance.previewObject.transform);

            //Instance.previewChild.layer = LayerMask.NameToLayer("InteractablePreview");
            //foreach(Transform child in Instance.previewChild.transform)
            //    child.gameObject.layer = LayerMask.NameToLayer("InteractablePreview");
            SetLayerRecursively(Instance.previewChild, 0);

            Instance.previewText.text = interactable.Name;
        }

        /// <summary>  Recursively set the layer for all children to "InteractablePreview" until max depth is reached or there is no more children </summary>
        static public void SetLayerRecursively(GameObject gameObject, int depth)
        {
            gameObject.layer = LayerMask.NameToLayer("InteractablePreview");

            if (depth > Instance.depthMax)
                return;

            foreach (Transform child in gameObject.transform)
                SetLayerRecursively(child.gameObject, depth+1);
        }
    }
}
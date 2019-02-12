using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Utils;

namespace SeedQuest.Interactables
{
    public enum InteractablePreviewLocation { topright, bottomright }

    [System.Serializable]
    public class InteractablePreviewInfo : ObserverCopy
    {
        public Vector3 scale = Vector3.one;
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public int fontSize = 36;
        public bool useOrthographic = true;
        public float fieldOfView = 60;
        public float orthographicSize = 1;
        public GameObject previewPrefab;
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

        public float previewScale = 1f;
        public InteractablePreviewLocation location = InteractablePreviewLocation.bottomright;
        private Observable<InteractablePreviewLocation> locationObservable;
        private Observable<float> scaleObservable;

        private int depthMax = 10;
        private Camera previewCamera;
        private GameObject previewObject;
        private GameObject previewChild;
        private TMPro.TextMeshProUGUI previewText;
        private Interactable previewInteractable;
        private List<RectTransform> canvasTransforms;
        private RectTransform imageTransform;

        private void Start()  {
            locationObservable = new Observable<InteractablePreviewLocation>( () => location, _ => {location = _;} );
            scaleObservable = new Observable<float>(() => previewScale, _ => { previewScale = _; } );

            SetReferencesFromTags();
            SetLocationTransform();
        }

        private void Update()  {
            locationObservable.onChange(SetLocationTransform);
            scaleObservable.onChange(SetLocationTransform);

            SetPreviewProperties();
            RotatePreview();
        } 

        private void SetReferencesFromTags() {
            previewObject = GameObject.FindGameObjectWithTag("PreviewObject");
            previewText = GameObject.FindGameObjectWithTag("PreviewText").GetComponent<TMPro.TextMeshProUGUI>();
            previewCamera = GameObject.FindGameObjectWithTag("PreviewCamera").GetComponent<Camera>();
            canvasTransforms = new List<RectTransform>();

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PreviewCanvas"))
                canvasTransforms.Add(obj.GetComponent<RectTransform>());

            imageTransform = GameObject.FindGameObjectWithTag("PreviewImage").GetComponent<RectTransform>();
        }

        public void SetPreviewProperties() {
            InteractablePreviewInfo preview;
            if (Instance.previewInteractable == null)
                return;
            else
                preview = Instance.previewInteractable.interactablePreview;
                
            if (preview != null)
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

        public void RotatePreview() {
            if (previewChild != null)
                previewChild.transform.localRotation = previewChild.transform.localRotation * Quaternion.Euler(new Vector3(0,1.0f,0));
        }

        public void SetLocationTransform() {
            foreach (RectTransform canvasTransform in canvasTransforms)
                canvasTransform.localScale = new Vector3(previewScale, previewScale, previewScale);

            if (location == InteractablePreviewLocation.bottomright)
            {
                foreach(RectTransform canvasTransform in canvasTransforms) {
                    canvasTransform.anchorMax = new Vector2(1, 0);
                    canvasTransform.anchorMin = new Vector2(1, 0);
                    canvasTransform.anchoredPosition3D = Vector3.zero;
                }

                imageTransform.anchorMax = new Vector2(0, 1);
                imageTransform.anchorMin = new Vector2(0, 1);
                imageTransform.anchoredPosition3D = new Vector3(50, 0, 0);
            }
            else if (location == InteractablePreviewLocation.topright)
            { 
                foreach(RectTransform canvasTransform in canvasTransforms) {
                    canvasTransform.anchorMax = new Vector2(1, 1);
                    canvasTransform.anchorMin = new Vector2(1, 1);
                    canvasTransform.anchoredPosition3D = Vector3.zero;
                }

                imageTransform.anchorMax = new Vector2(0, 0);
                imageTransform.anchorMin = new Vector2(0, 0);
                imageTransform.anchoredPosition3D = new Vector3(50, 60, 0);
            }
        }

        static public void SetPreviewObject(Interactable interactable)  {

            if (Instance == null)
                return;

            if (interactable == Instance.previewInteractable)
                return;

            Instance.previewInteractable = interactable;

            foreach (Transform child in Instance.previewObject.transform)
                if (child.tag != "Static")
                    GameObject.Destroy(child.gameObject);

            if(interactable.interactablePreview.previewPrefab != null)
                Instance.previewChild = Instantiate(interactable.interactablePreview.previewPrefab, Instance.previewObject.transform);
            else 
                Instance.previewChild = Instantiate(interactable.gameObject, Instance.previewObject.transform);

            SetLayerRecursively(Instance.previewChild, 0);
            Instance.previewText.text = interactable.Name;
        } 

        /// <summary>  Recursively set the layer for all children to "InteractablePreview" until max depth is reached or there is no more children </summary>
        static public void SetLayerRecursively(GameObject gameObject, int depth) {
            gameObject.layer = LayerMask.NameToLayer("InteractablePreview");

            if (depth > Instance.depthMax)
                return;

            foreach (Transform child in gameObject.transform)
                SetLayerRecursively(child.gameObject, depth+1);
        }
    }
}
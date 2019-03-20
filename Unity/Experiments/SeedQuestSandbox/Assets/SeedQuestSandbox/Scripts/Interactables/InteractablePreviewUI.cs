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
        public bool useRotate = true;
        public float rotateSpeed = 25;
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
        private InteractablePreviewInfo preview = null; 
        //private Observer previewObserver = new Observer();

        private int depthMax = 10;
        private Camera previewCamera;
        private GameObject previewObject;
        private GameObject previewChild;
        private TMPro.TextMeshProUGUI previewTitle;
        private TMPro.TextMeshProUGUI previewText;
        private List<RectTransform> canvasTransforms;
        private RectTransform imageTransform;

        private void Awake()  {
            locationObservable = new Observable<InteractablePreviewLocation>( () => location, _ => {location = _;} );
            scaleObservable = new Observable<float>(() => previewScale, _ => { previewScale = _; } );

            SetReferencesFromTags();
            SetLocationTransform();
        }

        private void Update()  {
            if (GameManager.Mode == GameMode.Recall)
                gameObject.SetActive(false);
            else 
                gameObject.SetActive(true);

            locationObservable.onChange(SetLocationTransform);
            scaleObservable.onChange(SetLocationTransform);
            //previewObserver.onChange(SetPreviewProperties);
            SetPreviewProperties();
        } 

        public static bool IsActive {
            get { return Instance.preview != null; }
        }

        private void SetReferencesFromTags() {
            previewObject = GameObject.FindGameObjectWithTag("PreviewObject");
            previewTitle = GameObject.FindGameObjectWithTag("PreviewTitle").GetComponent<TMPro.TextMeshProUGUI>();
            previewText = GameObject.FindGameObjectWithTag("PreviewText").GetComponent<TMPro.TextMeshProUGUI>();
            previewCamera = GameObject.FindGameObjectWithTag("PreviewCamera").GetComponent<Camera>();
            canvasTransforms = new List<RectTransform>();

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PreviewCanvas"))
                canvasTransforms.Add(obj.GetComponent<RectTransform>());

            imageTransform = GameObject.FindGameObjectWithTag("PreviewImage").GetComponent<RectTransform>();
        }

        private float rotateAccumlator = 0;
        public void SetPreviewProperties() {                
            if (preview != null)
            {
                Instance.previewChild.GetComponent<Interactable>().HighlightInteractable(false);

                previewChild.transform.localPosition = preview.position;
                previewChild.transform.localRotation = Quaternion.Euler(preview.rotation);
                previewChild.transform.localScale = preview.scale;

                previewTitle.fontSize = preview.fontSize;
                previewText.fontSize = 0.8f * preview.fontSize;

                Instance.previewCamera.orthographic = preview.useOrthographic;
                Instance.previewCamera.fieldOfView = preview.fieldOfView;
                Instance.previewCamera.orthographicSize = preview.orthographicSize;

                if(preview.useRotate) {
                    rotateAccumlator += preview.rotateSpeed * Time.deltaTime;
                    previewChild.transform.localRotation = Quaternion.Euler(preview.rotation) * Quaternion.Euler(Vector3.up * rotateAccumlator);
                }
            }
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

        /// <summary>  Sets Interactable Preview from Interactable </summary>
        /// <param name="interactable"> Interactable to set Preview with </param>
        static public void SetPreviewObject(Interactable interactable)  {

            // Set Preview if interactablePreview has changed
            if (Instance == null || interactable.interactablePreview == Instance.preview)
                return;
            else
                Instance.preview = interactable.interactablePreview;
            
            // Set Preview Watcher
            //Instance.previewObserver.Watch(Instance.preview);

            // Remove old preview object
            foreach (Transform child in Instance.previewObject.transform)
                    GameObject.Destroy(child.gameObject);

            // Create Preview Gameobject
            if(interactable.interactablePreview.previewPrefab != null)
                Instance.previewChild = Instantiate(interactable.interactablePreview.previewPrefab, Instance.previewObject.transform);
            else 
                Instance.previewChild = Instantiate(interactable.gameObject, Instance.previewObject.transform);

            // Destroy InteractableUI and Remove Highlights
            Instance.previewChild.GetComponent<Interactable>().DeleteUI();
            Instance.previewChild.GetComponent<Interactable>().HighlightInteractableWithEffect(false);

            // Set Layer to "InteractablePreview"
            SetLayerRecursively(Instance.previewChild, 0);

            // Set Label Text
            Instance.previewTitle.text = interactable.Name;
            if (GameManager.Mode == GameMode.Rehearsal)
                Instance.previewText.text = interactable.RehearsalActionName;
            else
                Instance.previewText.text = "";
        } 

        /// <summary>  Recursively set the layer for all children to "InteractablePreview" until max depth is reached or there is no more children </summary>
        static public void SetLayerRecursively(GameObject gameObject, int depth) {
            gameObject.layer = LayerMask.NameToLayer("InteractablePreview");

            if (depth > Instance.depthMax)
                return;

            foreach (Transform child in gameObject.transform)
                SetLayerRecursively(child.gameObject, depth+1);
        }

        /// <summary> Set interactable state with given action index </summary>
        /// <param name="actionIndex"> Action Index </param>
        static public void SetPreviewAction(int actionIndex) {
            Interactable interactable = Instance.previewChild.GetComponent<Interactable>();
            InteractableState state = interactable.stateData.states[actionIndex];
            state.enterState(interactable, false);

            SetLayerRecursively(Instance.previewChild, 0);
        }
    }
}
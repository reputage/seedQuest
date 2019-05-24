using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

public class InteractableCameraRig : MonoBehaviour
{

    private Camera previewCamera;                   // Rig Camera
    private GameObject previewObject;               // Interactable Holder
    private GameObject previewChild;                // Interactable Object
    private InteractablePreviewInfo previewProps;   // PreviewInfo Properties

    private int depthMax = 10;

    void Awake() {
        previewCamera = GetComponentInChildren<Camera>();
        previewObject = GetComponentsInChildren<Transform>()[10].gameObject;
    }

    private void Update() {
        SetPreviewProperties();
    }

    /// <summary>  Sets Interactable Preview from Interactable </summary>
    /// <param name="interactable"> Interactable to set Preview with </param>
    public void SetPreviewObject(Interactable interactable)
    {

        // Set Preview if interactablePreview has changed
        if (interactable.interactablePreview == previewProps)
            return;
        else
            previewProps = interactable.interactablePreview;


        // Remove old preview object
        foreach (Transform child in previewObject.transform)
            GameObject.Destroy(child.gameObject);

        // Create Preview Gameobject
        if (interactable.interactablePreview.previewPrefab != null)
        {
            previewChild = Instantiate(interactable.interactablePreview.previewPrefab, previewObject.transform);
        }
        else
        {
            previewChild = Instantiate(interactable.gameObject, previewObject.transform);

            // Destroy InteractableUI and Remove Highlights
            Interactable previewInteractable = previewChild.GetComponent<Interactable>();
            previewInteractable.DeleteUI();
            previewInteractable.HighlightInteractableWithEffect(false);
            Destroy(previewInteractable);
        }

        // Set Layer to "InteractablePreview"
        SetLayerRecursively(previewChild, 0);
    }

    /// <summary>  Recursively set the layer for all children to "InteractablePreview" until max depth is reached or there is no more children </summary>
    public void SetLayerRecursively(GameObject gameObject, int depth)
    {
        gameObject.layer = LayerMask.NameToLayer("InteractablePreview");

        if (depth > depthMax)
            return;

        foreach (Transform child in gameObject.transform)
            SetLayerRecursively(child.gameObject, depth + 1);
    }

    /// <summary> Set interactable state with given action index </summary>
    /// <param name="actionIndex"> Action Index </param>
    public void SetPreviewAction(int actionIndex)
    {
        if (previewChild == null) return;

        Interactable interactable = previewChild.GetComponent<Interactable>();
        if (interactable == null) return;
        if (interactable.stateData == null) return;

        InteractableState state = interactable.stateData.states[actionIndex];
        state.enterState(interactable, false);

        SetLayerRecursively(previewChild, 0);
    }

    private float rotateAccumlator = 0;
    public void SetPreviewProperties()
    {
        if (previewChild == null) return;

        if (previewProps != null)
        {
            Interactable interactable = previewChild.GetComponent<Interactable>();
            if (interactable != null)
                interactable.HighlightInteractable(false);

            previewChild.transform.localPosition = previewProps.position;
            previewChild.transform.localRotation = Quaternion.Euler(previewProps.rotation);
            previewChild.transform.localScale = previewProps.scale;

            previewCamera.orthographic = previewProps.useOrthographic;
            previewCamera.fieldOfView = previewProps.fieldOfView;
            previewCamera.orthographicSize = previewProps.orthographicSize;

            if (previewProps.useRotate)
            {
                rotateAccumlator += previewProps.rotateSpeed * Time.deltaTime;
                previewChild.transform.localRotation = Quaternion.Euler(previewProps.rotation) * Quaternion.Euler(Vector3.up * rotateAccumlator);
            }
        }
    }
}

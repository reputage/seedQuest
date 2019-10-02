using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUtils {
    
    /// <summary> Sets UI Rotation </summary>
    public void SetRotation(Transform transform, Vector3 rotationOffset) {
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    /// <summary> Sets Billboarding for UI i.e. so UI follows camera </summary>
    public void BillboardInteractable(Transform transform, Vector3 rotationOffset) {
        Camera c = Camera.main;
        Vector3 targetPosition = c.transform.position - (100 * c.transform.forward);
        Vector3 interactablePosition = transform.position;
        Vector3 lookAtDir = targetPosition - interactablePosition;

        Quaternion rotate = Quaternion.LookRotation(lookAtDir);
        transform.rotation = rotate;
        transform.Rotate(rotationOffset);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
[RequireComponent(typeof(BoxCollider))]
public class DynamicBoxCollider : MonoBehaviour {

    public void Awake()
    {
        FitBoxColliderToChildren();
    }

    private void FitBoxColliderToChildren() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
            transform.gameObject.AddComponent<BoxCollider>();

        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer render in renderers) {
            if (hasBounds)
                bounds.Encapsulate(render.bounds);
            else {
                bounds = render.bounds;
                hasBounds = true;
            }
        }

        if (hasBounds) {
            boxCollider.center = bounds.center - transform.position;
            boxCollider.size = bounds.size;
        }
        else {
            boxCollider.size = boxCollider.center = Vector3.zero;
            boxCollider.size = Vector3.zero;
        }
    }
}

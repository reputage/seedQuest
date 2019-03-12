using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBehavior : MonoBehaviour
{
    private float currentScale;
    public float initialScale = 1.0f;

    bool canScale;
    public float scalePulseAmount = 0.25f;
    public float scalePulseSpeed = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        currentScale = initialScale;

        if (scalePulseAmount != 0 && scalePulseSpeed != 0)
            canScale = true;
    }

    // Update is called once per frame
    void Update() {
        if(canScale)
        {
            currentScale = Mathf.Sin(Time.time * scalePulseSpeed) * scalePulseAmount + initialScale;
            gameObject.transform.localScale = Vector3.one * currentScale;
        }
    }
}
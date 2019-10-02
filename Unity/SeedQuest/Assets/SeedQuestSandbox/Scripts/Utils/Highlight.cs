using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighlightsProps
{
    public bool useHighlightsShader = true;
    public Color highlightColor = Color.white;
    public float highlightPower = 0.2f;
    public Color rimColor = Color.white;
    public float rimExponent = 3.0f;
    public float rimPower = 0.6f;
    public Color outlineColor = Color.white;
    public float outlineWidth = 0.02f;
    public float outlinePower = 0.2f;
    public float dynamicFlashSpeed = 0.5f;
}

public class Highlight : MonoBehaviour
{
    public HighlightsProps highlightProps;
    public Shader defaultShader;
    public Shader highlightShader;

    // Start is called before the first frame update
    void Start() {
        defaultShader = Shader.Find("Lightweight Render Pipeline/Lit"); 
        highlightShader = Shader.Find("Shader Graphs/RimHighlights");
    }


    public void HighlightInteractable(bool useHighlight) {
        HighlightInteractable(useHighlight, false);
    }

    public void HighlightInteractable(bool useHighlight, bool useDynamicRim) {
        if (!highlightProps.useHighlightsShader)
            return;

        Renderer[] rendererList = transform.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in rendererList)
        {

            if (renderer.GetComponent<ParticleSystem>() != null)
                continue;

            foreach (Material material in renderer.materials)
            {
                if (useHighlight)
                {
                    material.shader = highlightShader;

                    /*
                    material.SetFloat("_HighlightPower", highlightShader.highlightPower);
                    material.SetFloat("_RimExponent", highlightShader.rimExponent);
                    material.SetFloat("_RimPower", highlightShader.rimPower);
                    material.SetFloat("_OutlineWidth", highlightShader.outlineWidth);
                    material.SetFloat("_OutlinePower", highlightShader.outlinePower);
                    material.SetFloat("_DynamicColorSpeed", highlightShader.dynamicFlashSpeed);
                    */

                    if(useDynamicRim)
                        material.SetFloat("_UseDynamicColor", 1.0f);
                    else
                        material.SetFloat("_UseDynamicColor", 0.0f);
                }
                else
                    material.shader = defaultShader;
            }
        }
    }

    public void HighlightInteractableWithEffect(bool useHighlight) {
        HighlightInteractable(true, true);

        if (useHighlight)
            EffectsManager.PlayEffect("highlight", this.transform);
        else
            EffectsManager.StopEffect(this.transform);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionPulse : MonoBehaviour {

    public float maxLevel = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Renderer rend = GetComponent<Renderer>();
        Material mat = rend.material;

        float intensity = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = new Color(maxLevel, maxLevel, maxLevel); 
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(intensity);
        mat.SetColor("_EmissionColor", finalColor);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowSpot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time, 1.0f);
        float emissionB = emission + 0.05f;
        //Debug.Log(emission);

        Color newColor = new Color32(0x18, 0xA8, 0x95, 0xFF);

        Color finalColor = newColor * Mathf.LinearToGammaSpace(emissionB);

        mat.SetColor("_EmissionColor", finalColor);
	}
}

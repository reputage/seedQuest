using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour {

    public bool glowing = false;
    public int itemID = 100000;

	// Use this for initialization
	void Start () {
        noGlow();

	}
	
	// Update is called once per frame
    void Update()
    {
        if (glowing)
        {

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

    void noGlow(){
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = 0.0f;
        //Debug.Log(emission);

        Color newColor = new Color32(0x18, 0xA8, 0x95, 0xFF);
        Color finalColor = newColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }

    public int takeItem(){
        Debug.Log("Item taken!");
        return 0;
    }

    public void activateGlow(){
        glowing = true;
    }

    public void deactivateGlow(){
        glowing = false;
        noGlow();
    }

}

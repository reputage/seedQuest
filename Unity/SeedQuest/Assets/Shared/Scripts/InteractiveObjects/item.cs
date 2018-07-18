using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class item : MonoBehaviour
{
    /* 
       This script is used for items the player can interact with.
    */

    public bool glowing = false;
    public bool emissive = false;
    public bool activeEmissive = false;
    public Light lt;
    public int itemID = 100000;
    public string itemName = "book";
    public GameObject zonePrefab;
    //(Instantiate (m_Prefab, position, rotation) as GameObject).transform.parent = parentGameObject.transform;

    void Start()
    {
        noGlow();
        if (transform.childCount <= 0)
        {
            GameObject zone;
            zone = Instantiate(zonePrefab, transform.position, Quaternion.identity) as GameObject;
            zone.transform.parent = transform;   
        }
    }

    void Update()
    {

        // Function for glowing using a point light
        if (glowing)
        {
            //Renderer renderer = lt.gameObject //lt.gameObject.transform.GetChild(PointLight);
            //Material mat = renderer.material;

            float emission = Mathf.PingPong(Time.time, 1.0f);
            float emissionB = emission - 0.05f;
            //Debug.Log(emission);

            Color newColor = new Color32(0x18, 0xA8, 0x95, 0xFF);

            Color finalColor = newColor * Mathf.LinearToGammaSpace(emissionB);

            lt.color = finalColor;

        }


        // Function for glowing using material emission

        if (emissive && activeEmissive)
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

    // Used to reset the glow effect on items
    void noGlow()
    {
        float emission = 0.0f;

        Color newColor = new Color32(0x18, 0xA8, 0x95, 0xFF);
        Color finalColor = newColor * Mathf.LinearToGammaSpace(emission);

        lt.color = finalColor;

        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        //Debug.Log(emission);

        mat.SetColor("_EmissionColor", finalColor);

    }


    public int takeItem()
    {
        Debug.Log("Item taken!");
        return 0;
    }

    // Activate the glowing effects
    public void activateGlow()
    {
        
        lt.enabled = true;
        //lt.SetActive(true);
        if (emissive)
        {
            activeEmissive = true;
        }
        else
        {
            glowing = true;
        }
    }

    // Deactivate the glowing effects
    public void deactivateGlow(){
        glowing = false;
        activeEmissive = false;
        noGlow();
    }

}

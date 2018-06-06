using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUI : MonoBehaviour {

    public GameObject Tooltip;
    public string title = "Title";
    public string label = "Label";
    public Vector3 offset = new Vector3();

    public void Start()
    {
        Tooltip.SetActive(false);
    }

    public void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Debug.Log("test");

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform != null)
            {
                Tooltip.transform.position = hit.point - 0.5f * ray.direction + offset;
            }
        }

        UnityEngine.UI.Text[] t = Tooltip.GetComponentsInChildren<UnityEngine.UI.Text>();
        t[0].text = title;
        t[1].text = label;

        Tooltip.SetActive(true);
    }

    public void OnMouseExit()
    {
        Tooltip.SetActive(false);
    }
}

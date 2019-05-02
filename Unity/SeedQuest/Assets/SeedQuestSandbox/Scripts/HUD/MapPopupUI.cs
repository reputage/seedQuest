using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeedQuest.HUD {

public class MapPopupUI : MonoBehaviour
{
    public Button mapButton;
    public Button closeButton;
    public Canvas cursorCanvas;
    public GameObject mapPanel;

    private bool hidden = true;

    private void Start()
    {
        mapButton.onClick.AddListener(mapButtonClick);
        closeButton.onClick.AddListener(closeButtonClick);
    }
    
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M) && hidden)
        {
            showMap();
        }

        else if (Input.GetKeyDown(KeyCode.M) && !hidden)
        {
            closeMap();
        }
    }

    public void mapButtonClick()
    {
        showMap();
    }

    public void closeButtonClick()
    {
        closeMap();
    }

    private void showMap()
    {
        cursorCanvas.GetComponent<Canvas>().enabled = false;
        mapPanel.SetActive(true);
        hidden = false;
    }

    private void closeMap()
    {
        cursorCanvas.GetComponent<Canvas>().enabled = true;
        mapPanel.SetActive(false);
        hidden = true;
    }
}

}
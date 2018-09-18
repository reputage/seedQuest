using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialPopupCanvas : MonoBehaviour
{
    public GameObject popup;
    public Text popupText;

    private bool oneTimePopup;

    void Start()
    {
        oneTimePopup = true;
        deactivate();
    }

    void Update()
    {
        if (GameManager.State != GameState.GameStart && oneTimePopup)
        {
            oneTimePopup = false;
            activate();
        }

        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
            deactivate();
        
        if (Input.GetKeyDown("up") || Input.GetKeyDown("down") || Input.GetKeyDown("left") || Input.GetKeyDown("right"))
            deactivate();
        
    }

    void activate()
    {
        popup.SetActive(true);
    }

    void deactivate()
    {
        popup.SetActive(false);
    }

    public void setNewText(string newText)
    {
        popupText.text = newText;
    }
}
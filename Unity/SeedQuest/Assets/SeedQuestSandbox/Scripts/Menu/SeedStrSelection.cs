using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeedQuest.Interactables;

public class SeedStrSelection : MonoBehaviour
{
    public Button weakButton;
    public Button strongButton;
    public Button crytoButton;

    public bool updateFlag = false;

    public void SetSeedStrength(int sceneCount) {
        // Set SeedStrength
        InteractableConfig.SitesPerGame = sceneCount;

        // Update UI
        Color[] colors = new Color[3];
        if (sceneCount == 1)
        {
            colors[0] = new Color(1f, 1f, 1f, 1f);
            colors[1] = new Color(1f, 1f, 1f, 0.7f);
            colors[2] = new Color(1f, 1f, 1f, 0.7f);
        }
        else if (sceneCount == 2)
        {
            colors[0] = new Color(1f, 1f, 1f, 0.7f);
            colors[1] = new Color(1f, 1f, 1f, 1f);
            colors[2] = new Color(1f, 1f, 1f, 0.7f);
        }
        else if (sceneCount == 6)
        {
            colors[0] = new Color(1f, 1f, 1f, 0.7f);
            colors[1] = new Color(1f, 1f, 1f, 0.7f);
            colors[2] = new Color(1f, 1f, 1f, 1f);
        }

        weakButton.GetComponent<Image>().color = colors[0];
        strongButton.GetComponent<Image>().color = colors[1];
        crytoButton.GetComponent<Image>().color = colors[2];

        updateFlag = true;
    }

    public void reset()
    {
        InteractableConfig.SitesPerGame = 6;

        Color[] colors = new Color[3];
        colors[0] = new Color(1f, 1f, 1f, 0.7f);
        colors[1] = new Color(1f, 1f, 1f, 0.7f);
        colors[2] = new Color(1f, 1f, 1f, 1f);

        weakButton.GetComponent<Image>().color = colors[0];
        strongButton.GetComponent<Image>().color = colors[1];
        crytoButton.GetComponent<Image>().color = colors[2];

        updateFlag = true;
    }

}
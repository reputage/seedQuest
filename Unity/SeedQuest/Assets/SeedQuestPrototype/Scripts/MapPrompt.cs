using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SeedQuest.Interactables;

public class MapPrompt : MonoBehaviour
{
    public int siteIndex = 0;
    
    void Update() {       
        
        if (siteIndex == InteractablePathManager.NextInteractableSiteID()) {
            GetComponent<CanvasGroup>().alpha = 0.75f + 0.25f * Mathf.Sin(5 * Time.time);
            GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        }
        else {
            GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(false);
        }
    }
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPopupUI : MonoBehaviour 
{
    public void Start() {
        gameObject.SetActive(false);         // Default popup inactive
    }

    public void toggleShow() {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

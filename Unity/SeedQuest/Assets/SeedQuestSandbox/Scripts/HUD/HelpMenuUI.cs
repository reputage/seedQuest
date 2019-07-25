using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuUI : MonoBehaviour
{
    static private HelpMenuUI instance = null;
    static private HelpMenuUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<HelpMenuUI>(true); return instance; }
    static public HelpMenuUI Instance { get { return instance == null ? setInstance() : instance; } }

    public void Toggle() {
        bool active = Instance.gameObject.activeSelf;
        Instance.gameObject.SetActive(!active);
    }

    public static void ToggleOn() {
        bool active = Instance.gameObject.activeSelf;
        Instance.gameObject.SetActive(true);
    }

}

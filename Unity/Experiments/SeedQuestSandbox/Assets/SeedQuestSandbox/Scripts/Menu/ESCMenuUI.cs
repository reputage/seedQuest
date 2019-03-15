using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ESCMenuUI : MonoBehaviour
{
    private Button close;
    private Button cancel;
    private Button quit;
    private Button mainmenu;

    static private ESCMenuUI instance = null;
    static private ESCMenuUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<ESCMenuUI>(true); return instance; }
    static public ESCMenuUI Instance { get { return instance == null ? setInstance() : instance; } }

    void Start() {
        Button[] buttons = GetComponentsInChildren<Button>();
        close = buttons[0];
        mainmenu = buttons[1];
        quit = buttons[2];

        close.onClick.AddListener(onCancelClick);
        quit.onClick.AddListener(onQuitClick);
        mainmenu.onClick.AddListener(onMenuMenuClick);
    }

    static public void ToggleOn() {
        if (Instance.gameObject.activeSelf)
            return;

        Instance.gameObject.SetActive(true);
        GameManager.State = GameState.Menu;
    }

    public void onCancelClick() {
        gameObject.SetActive(false);
        GameManager.State = GameManager.PrevState;
    }

    public void onQuitClick() {
        Application.Quit();
    }

    public void onMenuMenuClick() {
        if (SceneManager.GetActiveScene().name == "PrototypeSelect") {
            return;
        }

        SceneManager.LoadScene("PrototypeSelect");
    }
}

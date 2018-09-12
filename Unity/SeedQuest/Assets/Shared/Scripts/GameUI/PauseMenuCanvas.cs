using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState { PauseMenu, SettingsMenu };

public class PauseMenuCanvas : MonoBehaviour {

    public GameObject PauseMenu = null;
    public GameObject SettingsMenu = null;
    private MenuState State = MenuState.PauseMenu;

    public void Update()
    {
        switch(State) {
            case MenuState.PauseMenu:
                PauseMenu.gameObject.SetActive(true);
                SettingsMenu.gameObject.SetActive(false);
                break;
            case MenuState.SettingsMenu:
                PauseMenu.gameObject.SetActive(false);
                SettingsMenu.gameObject.SetActive(true);
                break;
            default:
                break;
        }    
    }

    public void ContinueGame()
    {
        GameManager.State = GameManager.PrevState;
    }

    public void RestartGame()
    {
        GameManager.State = GameState.GameStart;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }

    public void GoToSettingsMenu()
    {
        State = MenuState.SettingsMenu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

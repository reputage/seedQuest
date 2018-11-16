using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState { PauseMenu, SettingsMenu };

public class PauseMenuCanvas : MonoBehaviour {

    public GameObject PauseMenu = null;
    public GameObject SettingsMenu = null;

    private static PauseMenuCanvas instance = null;
    public static PauseMenuCanvas Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<PauseMenuCanvas>();
            return instance;
        }
    }

    private MenuState state = MenuState.PauseMenu;
    static public MenuState State
    {
        get { return Instance.state;  }
        set { Instance.state = value; }
    }

	private void Start()
	{
        SettingsMenu.gameObject.SetActive(false);
	}

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

    public void RestartGameToMainMenu() {
        GameManager.State = GameState.GameStart;
        SceneLoader.LoadMainMenu();
    }

    public void GoToSettingsMenu()
    {
        State = MenuState.SettingsMenu;
        GetComponent<VolumeOptionsMenu>().updateSettings();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

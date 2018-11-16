using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour {

    public Button learn;
    public Button learn_info;
    public Button recover;
    public Button recover_info;
    public Button about;
    public Button quit;

    private GameObject icon;
    private TextMeshProUGUI info_text;

    void Start() {
        info_text = GameObject.FindGameObjectWithTag("Menu Info Text").GetComponent<TextMeshProUGUI>();
        info_text.text = "";
        info_text.gameObject.SetActive(false);
        icon = GameObject.FindGameObjectWithTag("Menu Icon");


        learn.onClick.AddListener(onClickLearn);
        learn_info.onClick.AddListener(onClickLearnInfo);
        recover.onClick.AddListener(onClickRecover);
        recover_info.onClick.AddListener(onClickRecoverInfo);
        about.onClick.AddListener(onClickAbout);
        quit.onClick.AddListener(onClickQuitGame);
    } 

    public void onClickLearn()
    {
        icon.gameObject.SetActive(false);
        SceneLoader.LoadGame();
    }

    public void onClickLearnInfo()
    {
        info_text.text = "Learn: Practice game as many times as needed to memorize actions in order to recover your private key.";
        icon.gameObject.SetActive(false);
        info_text.gameObject.SetActive(true);
    }

    public void onClickRecover()
    {
        GameManager.State = GameState.LoadingRecall;

        icon.gameObject.SetActive(false);
        SceneLoader.LoadGame();
    } 

    public void onClickRecoverInfo()
    {
        info_text.text = "Recover: Play the game to recover your private key";
        icon.gameObject.SetActive(false);
        info_text.gameObject.SetActive(true);
    }

    public void onClickAbout()
    {
        info_text.text = "SeedQuest is a 3D mnemonic game used for key recovery. It is designed to be simple and fun, and it only requires a few minutes to complete. In a virtual 3D game world, the player visits a number of locations and performs various actions in a specific order. This encodes a cryptographic private key. After rehearsing the generated quest, the key can be recovered at any time by playing the game and completing the rehearsed actions in the correct sequence.";
        icon.gameObject.SetActive(false);
        info_text.gameObject.SetActive(true);
    }

    public void onClickQuitGame() {
        Application.Quit();
    }
}

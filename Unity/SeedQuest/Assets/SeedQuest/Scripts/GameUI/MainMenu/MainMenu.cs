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
    public Button start;
    public Button demo;
    public Button back;
    public TMP_InputField seed_input;

    private GameObject icon;
    private GameObject loader;
    private TextMeshProUGUI info_text;
    private TextMeshProUGUI title;
    private GameObject button_group;
    private GameObject input_group;
    private TextMeshProUGUI warning_text;
    private bool allowEnter;
    private bool entered;

    void Start() {
        info_text = GameObject.FindGameObjectWithTag("Menu Info Text").GetComponent<TextMeshProUGUI>();
        warning_text = GameObject.FindGameObjectWithTag("Warning Text").GetComponent<TextMeshProUGUI>();
        info_text.text = "";
        info_text.gameObject.SetActive(false);
        warning_text.gameObject.SetActive(false);
        icon = GameObject.FindGameObjectWithTag("Menu Icon");
        loader = GameObject.FindGameObjectWithTag("Loader");
        loader.gameObject.GetComponent<Renderer>().enabled = false;
        input_group = GameObject.FindGameObjectWithTag("Input Group");
        input_group.gameObject.SetActive(false);
        button_group = GameObject.FindGameObjectWithTag("Menu Button Group");
        title = GameObject.FindGameObjectWithTag("Menu Title").GetComponent<TextMeshProUGUI>();

        learn.onClick.AddListener(onClickLearn);
        start.onClick.AddListener(onClickStart);
        demo.onClick.AddListener(onClickStartDemo);
        back.onClick.AddListener(onClickBack);
        learn_info.onClick.AddListener(onClickLearnInfo);
        recover.onClick.AddListener(onClickRecover);
        recover_info.onClick.AddListener(onClickRecoverInfo);
        about.onClick.AddListener(onClickAbout);
        quit.onClick.AddListener(onClickQuitGame);
    }

    private void Update()
    {
        if (seed_input.text != "" && Input.GetKey(KeyCode.Return))
        {
            onClickStart();
        }
    }

    public void onClickLearn()
    {
        icon.gameObject.SetActive(false);
        button_group.gameObject.SetActive(false);
        info_text.gameObject.SetActive(false);
        input_group.gameObject.SetActive(true);
        title.text = "LEARN MODE";
    }

    public void onClickStart()
    {
        if (seed_input.text == "")
        {
            warning_text.gameObject.SetActive(true);
        }
        else
        {
            title.gameObject.SetActive(false);
            input_group.gameObject.SetActive(false);
            warning_text.gameObject.SetActive(false);
            loader.gameObject.GetComponent<Renderer>().enabled = true;
            DideryDemoManager.Instance.demoEncryptKey(seed_input.text);
            GameManager.State = GameState.LoadingRehersal;
            StartCoroutine(SceneLoader.LoadGame());
        }
    }

    public void onClickStartDemo()
    {
        title.gameObject.SetActive(false);
        input_group.gameObject.SetActive(false);
        warning_text.gameObject.SetActive(false);
        loader.gameObject.GetComponent<Renderer>().enabled = true;
        DideryDemoManager.IsDemo = true;
        DideryDemoManager.DemoBlob = "4040C1A90886218984850151AC123249";
        SeedManager.InputSeed = "A021E0A80264A33C08B6C2884AC0685C";
        GameManager.State = GameState.LoadingRehersal;
        StartCoroutine(SceneLoader.LoadGame());
    }

    public void onClickBack()
    {

        input_group.gameObject.SetActive(false);
        warning_text.gameObject.SetActive(false);
        icon.gameObject.SetActive(true);
        button_group.gameObject.SetActive(true);
        title.text = "SEED QUEST"; 
    }

    public void onClickLearnInfo()
    {
        if (info_text.gameObject.activeSelf && info_text.text.StartsWith("Learn mode"))
        {
            icon.gameObject.SetActive(true);
            info_text.gameObject.SetActive(false);
        }

        else {
            info_text.text = "Learn mode lets you input your private key to generate a key recovery “quest” — a series of actions in a 3D world. Each action in the game world is associated with bits of data. By completing the “quest” (completing the given actions in the correct order), those bits of data are strung together to reform your private key. While still in learn mode you can repeat your “quest” as many times as needed to memorize all of the actions required to complete the game. When you leave learn mode, SeedQuest will forget your private key, so make sure you have fully memorized your “quest” before quitting or switching modes.";//"Learn: Practice game as many times as needed to memorize actions in order to recover your private key.";
            icon.gameObject.SetActive(false);
            info_text.gameObject.SetActive(true);
        }
    }

    public void onClickRecover()
    {
        title.gameObject.SetActive(false);
        button_group.gameObject.SetActive(false);
        info_text.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        loader.gameObject.GetComponent<Renderer>().enabled = true;
        GameManager.State = GameState.LoadingRecall;
        StartCoroutine(SceneLoader.LoadGame());
    } 

    public void onClickRecoverInfo()
    {
        if (info_text.gameObject.activeSelf && info_text.text.StartsWith("Recover mode"))
        {
            icon.gameObject.SetActive(true);
            info_text.gameObject.SetActive(false);
        }

        else
        {
            info_text.text = "Recover mode lets you recover your private key by completing a “quest” given to you in learn mode. To successfully recover your key you will need to complete all of the actions associated with your “quest” in the correct order. Performing the wrong actions or completing actions in the wrong order will result in an incorrect key being generated by the game. Because of this it is recommended that you practice recovering your key from time to time to avoid forgetting how to complete your “quest”.";//"Recover: Play the game to recover your private key";
            icon.gameObject.SetActive(false);
            info_text.gameObject.SetActive(true);
        }
    }

    public void onClickAbout()
    {
        if (info_text.gameObject.activeSelf && info_text.text.StartsWith("SeedQuest"))
        {
            icon.gameObject.SetActive(true);
            info_text.gameObject.SetActive(false);
        }

        else
        {
            info_text.text = "SeedQuest is a 3D mnemonic game used for key recovery. It is designed to be simple and fun, and it only requires a few minutes to complete. In a virtual 3D game world, the player visits a number of locations and performs various actions in a specific order. This encodes a cryptographic private key. After rehearsing the generated “quest”, the key can be recovered at any time by playing the game and completing the rehearsed actions in the correct sequence.";
            icon.gameObject.SetActive(false);
            info_text.gameObject.SetActive(true);
        }
    }

    public void onClickQuitGame() {
        Application.Quit();
    }
}

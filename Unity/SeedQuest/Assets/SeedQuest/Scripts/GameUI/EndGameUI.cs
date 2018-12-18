using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class EndGameUI : MonoBehaviour {

    public TextMeshProUGUI seedString = null;
    public TextMeshProUGUI keyString = null;
    public DideryDemoManager dideryDemoManager;

    public Button copy;
    public Button menu;
    public Button quit;

    public TextMeshProUGUI copied;

    private string finalKey;
    public void Start()
	{

        copied = GameObject.FindGameObjectWithTag("Copied Text").GetComponent<TextMeshProUGUI>();
        copied.text = "";
        copied.gameObject.SetActive(false);
        if (DideryDemoManager.IsDemo)
        {
            keyString.text = DideryDemoManager.DemoBlob;
        }
        else
        {
            byte[] keyByte = OTPworker.decryptFromBlob(SeedManager.RecoveredSeed, dideryDemoManager.demoBlob);
            finalKey = Encoding.ASCII.GetString(keyByte);
            keyString.text = finalKey;
        }
        copy.onClick.AddListener(onClickCopyKey);
        menu.onClick.AddListener(onClickMainMenu);
        quit.onClick.AddListener(onClickQuit);
        //copyButton.SetActive(false);
    }

	public void Update() {
        /*if (GameManager.State == GameState.GameEnd)
        {
            seedString.text = SeedManager.RecoveredSeed;
            if (!DideryDemoManager.IsDemo)
                dideryDemoManager.demoGetEncryptedKey();
        }*/
    }

    public void onClickMainMenu() {
        GameManager.State = GameState.GameStart;
        StartCoroutine(SceneLoader.LoadMainMenu());
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void onClickCopyKey() {
        TextEditor editor = new TextEditor();
        editor.text = finalKey;//DideryDemoManager.DemoBlob;
        editor.SelectAll();
        editor.Copy();
        copied.text = "Your key has been copied to your clipboard.";
        copied.gameObject.SetActive(true);
    }

    public void decryptKey()
    {
        if (DideryDemoManager.IsDemo) {
            keyString.text = DideryDemoManager.DemoBlob;
        }
        else
        {
            byte[] keyByte = OTPworker.decryptFromBlob(SeedManager.RecoveredSeed, dideryDemoManager.demoBlob);
            finalKey = Encoding.ASCII.GetString(keyByte);
            keyString.text = finalKey;
        }

        //copyButton.SetActive(true);
    }

    public void onClickQuit()
    {
        Application.Quit();
    }
}

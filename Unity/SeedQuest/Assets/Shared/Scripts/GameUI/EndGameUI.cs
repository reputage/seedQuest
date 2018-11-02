using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class EndGameUI : MonoBehaviour {

    public TextMeshProUGUI seedString = null;
    public TextMeshProUGUI keyString = null;
    public DideryDemoManager dideryDemoManager;
    //public OTPworker otpWorker;


    public GameObject copyButton;

	public void Start()
	{
        //otpWorker = FindObjectOfType<OTPworker>();
        copyButton.SetActive(false);
	}

	public void Update() {
        if (GameManager.State == GameState.GameEnd)
        {
            seedString.text = SeedManager.RecoveredSeed;
            if (!DideryDemoManager.IsDemo)
                dideryDemoManager.demoGetEncryptedKey();
        }
    }

    public void RestartGame() {
        GameManager.State = GameState.GameStart;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void CopySeed() {
        TextEditor editor = new TextEditor();
        editor.text = DideryDemoManager.DemoBlob; //SeedManager.RecoveredSeed;
        editor.SelectAll();
        editor.Copy();
    }

    public void decryptKey()
    {
        if (DideryDemoManager.IsDemo) {
            keyString.text = DideryDemoManager.DemoBlob;
        }
        else
        {
            byte[] keyByte = OTPworker.decryptFromBlob(SeedManager.RecoveredSeed, dideryDemoManager.demoBlob);
            string finalKey = Encoding.ASCII.GetString(keyByte);
            keyString.text = finalKey;
        }

        copyButton.SetActive(true);
    }
}

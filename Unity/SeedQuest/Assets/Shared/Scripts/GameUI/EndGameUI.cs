using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class EndGameUI : MonoBehaviour {

    public TextMeshProUGUI seedString = null;
    public TextMeshProUGUI keyString = null;
    public OTPworker otpWorker;

    public void Update() {
        if (GameManager.State == GameState.GameEnd)
        {
            seedString.text = SeedManager.RecoveredSeed;
            otpWorker.getEncryptedKey();
        }
    }

    public void RestartGame() {
        GameManager.State = GameState.GameStart;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void CopySeed() {
        TextEditor editor = new TextEditor();
        editor.text = SeedManager.RecoveredSeed;
        editor.SelectAll();
        editor.Copy();
    }

    public void decryptKey()
    {
        byte[] keyByte = otpWorker.decryptFromBlob(SeedManager.RecoveredSeed);
        string finalKey = Encoding.ASCII.GetString(keyByte);
        keyString.text = finalKey;
    }
}

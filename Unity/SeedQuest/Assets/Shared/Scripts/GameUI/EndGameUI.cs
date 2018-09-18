using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameUI : MonoBehaviour {

    public TextMeshProUGUI seedString = null;

    public void Update() {
        if (GameManager.State == GameState.GameEnd)
            seedString.text = SeedManager.RecoveredSeed;
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
}

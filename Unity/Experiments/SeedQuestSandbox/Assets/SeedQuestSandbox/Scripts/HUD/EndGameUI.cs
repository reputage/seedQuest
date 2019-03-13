using UnityEngine;
using UnityEngine.SceneManagement;

using SeedQuest.Interactables;

public class EndGameUI : MonoBehaviour {

    static private EndGameUI instance = null;

    static public EndGameUI Instance  {
        get {
            if (instance == null)
                instance = Resources.FindObjectsOfTypeAll<EndGameUI>()[0];
            return instance;
        }
    }

    /// <summary> Toggles On the EndGameUI </summary>
    static public void ToggleOn() {
        if (Instance.gameObject.activeSelf)
            return;
        
        Instance.gameObject.SetActive(true);
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        textList[0].text = InteractablePathManager.SeedString;

        if (GameManager.Mode == GameMode.Rehearsal)
            textList[1].text = "Rehearsal Complete! \n Need more practice? Select Rehearsal mode. \n Ready to go? Select Recall";
        else
            textList[1].text = "Key is Recovered!";
    }

    /// <summary> Handles selecting PrototypeSelect Button </summary>
    public void PrototypeSelect() {
        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();
        SceneManager.LoadScene("PrototypeSelect");
    }

    /// <summary> Handles selecting Recall Button </summary>
    public void Recall() {
        GameManager.Mode = GameMode.Recall;
        GameManager.State = GameState.Play;

        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary> Handles selecting Rehearsal Button </summary>
    public void Rehearsal() {
        GameManager.Mode = GameMode.Rehearsal;
        GameManager.State = GameState.Play;

        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();
        
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
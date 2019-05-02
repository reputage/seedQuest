using UnityEngine;
using UnityEngine.SceneManagement;

using SeedQuest.Interactables;
using SeedQuest.SeedEncoder;

public class EndGameUI : MonoBehaviour {

    static private EndGameUI instance = null;
    static private EndGameUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<EndGameUI>(true); return instance; }
    static public EndGameUI Instance { get { return instance == null ? setInstance() : instance; } }

    public string PrototypeSelectScene = "PrototypeSelect";
    public string RehearsalScene = "PrototypeSelect";
    public string RecallScene = "PrototypeSelect";

    /// <summary> Toggles On the EndGameUI </summary>
    static public void ToggleOn() {
        if (Instance.gameObject.activeSelf)
            return;
        
        Instance.gameObject.SetActive(true);
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        SeedConverter converter = new SeedConverter();
        textList[0].text = converter.DecodeSeed();

        if (GameManager.Mode == GameMode.Rehearsal)
            textList[1].text = "Practice Complete!";
        else
            textList[1].text = "Seed is Recovered!";
    }

    /// <summary> Handles selecting PrototypeSelect Button </summary>
    public void PrototypeSelect() {
        LoadingScreenUI.LoadScene(PrototypeSelectScene, true);
    }

    /// <summary> Handles selecting Rehearsal Button </summary>
    public void Rehearsal() {
        LoadingScreenUI.LoadRehearsal(RehearsalScene, true);
    }

    /// <summary> Handles selecting Recall Button </summary>
    public void Recall() {
        LoadingScreenUI.LoadRecall(RecallScene, true);
    }

    public void GoToStartScreen() {
        MenuScreenManager.ActivateStart();
        gameObject.SetActive(false);
    }

}
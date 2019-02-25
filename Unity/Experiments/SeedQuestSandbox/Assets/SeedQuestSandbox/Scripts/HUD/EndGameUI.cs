using UnityEngine;
using UnityEngine.SceneManagement;

using SeedQuest.Interactables;

public class EndGameUI : MonoBehaviour {

    static private EndGameUI instance = null;

    static public EndGameUI Instance  {
        get {
            if (instance == null)
                instance = Resources.FindObjectsOfTypeAll<EndGameUI>()[0];
            else if(instance == null)
                instance = Instantiate(GameManager.Instance.HUDEndGamePrefab).GetComponent<EndGameUI>();
            return instance;
        }
    }

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

    static public void Toggle() {
        Instance.gameObject.SetActive(!Instance.gameObject.activeSelf);

        if(Instance.gameObject.activeSelf) {
            var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            textList[0].text = InteractablePathManager.SeedString; 
        }
    }

    public void PrototypeSelect() {
        SceneManager.LoadScene("PrototypeSelect");
    }

    public void Recall() {
        GameManager.Mode = GameMode.Recall;
        GameManager.State = GameState.Play;

        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Rehearsal()
    {
        GameManager.Mode = GameMode.Rehearsal;
        GameManager.State = GameState.Play;

        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();
        
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
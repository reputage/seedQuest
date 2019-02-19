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
        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Rehearsal()
    {
        GameManager.Mode = GameMode.Rehearsal;
        InteractablePathManager.InitalizePathAndLog();
        InteractableManager.destroyInteractables();
        
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
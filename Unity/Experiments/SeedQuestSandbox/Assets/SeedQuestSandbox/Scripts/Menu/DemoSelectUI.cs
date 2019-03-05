using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using SeedQuest.Interactables;

[System.Serializable]
public class DemoInfo {
    public string name;
    public string sceneName;
    public string demoTitle;
    public string demoText;
    public Sprite demoImage;
    public string[] demoPopupInfo;

    [HideInInspector]
    public Button select;
}

public class DemoSelectUI : MonoBehaviour {

    public DemoInfo[] demoList;
    public DemoInfo selectedDemo;
    public GameObject selectButtonPrefab; 
    public Vector3 buttonOffset = new Vector3(170, 250, 0);
    public int buttonPadding = 60;

    private void Start() {
        GameManager.ResetCursor();
        InteractableManager.Reset();
        InteractablePathManager.Reset();

        GameObject sideNav = GameObject.FindGameObjectWithTag("SideNav");

        for (int i = 0; i < demoList.Length; i++) {
            Vector3 position = buttonOffset + new Vector3(0, -i * buttonPadding, 0);
            demoList[i].select = createLevelButton(demoList[i], sideNav.transform, position);
        }

        demoList[0].select.onClick.Invoke();
    }

    private void selectDemo(DemoInfo info) {

        TextMeshProUGUI infoTitle = GameObject.FindGameObjectWithTag("InfoTitle").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI infoText = GameObject.FindGameObjectWithTag("InfoText").GetComponentInChildren<TextMeshProUGUI>();
        Image infoImage = GameObject.FindGameObjectWithTag("InfoImage").GetComponent<Image>();
        PopupUI popup = PopupUI.Instance;

        // Set demo title, info text, and image
        selectedDemo = info;
        infoTitle.text = info.demoTitle;
        infoText.text = info.demoText;
        infoImage.sprite = info.demoImage;

        // Set popup text
        popup.popupText = "";
        foreach (string text in info.demoPopupInfo)
            popup.popupText += text + "\n";

        // Set button image highlight for selected demo button
        foreach (DemoInfo _ in demoList) {
            _.select.image.sprite = null;
        }
        info.select.image.sprite = info.select.spriteState.highlightedSprite;
    }

    public void startDemo() {
        string sceneName = selectedDemo.sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void StartDemoWithRehearsalMode()
    {
        GameManager.Mode = GameMode.Rehearsal;
        string sceneName = selectedDemo.sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void StartDemoWithRecallMode()
    {
        GameManager.Mode = GameMode.Recall;
        string sceneName = selectedDemo.sceneName;
        SceneManager.LoadScene(sceneName);
    }

    private Button createLevelButton(DemoInfo info, Transform parent, Vector3 position) {
        GameObject buttonObj = Instantiate(selectButtonPrefab);
        buttonObj.transform.SetParent(parent);
        buttonObj.GetComponent<RectTransform>().anchoredPosition3D = position;
        buttonObj.name = info.name + " Button";

        TMPro.TextMeshProUGUI text = buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.text = info.name;

        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener( delegate { selectDemo(info); } );
        return button;
    }
}
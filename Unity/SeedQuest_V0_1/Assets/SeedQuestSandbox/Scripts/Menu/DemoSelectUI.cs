using System.Collections;
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

    private Button[] buttons;
    private TMP_InputField seedInputField;
    private TextMeshProUGUI infoName;
    private TextMeshProUGUI infoTitle;
    private TextMeshProUGUI infoText;
    private Image infoImage;
    private RectTransform welcome;

    private void Start() {
        GameManager.ResetCursor();

        infoName = GetComponentsInChildren<Canvas>()[3].GetComponentInChildren<TextMeshProUGUI>();
        infoTitle = GameObject.FindGameObjectWithTag("InfoTitle").GetComponentInChildren<TextMeshProUGUI>();
        infoText = GameObject.FindGameObjectWithTag("InfoText").GetComponentInChildren<TextMeshProUGUI>();
        infoImage = GameObject.FindGameObjectWithTag("InfoImage").GetComponent<Image>();
        welcome = infoTitle.GetComponentInParent<Canvas>().GetComponentsInChildren<RectTransform>(true)[5];
        buttons = infoTitle.GetComponentInParent<Canvas>().gameObject.GetComponentsInChildren<Button>();
        seedInputField = GetComponentInChildren<TMP_InputField>();
        seedInputField.text = InteractablePathManager.SeedString;
        seedInputField.characterLimit = InteractableConfig.SeedHexLength; 

        GameObject sideNav = GameObject.FindGameObjectWithTag("SideNav");
        for (int i = 0; i < demoList.Length; i++) {
            Vector3 position = buttonOffset + new Vector3(0, -i * buttonPadding, 0);
            demoList[i].select = createLevelButton(demoList[i], sideNav.transform, position);
        }

        demoList[0].select.onClick.Invoke();
    }

    private void selectDemo(DemoInfo info) {
        PopupUI popup = PopupUI.Instance;

        // Set demo title, info text, and image
        selectedDemo = info;
        infoName.text = info.name;
        infoTitle.text = info.demoTitle;
        infoText.text = info.demoText;
        infoImage.sprite = info.demoImage;

        // Set popup text
        popup.popupText = "";
        foreach (string text in info.demoPopupInfo)
            popup.popupText += text + "\n";

        // Set button image highlight for selected demo button
        foreach (DemoInfo _ in demoList)
        {
            _.select.image.sprite = null;
        }
        info.select.image.sprite = info.select.spriteState.highlightedSprite;

        SetupSurveySelect(info);
    }

    public void SetupSurveySelect(DemoInfo info) {

        welcome.gameObject.SetActive(false);
        infoTitle.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
        infoImage.gameObject.SetActive(true);

        if (info.name == "Survey") {
            buttons[0].gameObject.SetActive(false);
            buttons[1].gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(false);
            buttons[3].gameObject.SetActive(true);
        }
        else if(info.name == "Welcome") {
            welcome.gameObject.SetActive(true);
            infoTitle.gameObject.SetActive(false);
            infoText.gameObject.SetActive(false);
            infoImage.gameObject.SetActive(false);
            buttons[0].gameObject.SetActive(false);
            buttons[1].gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(false);
            buttons[3].gameObject.SetActive(false);  
        }
        else {
            buttons[0].gameObject.SetActive(true);
            buttons[1].gameObject.SetActive(true);
            buttons[2].gameObject.SetActive(true);
            buttons[3].gameObject.SetActive(false);
        }
    }

    public void startDemo() {
        string sceneName = selectedDemo.sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public bool CheckValidSeed() {
        bool valid = seedInputField.text.Length == InteractableConfig.SeedHexLength && SeedQuest.Utils.StringUtils.CheckIfValidHex(seedInputField.text);
        if (!valid)
            GetComponentInChildren<CardPopupUI>(true).toggleShow();

        return valid;
    }

    public void StartDemoWithRehearsalMode() {
        if (!CheckValidSeed())
            return;
        InteractablePathManager.SeedString = seedInputField.text;
        string sceneName = selectedDemo.sceneName;
        LoadingScreenUI.LoadRehearsal(sceneName, true);
    }

    public void StartDemoWithRecallMode() {
        if (!CheckValidSeed())
            return;
        InteractablePathManager.SeedString = seedInputField.text;
        string sceneName = selectedDemo.sceneName;
        LoadingScreenUI.LoadRecall(sceneName, true);
    } 

    private Button createLevelButton(DemoInfo info, Transform parent, Vector3 position) {
        GameObject buttonObj = Instantiate(selectButtonPrefab);
        buttonObj.transform.SetParent(parent, false);
        buttonObj.GetComponent<RectTransform>().anchoredPosition3D = position;
        buttonObj.name = info.name + " Button";

        TMPro.TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        text.text = info.name;

        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(delegate { selectDemo(info); });
        return button;
    }

    public void SetRandomSeed() {
        InteractablePathManager.SetRandomSeed();
        seedInputField.text = InteractablePathManager.SeedString;
    }
}
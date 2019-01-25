using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class DemoInfo {
    public string name;
    public string sceneName;
    public string demoTitle;
    public string demoText;
    public Sprite demoImage;
}

public class DemoSelectUI : MonoBehaviour {

    public DemoInfo[] demoList;
    public DemoInfo selectedDemo;
    public GameObject selectButtonPrefab;
    public Vector3 buttonOffset = new Vector3(170, 250, 0);
    public int buttonPadding = 60;

    private void Start() {

        selectedDemo = demoList[0];
        GameObject sideNav = GameObject.FindGameObjectWithTag("SideNav");

        for (int i = 0; i < demoList.Length; i++) {
            Vector3 position = buttonOffset + new Vector3(0, -i * buttonPadding, 0);
            Debug.Log(position);
            createLevelButton(demoList[i], sideNav.transform, position);
        }
    }

    private void selectDemo(DemoInfo info) {

        TextMeshProUGUI infoTitle = GameObject.FindGameObjectWithTag("InfoTitle").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI infoText = GameObject.FindGameObjectWithTag("InfoText").GetComponentInChildren<TextMeshProUGUI>();
        Image infoImage = GameObject.FindGameObjectWithTag("InfoImage").GetComponent<Image>();
       
        infoTitle.text = info.demoTitle;
        infoText.text = info.demoText;
        infoImage.sprite = info.demoImage;

        //string sceneName = info.sceneName;
        //SceneManager.LoadScene(sceneName);
    }

    private GameObject createLevelButton(DemoInfo info, Transform parent, Vector3 position) {
        GameObject buttonObj = Instantiate(selectButtonPrefab);
        buttonObj.transform.parent = parent;
        buttonObj.GetComponent<RectTransform>().anchoredPosition3D = position;
        buttonObj.name = info.name + " Button";

        TMPro.TextMeshProUGUI text = buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.text = info.name;

        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener( delegate { selectDemo(info); } );
        return buttonObj;
    }

}

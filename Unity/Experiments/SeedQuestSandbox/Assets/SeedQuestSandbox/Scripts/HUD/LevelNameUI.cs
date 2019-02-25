using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using SeedQuest.Level;

[System.Serializable]
public class LevelNameProps {
    public Sprite background;
    public float opacity = 1.0f;
}

public class LevelNameUI : MonoBehaviour
{
    public LevelNameProps[] backgrounds;
    public int backgroundIndex = 0;

    private Image backgroundImage;
    private TextMeshProUGUI levelNameUI;
    private TextMeshProUGUI zoneNameui;
    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        backgroundImage = GetComponentInChildren<Image>();
        TextMeshProUGUI[] textmesh = GetComponentsInChildren<TextMeshProUGUI>();
        levelNameUI = textmesh[0];
        zoneNameui = textmesh[1];
    }

    private void Update() {
        UpdateText();
        UpdateBackground();
    }

    void UpdateText() {
        levelNameUI.text = LevelManager.LevelName;

        BoundingBox bb = LevelManager.GetBoundingBoxPlayerIsIn();
        if (bb != null)
            zoneNameui.text = bb.name;
        else
            zoneNameui.text = "";
    } 

    void UpdateBackground() {
        backgroundImage.sprite = backgrounds[backgroundIndex].background;
        var color = backgroundImage.color;
        color.a = backgrounds[backgroundIndex].opacity;
        backgroundImage.color = color;
    }
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using SeedQuest.Level;

public class LevelNameUI : MonoBehaviour
{
    private TextMeshProUGUI levelNameUI;
    private TextMeshProUGUI zoneNameui;
    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        TextMeshProUGUI[] textmesh = GetComponentsInChildren<TextMeshProUGUI>();
        levelNameUI = textmesh[0];
        zoneNameui = textmesh[1];
    }

    private void Update() {
        UpdateText();
    }

    void UpdateText() {
        levelNameUI.text = LevelManager.LevelName;

        BoundingBox bb = LevelManager.GetBoundingBoxPlayerIsIn();
        if (bb != null)
            zoneNameui.text = bb.name;
        else
            zoneNameui.text = "";
    } 
} 
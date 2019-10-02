using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using SeedQuest.Interactables;
using SeedQuest.SeedEncoder;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Runtime.InteropServices;
using QRCoder;
using QRCoder.Unity;
using sharpPDF;

public class EndGameUI_v2 : MonoBehaviour
{
    #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void Copy(string copy_str);

        [DllImport("__Internal")]
        private static extern void Download(string file, string content);

        [DllImport("__Internal")]
        private static extern void Print(string str);
    #endif

    static private EndGameUI_v2 instance = null;
    static private EndGameUI_v2 setInstance() { instance = HUDManager.Instance.GetComponentInChildren<EndGameUI_v2>(true); return instance; }
    static public EndGameUI_v2 Instance { get { return instance == null ? setInstance() : instance; } }

    public string PrototypeSelectScene = "PrototypeSelect";
    public string RehearsalScene = "PrototypeSelect";
    public string RecallScene = "PrototypeSelect";

    public GameObject wordsButton;
    public GameObject characterButton;

    private static string hexSeed;
    private static string bipSeed;

    /// <summary> Toggles On the EndGameUI </summary>
    static public void ToggleOn()
    {
        if (Instance.gameObject.activeSelf)
            return;

        Instance.gameObject.SetActive(true);
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        SeedConverter converter = new SeedConverter();
        BIP39Converter bpc = new BIP39Converter();
        //textList[0].text = converter.DecodeSeed();

        /*        for (int i = 0; i < textList.Length; i++)
                {
                    Debug.Log("Text data #" + i + ": " + textList[i].text);
                }
        */
        if (InteractableConfig.SeedHexLength % 2 == 1)
        {
            string alteredSeedText = converter.DecodeSeed();
            string sentence = bpc.getSentenceFromHex(alteredSeedText);

            char[] array = alteredSeedText.ToCharArray();
            array[array.Length - 2] = array[array.Length - 1];
            alteredSeedText = new string(array);
            if (alteredSeedText.Length > 1)
                alteredSeedText = alteredSeedText.Substring(0, (alteredSeedText.Length - 1));

            hexSeed = alteredSeedText;
            bipSeed = sentence;
            textList[0].text = sentence;
        }
        else
        {
            //textList[0].text = converter.DecodeSeed();
            string hex = converter.DecodeSeed();
            string sentence = bpc.getSentenceFromHex(hex);
            hexSeed = hex;
            bipSeed = sentence;
            textList[0].text = sentence;
        }

        //Debug.Log("Hex: " + hexSeed);
        //Debug.Log("Bip: " + bipSeed);

        if (GameManager.Mode == GameMode.Rehearsal)
        {
            //textList[2].text = "Key Learned!";
            textList[3].text = "Practice Again";
        }
        else
        {
            //textList[2].text = "Key Recovered!";
            textList[3].text = "Try Again";
        }

        for (int i = 0; i < textList.Length; i++)
        {
            //Debug.Log("Text data #" + i + ": " + textList[i].text);
        }

        setupCharacterMode();
    }

    /// <summary> Handles selecting PrototypeSelect Button </summary>
    public void PrototypeSelect()
    {
        LoadingScreenUI.LoadScene(PrototypeSelectScene, true);
    }

    /// <summary> Handles selecting Rehearsal Button </summary>
    public void Rehearsal()
    {
        LoadingScreenUI.LoadRehearsal(RehearsalScene, true);
    }

    /// <summary> Handles selecting Recall Button </summary>
    public void Recall()
    {
        LoadingScreenUI.LoadRecall(RecallScene, true);
    }

    public void GoToStartScreen()
    {
        SeedQuest.Level.LevelManager.Instance.StopLevelMusic();
        MenuScreenManager.ActivateStart();
        gameObject.SetActive(false);
        GameManager.ResetGraduatedRehearsal();
    }

    public void ResetPlaythrough()
    {
        InteractablePathManager.Reset();
        MenuScreenManager.ActivateSceneLineUp();
        gameObject.SetActive(false);
    }

    public void copySeed()
    {
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        string seed = textList[0].text;

    #if UNITY_WEBGL
            Copy(seed);
    #else
        GUIUtility.systemCopyBuffer = seed;
    #endif

        textList[1].text = "Seed Copied!";
        textList[1].gameObject.SetActive(true);
    }

    public void copyHexSeed()
    {
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        BIP39Converter bpc = new BIP39Converter();
        string seed = bpc.getHexFromSentence(textList[0].text);

    #if UNITY_WEBGL
            Copy(seed);
    #else
        GUIUtility.systemCopyBuffer = seed;
    #endif

        textList[1].text = "Seed Copied";
        textList[1].gameObject.SetActive(true);
    }

    public void downloadSeed()
    {
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        BIP39Converter bpc = new BIP39Converter();
        string seed = bipSeed + "\n0x" + hexSeed;

    #if UNITY_WEBGL
            Download("seed.txt", seed);
    #elif UNITY_EDITOR
        string path = EditorUtility.SaveFilePanel("Save As", "Downloads", "seed", "txt");
        if (path.Length != 0)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.WriteLine(seed);
            }
        }
    #else
            string downloads = "";
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                string home = System.Environment.GetEnvironmentVariable("HOME");
                downloads = System.IO.Path.Combine(home, "Downloads");
            }
            else
            {
                downloads = System.Convert.ToString(Microsoft.Win32.Registry.GetValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                    , "{374DE290-123F-4565-9164-39C4925E467B}"
                    , String.Empty));
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(downloads, "seed.txt")))
            {
                outputFile.WriteLine(seed);
            }
    #endif
        //textList[1].text = "Seed Downloaded";
        //textList[1].gameObject.SetActive(true);
    }

    public void wordsMode()
    {
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        textList[0].text = bipSeed;

        characterButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(89, 89, 89, 255);
        characterButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        wordsButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        wordsButton.GetComponent<Image>().color = new Color32(55, 90, 122, 255);

    }

    public static void setupCharacterMode()
    {
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        textList[0].text = hexSeed;
    }

    public void characterMode()
    {
        var textList = Instance.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        textList[0].text = hexSeed;

        characterButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        characterButton.GetComponent<Image>().color = new Color32(55, 90, 122, 255);
        wordsButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(89, 89, 89, 255);
        wordsButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }



}
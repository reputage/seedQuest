using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpMenuUI : MonoBehaviour
{
    static private HelpMenuUI instance = null;
    static private HelpMenuUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<HelpMenuUI>(true); return instance; }
    static public HelpMenuUI Instance { get { return instance == null ? setInstance() : instance; } }

    private TextMeshProUGUI helpText;
    public string characterSeedText;
    public string privateKeyText;
    public string encryptionText;
    public string wordSeedText;

    private Animator animator;

    public void Awake() {
        helpText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        animator = GetComponent<Animator>();
    }

    public void Start() {
        helpText.text = characterSeedText;
    }

    public void Toggle() {
        bool active = Instance.gameObject.activeSelf;
        Instance.gameObject.SetActive(!active);
    }

    public static void ToggleOn() {
        Instance.animator = Instance.GetComponent<Animator>();
        Instance.animator.Play("Default");
        Instance.gameObject.SetActive(true);
        Instance.animator.Play("SlideUp");
    }

    public void ToggleOff() {
        gameObject.SetActive(false);
    }

    public void SelectCharacterSeedText() {
        helpText.text = characterSeedText;
    }

    public void SelectPrivateKeyText() {
        helpText.text = privateKeyText;
    }

    public void SelectEncryptionText() {
        helpText.text = encryptionText;
    }

    public void SelectWordSeedText() {
        helpText.text = wordSeedText;
    }
}
using System.Collections;
using System.Collections.Generic;
using SeedQuest.Interactables;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    static private PauseMenuUI instance = null;
    static private PauseMenuUI setInstance() { instance = HUDManager.Instance.GetComponentInChildren<PauseMenuUI>(true); return instance; }
    static public PauseMenuUI Instance { get { return instance == null ? setInstance() : instance; } }

    public Animator animator;

    public void Awake() {
        animator = GetComponent<Animator>();
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

    public void ExitToMainMenu()
    {
        SeedQuest.Level.LevelManager.Instance.StopLevelMusic();
        InteractablePathManager.Reset();
        if (GameManager.V2Menus)
        {
            MenuScreenV2.Instance.GoToStart();
        }
        else
            MenuScreenManager.ActivateStart();
        //gameObject.SetActive(false);
        animator.Play("SlideDown");
        GameManager.GraduatedMode = false;
    }

    public void Quit() {
        Application.Quit();
    }
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenCanvas : MonoBehaviour
{
    private Animator animator;

    public void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Start() {
        Invoke("StartAnimation", 0.1f);
        AudioManager.Play("MenuStart");
    }

    private void StartAnimation() {
        animator.Play("StartMenuAnimation");
    }

    public void StartIdleAnimation() {
        animator.Play("StartMenu_Idle");
    }

    public void OnClickMenuButton() {
        PauseMenuUI.ToggleOn();
    }

    public void OnClickHelpButton() {
        HelpMenuUI.ToggleOn();
    }

    public void StartTutorial() {
        StartCoroutine(LoadAsync("NonnaISO"));
    }

    public void HideKey() {
        MenuScreenV2.Instance.SetModeLearnSeed();
    }

    public void FindKey() {
        MenuScreenV2.Instance.SetModeRecoverSeed();
    }

    IEnumerator LoadAsync(string sceneName) {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone) {

            if (operation.progress >= 0.9f) {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum GameState { Sandbox, Pause, Interact }

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;
    public static GameManager Instance {
        get  {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public GameState state = GameState.Sandbox;
    public GameState prevState = GameState.Sandbox;
    public static GameState State {
        get { return Instance.state; }
        set { if (value == Instance.state) return; Instance.prevState = Instance.state; Instance.state = value; }
    }
    public static GameState PrevState {
        get { return Instance.prevState; }
    }

    public GameSoundData gameSound = null;
    public static GameSoundData GameSound { get { return Instance.gameSound; } }

    public void Update() {
        CheckButtonClick();
        ListenForKeyDown();
        CheckForEndGame();
    }

    public void CheckForEndGame() {

    }

    public void ListenForKeyDown() {
        if (Input.GetKeyDown("escape")) {

        }
    }

    public void CheckButtonClick() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            ClickButtons();
        }
    }

    public void ClickButtons()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f)) {
                IPointerClickHandler clickHandler = hit.transform.gameObject.GetComponent<IPointerClickHandler>();
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                if (clickHandler != null) {
                    clickHandler.OnPointerClick(pointerEventData);
                }
            }
        }
    }
}

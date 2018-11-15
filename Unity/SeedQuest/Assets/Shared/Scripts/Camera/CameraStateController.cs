using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateController : MonoBehaviour {
    
    public Camera StartCam;
    public Camera MainCam;

    static private CameraStateController __instance = null;
    static public CameraStateController instance {
        get {
            if (__instance == null)
                __instance = GameObject.FindObjectOfType<CameraStateController>();
            return __instance;
        }
    }

    private void Update() {
        SetActiveCamera();
    }

    private void SetActiveCamera()
    {
        switch (GameManager.State)
        {
            case GameState.GameStart:
                StartCam.gameObject.SetActive(true);
                MainCam.gameObject.SetActive(false);
                break;
            case GameState.Rehearsal:
                StartCam.gameObject.SetActive(false);
                MainCam.gameObject.SetActive(true);
                break;
            case GameState.Recall:
                StartCam.gameObject.SetActive(false);
                MainCam.gameObject.SetActive(true);
                break;
            case GameState.GameEnd:
                StartCam.gameObject.SetActive(false);
                MainCam.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }


    static public Transform Transform
    {
        get { return instance.MainCam.transform; }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateController : MonoBehaviour {
    
    public Camera StartCam;
    public Camera MainCam;
    public GameStateData playerPathData;
    private bool mainCamActive = false;

    private void Update()
    {
        if (playerPathData.startPathSearch)
            SetMainCamera();
    }

    public void SetMainCamera()
    {
        mainCamActive = true;
        MainCam.gameObject.SetActive(mainCamActive);
        StartCam.gameObject.SetActive(!mainCamActive);
    } 
}

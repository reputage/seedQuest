using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class compassNSEW : MonoBehaviour {

    public RawImage image;

    void Update () 
    {
        Transform angles = CameraStateController.Transform;
        image.uvRect = new Rect(angles.localEulerAngles.y / 360f, 0, 1, 1);
	}
}

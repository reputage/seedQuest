using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class compassNSEW : MonoBehaviour {

    public RawImage image;

    void Update () 
    {
        image.uvRect = new Rect(PlayerManager.Transform.localEulerAngles.y / 360f, 0, 1, 1);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class compassNSEW : MonoBehaviour {

    public Transform player;

    public RawImage image;


    void Start () 
    {

    }
	

    void Update () 
    {
        image.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);
	}
}

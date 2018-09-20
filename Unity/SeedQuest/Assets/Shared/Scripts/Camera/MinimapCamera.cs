using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapCamera : MonoBehaviour {

    public GameObject target;

    public Vector3 cameraOffset;

    public int resWidth = 2550;
    public int resHeight = 2550;


	void Start () 
    {
        cameraOffset.y = 100;
        initializeRes();
	}
	

	void LateUpdate () 
    {
        transform.position = PlayerManager.Transform.position;
        transform.position += cameraOffset;
	}

    void initializeRes()
    {
        Camera cameraRef = GetComponent<Camera>();
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cameraRef.targetTexture = rt;
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cameraRef.Render();
    }
}

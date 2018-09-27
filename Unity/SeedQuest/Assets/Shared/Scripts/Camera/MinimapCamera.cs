using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapCamera : MonoBehaviour {

    public GameObject target;

    public Vector3 cameraOffset;
    public Image screenShotRender;

    public int resWidth = 2550;
    public int resHeight = 2550;


	void Start () 
    {
        cameraOffset.y = 100;
        screenShotRender.enabled = false;

        // Comment this line out if you want to just use the camera, 
        //  and not the rendered map image
        initializeImage();
	}
	

	void LateUpdate () 
    {
        transform.position = PlayerManager.Transform.position;
        transform.position += cameraOffset;
	}

    // Render an image of the map by having the camera take a "screenshot"
    //  of the world from overhead, then assigning that screenshot
    //  to a UI image which will move itself.
    void initializeImage()
    {
        Camera cameraRef = GetComponent<Camera>();
        cameraRef.transform.position = new Vector3(0, 500, 0);

        RenderTexture other = cameraRef.targetTexture;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cameraRef.targetTexture = rt;
        RenderTexture.active = rt;

        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cameraRef.Render();
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        screenShotRender.GetComponent<Image>().sprite = Sprite.Create(screenShot, new Rect(0, 0, resWidth, resHeight), new Vector2(0, 0));

        cameraRef.targetTexture = null;
        RenderTexture.active = null;
        screenShotRender.enabled = true;

        cameraRef.targetTexture = other;
        cameraRef.enabled = false;
    }
}

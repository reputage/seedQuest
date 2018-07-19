using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public float Y_ANGLE_MIN = 0.0f;
    public float Y_ANGLE_MAX = 50.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float distance = 6.0f;

    public Transform lookAt;

    public bool inverted = false;
    public float mouseSensitivityX = 5.0f;
    public float mouseSensitivityY = 2.0f;
    public float keySensitivityX = 50.0f;
    public Vector3 offset = new Vector3(0.0f, 2.0f, 0.0f);
    public Vector3 lookAtOffset = new Vector3(0.0f, 2.0f, 0.0f);

	// Use this for initialization
	void Start () {
	}

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        currentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
        if(inverted)
            currentY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        else
            currentY += -Input.GetAxis("Mouse Y") * mouseSensitivityY;
        
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    // Update is called once per frame
    void LateUpdate () {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * dir + offset;
        transform.LookAt(lookAt.position + lookAtOffset);
	}

    public Texture2D cursorImage;
    private void OnGUI()
    {
        //Vector3 mPos = Input.mousePosition;
        //GUI.DrawTexture(new Rect(mPos.x - 32, Screen.height - mPos.y - 32, 64, 64), cursorImage);
    }
}

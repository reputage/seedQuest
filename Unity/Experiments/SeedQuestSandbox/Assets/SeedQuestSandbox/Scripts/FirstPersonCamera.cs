using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstPersonCamera : MonoBehaviour {

    public float xSpeed = 1f;
    public float ySpeed = 1f;

    private Transform characterTransform;
    private Transform cameraTransform;

    public void Update() {
        UpdateRotation();
    }

    public void UpdateRotation() {
        float horizontal = Input.GetAxis("Mouse X") * xSpeed * SettingsManager.CameraSensitivity;
        float vertical = Input.GetAxis("Mouse Y") * ySpeed * SettingsManager.CameraSensitivity;
        vertical = Mathf.Clamp(vertical, -80, 80);

        characterTransform = transform;
        cameraTransform = GetComponentInChildren<Camera>().transform;
        characterTransform.localRotation *= Quaternion.Euler(0f, horizontal, 0f);
        cameraTransform.localRotation *= Quaternion.Euler(-vertical, 0f, 0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 

    public void ClickButtons() {
        if(Input.GetMouseButtonDown(0)) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                IPointerClickHandler clickHandler = hit.transform.gameObject.GetComponent<IPointerClickHandler>();
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                if(clickHandler != null) {
                    clickHandler.OnPointerClick(pointerEventData);
                    Debug.Log("Click - " + hit.transform.name);
                }
            } 
        }
    } 
}
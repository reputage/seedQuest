using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSlider : MonoBehaviour
{
    private Slider slider;
    public void Start() {
        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
    }

    public void Update() {
        ListenForKeyDown();
    }

    public void ListenForKeyDown() {
        var input = Input.GetAxis("Mouse ScrollWheel");

        if (input > 0.0f) {
            if (IsometricCamera.StaticDistance < CameraZoom.farDistance) {
                if (IsometricCamera.StaticDistance + input > CameraZoom.farDistance) {
                    IsometricCamera.StaticDistance = CameraZoom.farDistance;
                    slider.value = CameraZoom.farDistance;
                }
                else {
                    IsometricCamera.StaticDistance += input;
                    slider.value += input;
                }
            }
        }
        else if (input < 0.0f) {
            if (IsometricCamera.StaticDistance > CameraZoom.nearDistance) {
                if (IsometricCamera.StaticDistance + input < CameraZoom.nearDistance) {
                    IsometricCamera.StaticDistance = CameraZoom.nearDistance;
                    slider.value = CameraZoom.nearDistance;
                }
                else {
                    IsometricCamera.StaticDistance += input;
                    slider.value += input;
                }
            }
        }
    }

   public void ZoomOut() {
        if (IsometricCamera.StaticDistance < CameraZoom.farDistance) {
            IsometricCamera.StaticDistance += 1;
            slider.value += 1;
        }
    }

    public void ZoomIn() {
        if (IsometricCamera.StaticDistance > CameraZoom.nearDistance) {
            IsometricCamera.StaticDistance -= 1;
            slider.value -= 1;
        }
    }

    public void ValueChangeCheck() {
        IsometricCamera.StaticDistance = slider.value;
    }
}

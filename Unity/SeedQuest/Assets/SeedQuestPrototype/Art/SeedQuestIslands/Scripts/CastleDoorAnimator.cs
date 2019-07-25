using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class CastleDoorAnimator : MonoBehaviour {

    void Update () {
        clickOn();
    }

    public void clickOn()
    {
        Camera c = Camera.main;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit hit;
            Ray ray = new Ray(c.transform.position, c.transform.forward);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();
                if (hitThisInteractable)
                {
                    Debug.Log("Hit: " + transform.name);
                    GetComponent<Animator>().SetTrigger("OpenDoor");
                }

            }
        }
    }
}

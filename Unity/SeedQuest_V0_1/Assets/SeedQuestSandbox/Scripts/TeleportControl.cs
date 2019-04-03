using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControl : MonoBehaviour {

    public int locationIndex;

    private void Update() {
        OnClickControl();
    }

    public void OnClickControl() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();
                if (hitThisInteractable) {
                    TeleportTo();
                }
            }
        }
    }

    public void TeleportTo() {
        Debug.Log("Teleport to " + locationIndex);

        int index = locationIndex;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        TeleportLocation location = TeleportSystem.Locations[index];
        player.transform.position = location.teleportPad.transform.position + location.positionOffset;
        player.transform.eulerAngles = location.rotation;
    } 
} 
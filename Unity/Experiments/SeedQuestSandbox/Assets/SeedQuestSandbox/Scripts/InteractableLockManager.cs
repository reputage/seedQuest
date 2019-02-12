using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLockManager : MonoBehaviour {

    private bool locked = false;

    void OnTriggerEnter(Collider enterObject)
    {
        locked = true;
        FirstPersonCamera.SetFreeze = locked;
    }

    private void Update()
    {
        Unlock();
    }

    private void Unlock()
    {
        if (locked && Input.GetKeyDown(KeyCode.U))
        {
            locked = false;
            FirstPersonCamera.SetFreeze = locked;
        }
    }
}

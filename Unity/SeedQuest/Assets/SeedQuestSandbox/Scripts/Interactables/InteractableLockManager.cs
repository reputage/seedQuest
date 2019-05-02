using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLockManager : MonoBehaviour {

    public GameObject lockIcon;
    private bool locked = false;
    private AudioSource click;

    void OnTriggerEnter(Collider enterObject)
    {
        lockIcon.SetActive(true);
        click = enterObject.GetComponent<AudioSource>();
        click.Play();
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
            lockIcon.SetActive(false);
            click.Play();
            locked = false;
            FirstPersonCamera.SetFreeze = locked;
        }
    }
}

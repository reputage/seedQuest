using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip onHover;
    public AudioClip onClick;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnMouseEnter()
    {
        //Debug.Log("Playing sound for mouseOver");
        audioSource.PlayOneShot(onHover);
    }

    public void OnMouseClick()
    {
        audioSource.PlayOneShot(onClick);
    }
}
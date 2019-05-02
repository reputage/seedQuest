using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    public bool loop = false;
    [HideInInspector]
    public AudioSource source;
}
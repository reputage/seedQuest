using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable Data")]
public class InteractableData : ScriptableObject
{
    public List<Interactable> interactables = new List<Interactable>();

}

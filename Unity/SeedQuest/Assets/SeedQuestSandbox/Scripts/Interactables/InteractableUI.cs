using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractableUI { 
        public string name = "";
        public Vector3 positionOffset = new Vector3(0, 0, 0);
    }
}
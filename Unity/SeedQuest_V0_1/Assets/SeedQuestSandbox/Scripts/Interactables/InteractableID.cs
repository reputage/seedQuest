using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableID {
    
    public int siteID;
    public int spotID;
    public int actionID;

    public InteractableID(int _siteId, int _spotId, int _actionID) {
        siteID = _siteId;
        spotID = _spotId;
        actionID = _actionID;
    }
}

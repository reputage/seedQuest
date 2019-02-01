using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeleportLocation {
    public GameObject teleportPad;
    public Vector3 positionOffset;
    public Vector3 rotation;
}

public class TeleportSystem : MonoBehaviour {

    private static TeleportSystem instance = null;
    public static TeleportSystem Instance {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<TeleportSystem>();
            return instance;
        }
    }

    public TeleportLocation[] locations;
    public static TeleportLocation[] Locations {
       get { return Instance.locations; }
    }

    private void OnDrawGizmos()
    {
        foreach(TeleportLocation location in locations)
        {
            Gizmos.color = Color.red;
            Vector3 startPos = location.teleportPad.transform.position;
            Gizmos.DrawWireSphere(startPos, 0.2f);
        }
    }
} 
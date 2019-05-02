using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    /// <summary> Reference to Player Transform </summary>
    static private Transform player;

    private void Awake() {
        player = GameObject.FindWithTag("Player").transform;
    }

    /// <summary> Transform attached to Player gameobject </summary>
    static public Transform Transform {
        get { return player;  }
        set { player = value; }
    }

    /// <summary> Position of player in world space </summary>
    static public Vector3 Position {
        get { return player.position; }
    }

    /// <summary> Forward direction of player </summary>
    static public Vector3 Forward
    {
        get { return player.forward; }
    }
}
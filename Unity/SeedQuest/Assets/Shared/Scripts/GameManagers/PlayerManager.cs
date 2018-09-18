using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    /// <summary> Reference to singleton instance of PlayerManager </summary>
    static private PlayerManager instance = null;
    /// <summary> Reference to Player Transform </summary>
    private Transform player;

    void Awake() {
        instance = this;
    }

    /// <summary> Transform attached to Player gameobject </summary>
    static public Transform Transform {
        get { return instance.player; }
    }

    /// <summary> Position of player in world space </summary>
    static public Vector3 Position {
        get { return instance.player.position; }
    }

    /// <summary> Sets transform attached to Player gameobject </summary>
    static public void SetPlayer(Transform transform) {
        instance.player = transform;
    }
}
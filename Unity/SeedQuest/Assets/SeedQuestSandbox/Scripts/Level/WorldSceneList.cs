using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/WorldSceneList")]
public class WorldSceneList : ScriptableObject {
    public WorldSceneProps[] worldScenes = new WorldSceneProps[16];
}
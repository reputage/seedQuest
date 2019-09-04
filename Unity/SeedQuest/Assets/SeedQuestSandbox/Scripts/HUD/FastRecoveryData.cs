using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FastRecovery/FastRecoveryData")]
public class FastRecoveryData : ScriptableObject
{
    public Sprite source;
    public float scale;
    public float xOffset;
    public float yOffset;
    public float rotation;
    public Sprite interactableIcon;
    public Sprite interactableIconSelected;
    public bool useInteractableUIPositions;
    public bool useRenderTexture;
    public float renderCameraHeight;
    public bool restrictViewport;
}
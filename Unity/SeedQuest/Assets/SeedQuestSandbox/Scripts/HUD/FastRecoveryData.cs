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
    public bool restrictViewport;
    public bool useRenderTexture;
    public float renderCameraHeight;
    public float renderCameraOffsetX;
    public float renderCameraOffsetZ;
    public float renderRotationMultiplier;
}
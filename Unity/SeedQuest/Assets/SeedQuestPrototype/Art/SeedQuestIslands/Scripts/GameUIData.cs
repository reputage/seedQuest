using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Skin/UI")]
public class GameUIData : ScriptableObject {

    // Button Skins
    public Sprite[] buttonSprites;
    public RuntimeAnimatorController buttonAnimationCtrl;

    /// Box - Check/Unchecked states
    public Sprite uncheckedBox;
    public Sprite checkedBox;

    // Font
    public Font textFont;
    public TMP_FontAsset font;
    public Material fontMaterial;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialDataItem
{
    public string headerText;
    public string bodyText;
    public int bodyTextHeight;
    public int bodyTextYOffset;
    public bool useSecondBodyText;
    public string secondBodyText;
    public int secondBodyTextHeight;
    public int secondBodyTextYOffset;
    public int popupHeight;
    public bool useArrow;
    public Vector3 arrowPosition;
    public Vector3 arrowRotation;
    public bool useImage;
    public Sprite image;
    public Vector2 imageSize;
    public Vector3 imageLocalPosition;
}

[CreateAssetMenu(menuName = "Tutorial/TutorialData")]
public class TutorialData : ScriptableObject
{
    public List<TutorialDataItem> tutorialData;

}

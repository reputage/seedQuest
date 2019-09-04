using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class EncodeSeed_ScenePreview : MonoBehaviour
{
    public Image preview;
    public Image shade;
    public TextMeshProUGUI text;

    private void Awake() {
        preview = GetComponentsInChildren<Image>(true)[1];
        shade = GetComponentsInChildren<Image>(true)[2];
        text = GetComponentsInChildren<TextMeshProUGUI>(true)[1];
    }
}

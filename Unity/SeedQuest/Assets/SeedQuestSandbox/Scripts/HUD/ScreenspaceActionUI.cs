using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeedQuest.HUD
{
    public class ScreenspaceActionUI : MonoBehaviour
    {

        public string actionName;
        public TMPro.TextMeshProUGUI textObject;
        public Canvas canvasScreen;
        public RectTransform canvasRect;
        Camera c;

        private void Awake()
        {
            textObject = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            canvasScreen = gameObject.GetComponentInChildren<Canvas>();
            canvasRect = canvasScreen.GetComponent<RectTransform>();
            canvasScreen.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScreen.worldCamera = c;
            c = Camera.main;
            canvasScreen.gameObject.SetActive(false);
        }

        public void setAction(string inputText, Vector2 position, Vector2 offset)
        {
            activateCanvas();
            setText(inputText);
            setPosition(position, offset);
        }

        public void deactivate()
        {
            //deactivateCanvas();
            deactivateText();
        }

        public void activateCanvas()
        {
            canvasScreen.gameObject.SetActive(true);
        }

        public void deactivateCanvas()
        {
            canvasScreen.gameObject.SetActive(true);
        }

        public void deactivateText()
        {
            textObject.text = "";
        }

        public void setPosition(Vector2 position, Vector2 offset)
        {
            Vector2 relativePos = new Vector2(
                ((position.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((position.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            relativePos += offset;

            textObject.GetComponent<RectTransform>().anchoredPosition = relativePos;
        }

        public void setText(string inputText)
        {
            textObject.text = inputText;
        }

    }
}



using System.Collections;
using System.Collections.Generic;
using SeedQuest.Interactables;
using UnityEngine;
using UnityEngine.UI;

namespace SeedQuest.HUD
{
    public class MinimapUI : MonoBehaviour
    {
        public Sprite source;
        public float rotation;
        public float mapZoom;
        public float xScale;
        public float yScale;
        public float playerXOffset;
        public float playerYOffset;

        private GameObject player;
        private Image mapContainer;
        private Image map;
        private Image playerIcon;
        private Image pinIcon;
        private Image overlay;
        private Image largeMap;
        private bool active;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Image[] images = gameObject.GetComponentsInChildren<Image>();
            mapContainer = images[1];
            map = images[2];
            playerIcon = images[3];
            pinIcon = images[4];
            //overlay = images[5];
            //largeMap = images[6];
            //overlay.gameObject.SetActive(false);

            map.sprite = source;
            //largeMap.sprite = source;
            //map.transform.eulerAngles = new Vector3(0, 0, -rotation);
            map.rectTransform.sizeDelta = source.bounds.size * mapZoom;
            //largeMap.rectTransform.sizeDelta = new Vector2(980 / source.bounds.size.y * source.bounds.size.x, 980);
            //map.transform.localPosition = new Vector3(0, playerYOffset, 0);
            mapContainer.transform.eulerAngles = new Vector3(0, 0, rotation);
            if (GameManager.Mode != GameMode.Rehearsal)
            {
                pinIcon.gameObject.SetActive(false);
            }
            active = false;
        }

        private void Update()
        {
            playerIcon.transform.eulerAngles = new Vector3(0, -180, player.transform.eulerAngles.y-rotation);
            map.transform.localPosition = new Vector3(-player.transform.localPosition.x * xScale + playerXOffset, -player.transform.localPosition.z * yScale + playerYOffset, 0);
            if (GameManager.Mode == GameMode.Rehearsal)
            {
                pinIcon.transform.localPosition = new Vector3((InteractablePath.NextInteractable.LookAtPosition.x - player.transform.localPosition.x) * xScale, (InteractablePath.NextInteractable.LookAtPosition.z - player.transform.localPosition.z) * yScale, 0);
            }

           else if (GameManager.Mode == GameMode.Recall || GameManager.Mode == GameMode.Sandbox)
            {
                pinIcon.gameObject.SetActive(false);
            }

            //ListenForKeyDown();
        }

        /*private void ListenForKeyDown()
        {
            if (InputManager.GetKeyDown(KeyCode.M) && !active)
            {
                overlay.gameObject.SetActive(true);
                active = true;
            }

            else if (InputManager.GetKeyDown(KeyCode.M) && active)
            {
                overlay.gameObject.SetActive(false);
                active = false;
            }
        }*/

        // Legacy Minimap Code
        /*public GameObject lightObjects;

        private Light[] lights;

        private void Start()
        {
            lights = FindObjectsOfType(typeof(Light)) as Light[];
        }

        void OnPreCull()
        {
            //foreach (Light light in lights)
            //{
                //light.enabled = false;
            //}

            foreach (Transform child in lightObjects.transform)
            {
                child.gameObject.SetActive(false);
            }

        }

        void OnPostRender()
        {
            //foreach (Light light in lights)
            //{
                //light.enabled = false;
            //}

            foreach (Transform child in lightObjects.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        void LateUpdate()
        {
            Vector3 cameraPosition = player.position;
            cameraPosition.y = transform.position.y;
            transform.position = cameraPosition;

            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }*/
    }
}

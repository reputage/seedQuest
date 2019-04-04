using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.HUD
{
    public class MinimapUI : MonoBehaviour
    {
        public Transform player;
        public GameObject lightObjects;

        private Light[] lights;

        private void Start()
        {
            lights = FindObjectsOfType(typeof(Light)) as Light[];
        }

        void OnPreCull()
        {
            /*foreach (Light light in lights)
            {
                light.enabled = false;
            }*/

            foreach (Transform child in lightObjects.transform)
            {
                child.gameObject.SetActive(false);
            }

        }

        void OnPostRender()
        {
            /*foreach (Light light in lights)
            {
                light.enabled = true;
            }*/

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
        }
    }
}

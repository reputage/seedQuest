using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Animation {

    public class Rotate : MonoBehaviour
    {
        public Vector3 rotateDir = new Vector3(0, 0, 1);
        public float speed = 60.0f;
        private Vector3 rotate = Vector3.zero;

        void Update() {
            rotate += rotateDir * Time.deltaTime * speed;
            GetComponent<RectTransform>().rotation = Quaternion.Euler(rotate);
        }
    }
}
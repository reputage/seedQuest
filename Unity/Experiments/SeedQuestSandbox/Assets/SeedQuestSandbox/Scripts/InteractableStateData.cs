using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Interactables
{
    [System.Serializable]
    public class InteractableState
    {
        public string actionName = "";

        public GameObject prefab = null;
        public Vector3 positionOffset = Vector3.zero;

        public Mesh mesh = null;
        public Material material = null;
        public int materialIndex = 0;
        public Vector2 uvOffset = Vector2.zero;
        public RuntimeAnimatorController animatorController;
        public string soundEffectName = "";

        public void enterState(Interactable item) {
            // Remove Children GameObjects to Remove Assocaited Prefabs
            foreach (Transform child in item.transform)
                if (child.tag != "Static")
                    GameObject.Destroy(child.gameObject);

            // Update with Prefab
            if (prefab != null) {
                GameObject _prefab = GameObject.Instantiate(prefab, item.transform);
                _prefab.transform.position += positionOffset;

                if (item.GetComponent<MeshFilter>() != null)
                    item.GetComponent<MeshFilter>().sharedMesh = null;
            }
            else {
                item.transform.position += positionOffset;
            }

            // Update Mesh
            if (mesh != null)
                item.GetComponent<MeshFilter>().sharedMesh = mesh;

            // Update Material and Material Index
            if (material != null)  {
                Material[] materials = item.GetComponent<Renderer>().materials;
                materials[materialIndex] = material;
                item.GetComponent<Renderer>().materials = materials;
            }

            // Set UVOffset for current material/material index
            if (item.GetComponent<Renderer>() != null)
                item.GetComponent<Renderer>().materials[materialIndex].SetTextureOffset("_MainTex", uvOffset);

            // Update animation controller
            if (animatorController != null)
                item.GetComponentInChildren<Animator>().runtimeAnimatorController = animatorController;

            // Play sound effect clip
            if (soundEffectName != "")
                AudioManager.Play(soundEffectName);

        }

        public void doAction(Interactable item)
        {

        }

        public void exitState(Interactable item)
        {

        }
    }

    [CreateAssetMenu(menuName = "Interactables/InteractableStateData")]
    public class InteractableStateData : ScriptableObject
    {
        public string interactableName;
        public Vector3 labelPosOffset;
        public List<InteractableState> states;
        public GameObject effect;

        public string getStateName(int index)
        {
            return states[index].actionName;
        }
    }
}
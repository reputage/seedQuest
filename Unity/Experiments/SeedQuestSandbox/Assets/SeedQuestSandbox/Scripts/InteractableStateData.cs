using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableState {
    public string actionName = "";

    public GameObject prefab = null;
    public Vector3 positionOffset = Vector3.zero;

    public Mesh mesh = null;
    public Material material = null;
    public int materialIndex = 0;
    public Vector2 uvOffset = Vector2.zero;
    public RuntimeAnimatorController animatorController;
    public string soundEffectName = "";

    public void enterState(Interactable item)
    {
        if(prefab != null) {
            GameObject _prefab = GameObject.Instantiate(prefab, item.transform);
            _prefab.transform.position += positionOffset;
            item.GetComponent<MeshFilter>().sharedMesh = null;
        }
        else {
            item.transform.position += positionOffset;
            foreach (Transform child in item.transform)
                GameObject.Destroy(child.gameObject);
        }

        if (mesh != null)
            item.GetComponent<MeshFilter>().sharedMesh = mesh;

        if (material != null) {
            Material[] materials = item.GetComponent<Renderer>().materials;
            materials[materialIndex] = material;
            item.GetComponent<Renderer>().materials = materials;
        } 

        item.GetComponent<Renderer>().materials[materialIndex].SetTextureOffset("_MainTex", uvOffset);

        if (animatorController != null)
            item.GetComponentInChildren<Animator>().runtimeAnimatorController = animatorController;

        if (soundEffectName != "")
            AudioManager.Play(soundEffectName);

    }

    public void doAction(Interactable item) {
        
    }

    public void exitState(Interactable item) {
        
    }
}

[CreateAssetMenu(menuName = "Interactables/InteractableStateData")]
public class InteractableStateData : ScriptableObject {
    public string interactableName;
    public Vector3 labelPosOffset;
    public List<InteractableState> states;
    public GameObject effect;
    
    public string getStateName(int index) {
        return states[index].actionName;
    }
}

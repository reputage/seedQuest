using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InteractableDebug : MonoBehaviour {

    public Interactable[] list;
    public string seed;

    private void Update()
    {
        SeedConverter converter = new SeedConverter();
        seed = converter.InteractableListToSeed(list);
    }

    private void OnDrawGizmos()
    {
        Interactable[] list = InteractableManager.InteractableList;
        foreach(Interactable item in list)
            Gizmos.DrawWireSphere(item.transform.position, item.interactDistance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableManager : MonoBehaviour {

    public float interactDistance = 2.0f;
    public GameObject actionSpotIcon;
    private Interactable activeItem = null;

    static public Interactable[] InteractableList
    {
        get { return findAllInteractables(); }
    }

    static public Interactable ActiveItem
    {
        get { return instance.activeItem; }
        set { instance.activeItem = value; }
    }

    static public InteractableManager __instance = null;
    static public InteractableManager instance {
        get {
            if (__instance == null)
                __instance = GameObject.FindObjectOfType<InteractableManager>();
            return __instance;
        }
    }

    static Interactable[] findAllInteractables() {
        return GameObject.FindObjectsOfType<Interactable>();
    }

    static void findNearInteractables() {
        Interactable[] list = FindObjectsOfType<Interactable>();

        foreach (Interactable item in list)
        {
            Vector3 playerPosition = PlayerManager.GetPlayer().position;
            float dist = (item.transform.position - playerPosition).magnitude;
            if (dist < instance.interactDistance)
                doNearInteractable(true);
            else
                doNearInteractable(false);
        }  
    }

    static void doNearInteractable(bool isNear) {
        
    }

    static ParticleSystem getEffect() {
        ParticleSystem effect;

        InteractableStateData data = instance.activeItem.stateData;
        if(data == null)
            effect = EffectsManager.createEffect(instance.activeItem.transform);
        else if(data.effect == null)
            effect = EffectsManager.createEffect(instance.activeItem.transform);
        else 
            effect = EffectsManager.createEffect(instance.activeItem.transform, data.effect);

        return effect;
    }

    static public void showActions(Interactable interactable) {
        InteractableManager.ActiveItem = interactable;
        GameManager.State = GameState.Interact;
        InteractableUI.show(interactable);
    }

    static public void doInteractableAction(int actionIndex) {
        Debug.Log("Action " + actionIndex);

        ParticleSystem effect = getEffect();
        effect.Play();

        InteractableManager.ActiveItem.doAction(actionIndex);
        InteractableManager.ActiveItem = null;

        GameManager.State = GameManager.PrevState;
    } 
}

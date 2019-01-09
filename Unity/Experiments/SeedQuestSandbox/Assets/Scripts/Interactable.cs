using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//[ExecuteInEditMode] 
[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour {

    public InteractableStateData stateData = null;
    public InteractableID ID;
    public int currentStateID = 0;
    public int InteractableUI = 0; 

    [HideInInspector]
    public float interactDistance = 2.0f;
    private bool isOnHover = false;
    private GameObject actionSpot = null;
    private Button[] actionButtons;

    // Use this for initialization
    void Start () {
        InitInteractable();
	}
	
	void Update () {
        if (actionSpot != null)
        {
            BillboardInteractable();
            HoverOnInteractable();
            clickOnInteractable();
        }
	} 

    public void InitInteractable() {
        Vector3 positionOffset = Vector3.zero;
        if (stateData != null)
            positionOffset = stateData.labelPosOffset;
        Vector3 position = transform.position + positionOffset;
        Quaternion rotate = Quaternion.identity;
        actionSpot = Instantiate(InteractableManager.Instance.actionSpotIcons[InteractableUI], position, rotate, InteractableManager.Instance.transform);
        var text = actionSpot.GetComponentInChildren<TMPro.TextMeshProUGUI>(); 

        if (stateData != null)
            text.text = stateData.interactableName;
        else
            text.text = "Error: Require StateData";
        
        actionButtons = actionSpot.GetComponentsInChildren<Button>();
        if(InteractableUI == 0) {
            actionButtons[0].onClick.AddListener(onClickNext);
            actionButtons[1].onClick.AddListener(onClickPrev);
        }
        else if(InteractableUI == 1) { 
            for (int i = 0; i < 4; i++) {
                var actionText = actionButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                actionText.text = stateData.getStateName(i);
            }

            actionButtons[0].onClick.AddListener(delegate { doAction(0); });
            actionButtons[1].onClick.AddListener(delegate { doAction(1); });
            actionButtons[2].onClick.AddListener(delegate { doAction(2); });
            actionButtons[3].onClick.AddListener(delegate { doAction(3); });
        }

        hideActions();

        // Create Triggers for HoverEvents
        foreach (Button button in actionButtons) {
            setButtonHoverEvents(button);
        }
    } 

    // Create TriggerEntry and add callback
    public void setButtonHoverEvents(Button button) {
        EventTrigger trigger = button.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { GameManager.State = GameState.Interact; });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { GameManager.State = GameState.Sandbox; });
        trigger.triggers.Add(exit); 
    }

    public void hideActions() {
        foreach (Button button in actionButtons) {
            button.transform.gameObject.SetActive(false);
        }
    }

    public void showActions() {
        foreach (Button button in actionButtons) {
            button.transform.gameObject.SetActive(true);
        }
    }
    
    int mod(int x, int m) {
        return (x % m + m) % m;
    }

    public void onClickNext() {
        currentStateID = mod(currentStateID + 1, 4);
        Debug.Log("Next " + currentStateID);
        InteractableState state = stateData.states[currentStateID];
        state.enterState(this);
    }

    public void onClickPrev() {
        currentStateID = mod(currentStateID - 1, 4);
        Debug.Log("Prev " + currentStateID);
        InteractableState state = stateData.states[currentStateID];
        state.enterState(this);
    } 

    public void doAction(int actionIndex) {
        Debug.Log("DO Action - " + actionIndex);
        InteractableState state = stateData.states[actionIndex];
        state.enterState(this);

    }

    public void BillboardInteractable() {
        Vector3 targetPosition = Camera.main.transform.position;
        Vector3 interactablePosition = actionSpot.transform.position;
        Vector3 lookAtDir = targetPosition - interactablePosition;
        //lookAtDir.y = 0;

        Quaternion rotate = Quaternion.LookRotation(lookAtDir);
        actionSpot.transform.rotation = rotate;
    }

    private bool PlayerIsNear() {
        Vector3 playerPosition = PlayerCtrl.PlayerTransform.position;
        float dist = (transform.position - playerPosition).magnitude;
        if (dist < interactDistance)
            return true;
        else
            return false;
    }

    public void HoverOnInteractable() {
        
        Camera c = Camera.main;
        RaycastHit hit;
        //Ray ray = new Ray(c.transform.position, c.transform.forward);
        Ray ray = c.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();
            if (hitThisInteractable) {

                /*
                if (!isOnHover)
                    toggleHighlight(true);
                isOnHover = true;

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                    InteractableManager.showActions(this);
                    */
            }
            else {
                /*
                if (isOnHover)
                    toggleHighlight(false);
                isOnHover = false;
                */
            }
        }

    }

    public void clickOnInteractable() {

        if (PauseManager.isPaused == true)
            return; 

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("BtnDown Interactable"); 

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("Ray Interactable " + hit.transform.name + " " + transform.name);

                bool hitThisInteractable = hit.transform.GetInstanceID() == transform.GetInstanceID();
                if (hitThisInteractable) {
                    //InteractableManager.showActions(this);
                    showActions();
                    Debug.Log("Click Interactable"); 
                }
            }
        }

    }

    public void startEffect() {
        ParticleSystem effect = InteractableManager.getEffect();
        effect.Play();
    }

    public void toggleHighlight(bool highlight) {
        Renderer rend = transform.GetComponent<Renderer>();
        if (rend == null)
            return;

        Shader shaderDefault = Shader.Find("Standard");
        Shader shader = Shader.Find("Custom/Outline + Rim");

        Material[] materials = rend.materials;
        for (int i = 0; i < materials.Length; i++) {

            if (highlight)
                rend.materials[i].shader = shader;
            else
                rend.materials[i].shader = shaderDefault;
        }
    }

    /*
    public void HighlightPathTarget() {
        if (GameManager.State != GameState.Rehearsal)
            return;
        
        if (PathManager.PathTarget == this)
            toggleHighlight(true);
        else if(!isOnHover)
            toggleHighlight(false);
    }
    */

    public string getInteractableName()
    {
        if (stateData == null)
            return "Interactable Name";
        else
            return this.stateData.interactableName;
    }

    public string Name
    {
        get { return getInteractableName(); }
    }

    public string getStateName(int index)
    {
        if (stateData == null)
            return "Action #" + index;
        else
            return this.stateData.getStateName(index);
    }

    public string RehersalActionName
    {
        get { return getStateName(ID.actionID); }
    }

    public int getStateCount()
    {
        if (stateData == null)
            return 0;
        else
            return this.stateData.states.Count;
    }

} 
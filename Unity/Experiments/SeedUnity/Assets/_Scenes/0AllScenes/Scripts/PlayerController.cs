using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float rotationSpeed;
	public float yMin, yMax;
	public float gravity;
	public float jumpSpeed;

    public GameObject griddler;
    public GameObject actionMenu;
    public GameObject resultMenu;
    public GameObject actionOperator;
    public GameObject otherItem;
    public GameObject logDisplay;

	public Text countText;
	public Text winText;

	//private Rigidbody rb;
	public int count; 

	private Vector3 moveDirection = Vector3.zero;

    private bool nearItem = false;
    private bool logVisible = false;
    private bool pauseActive = false;
    private int logID = 0;
    private int logCool = 0;
    private int pauseCool = 0;

    //private bool singleEntry = true;

    public GameObject playerLog;

	void Start () 
	{
        Debug.Log(Time.timeScale);
		//rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
        logDisplay.GetComponentInChildren<Text>().text = "";
	}

	void Update() 
	{
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded && pauseActive == false) 
		{
			transform.Rotate (0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);

			moveDirection = new Vector3(Input.GetAxis("Strafe"), 0, Input.GetAxis("Vertical"));
			//moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;

		}
        /*
        if (Input.GetAxis("Fire1") > 0){
            griddler.GetComponent<GridCreator>().gridIron(0);
            griddler.GetComponent<GridCreator>().Proceed();
        }
        if (Input.GetAxis("Fire2") > 0) {
            griddler.GetComponent<GridCreator>().gridIron(1);
        }
        */

        //Input.GetAxis("FG")

        if(nearItem == true) {

            if (Input.GetAxis("FG") > 0 && pauseActive == false){
                logID = otherItem.GetComponent<item>().itemID;
                //Debug.Log(logID);

                actionOperator.GetComponent<actionOperator>().deactivateSpot();
                playerLog.GetComponent<playerLog>().actionLogger(logID);
                otherItem.GetComponent<item>().takeItem();

                logDisplay.GetComponentInChildren<Text>().text += "Item taken: " + otherItem.GetComponent<item>().itemName + "\nItem ID: " + otherItem.GetComponent<item>().itemID + "\n";

                otherItem.SetActive(false);
                nearItem = false;
            }
        }

        if (logCool == 0 && Input.GetAxis("FG") < 0){

            logCool += 20;
            if (logVisible == false)
            {
                logVisible = true;
                logDisplay.SetActive(true);
            }
            else{
                logVisible = false;
                logDisplay.SetActive(false);
            }
        }

        //float cancelVal = Input.GetAxis("Cancel");
        Debug.Log(Input.GetAxis("Cancel"));

        if (pauseCool == 0 && Input.GetAxis("Cancel") > 0)
        {
            //cancelVal = 0;
            pauseCool += 10;

            if (pauseActive == false)
            {
                activatePause();
            }
            else
            {
                deactivatePause();
            }
        }

        if (logCool > 0){
            logCool -= 1;
        }

        if(pauseCool > 0){
            pauseCool -= 1;
        }

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}

    public void activatePause()
    {
        pauseActive = true;
        actionOperator.GetComponent<actionOperator>().activatePause();
        moveDirection *= 0;
        Time.timeScale = 0;
    }

    public void deactivatePause()
    {
        pauseActive = false;
        actionOperator.GetComponent<actionOperator>().deactivatePause();
        Time.timeScale = 1;
    }

    public void undoAction()
    {
       //in progress 
    }

    public void quitGame()
    {
        Application.Quit();
    }

	/*
	// Tank controls
	void Update () 
	{
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
	}
	*/


	/*
	// Movement, buggy when used with rigidbody physics
	void FixedUpdate () 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.velocity = (movement * speed);

	}
	*/

	void OnTriggerEnter(Collider other)
	{

        if (other.gameObject.CompareTag ("Pick Up")) 
		{
			other.gameObject.SetActive (false);
			count += 1;
			SetCountText ();
		}

        if (other.gameObject.CompareTag("ActionSpot"))
        {
            Debug.Log("Action spot entered");
            //count += 1;
            SetCountText();
            other.GetComponent<actionSpot>().playerAlert();
            actionOperator.GetComponent<actionOperator>().activateSpot();
            nearItem = true;
            otherItem = other.gameObject.GetComponent<actionSpot>().item;
            //ItemController itemController = other.GetComponent<item.
                             
            //To do:
            // Store the action in log, including map index, ID of spot, and ID of action

        }

        if (other.gameObject.CompareTag("Entrance"))
        {
            Debug.Log("Entrance entered");

            //To do:
            // Have menu pop-up prompting button press to enter
            // Remove menu on exit
            // Get the index of the entrance
            //other.gameObject.GetComponent<>();
            // Change scene on button press
            // UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        }
	}

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("ActionSpot"))
        {
            other.GetComponent<actionSpot>().playerClear();
            actionOperator.GetComponent<actionOperator>().deactivateSpot();
            Debug.Log("Action spot exited");

            nearItem = false;

        }
    }

	public void SetCountText ()
	{
		countText.text = "Actions: " + count.ToString ();
		/* 
		 if (count >= 12) 
		{
			winText.text = "Way too many actions taken!";
		}
		*/
	}
}
	 
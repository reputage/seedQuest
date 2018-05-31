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

	public Text countText;
	public Text winText;

	//private Rigidbody rb;
	public int count; 

	private Vector3 moveDirection = Vector3.zero;
    private bool singleEntry = true;

    public GameObject playerLog;

	void Start () 
	{
		//rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void Update() 
	{
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) 
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
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
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
            singleEntry = false;
			other.gameObject.SetActive (false);
			count += 1;
			SetCountText ();
		}

        if (other.gameObject.CompareTag("ActionSpot"))
        {
            singleEntry = false;
            Debug.Log("Action spot entered");
            //count += 1;
            SetCountText();
            //actionMenu.gameObject.SetActive(true);
            actionOperator.GetComponent<actionOperator>().activateMenu();

            //To do:
            // Have menu pop-up for player actions at the spot
            // Remove menu on exit
            // Store the action in log, including index and ID
            //other.gameObject.GetComponent<>();
            // Close menu on button press

        }

        if (other.gameObject.CompareTag("Entrance"))
        {
            singleEntry = false;
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
            actionOperator.GetComponent<actionOperator>().deactivateMenu();
            Debug.Log("Action spot exited");

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
	 
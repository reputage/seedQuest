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

	public Text countText;
	public Text winText;

	//private Rigidbody rb;
	private int count; 
	private Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
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
	// Add force controls, for the ball version of player
	void FixedUpdate () 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

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
	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
		if (count >= 12) 
		{
			winText.text = "You Win!";
		}
	}
}
	 
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed = 2f;
	public float sensitivity = 2f;
	public float gravity = 20.0f;
	public Text console;

	CharacterController controller;

	public GameObject playerCamera;

	private float rotationX, rotationY;
	private Vector3 moveDirection = Vector3.zero;
	private int nbKeys = 0;

	// Use this for initialization
	void Start () {

		// Retrieve the CharacterController in order to make it move
		controller = GetComponent<CharacterController> ();
		RefreshConsole ();

	}
	
	// Update is called once per frame
	void Update () {
		//Retrieve keys input and make the player move
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

		// Retrieve mouse inputs
		rotationX = Input.GetAxis ("Mouse X") * sensitivity;
		rotationY -= Input.GetAxis ("Mouse Y") * sensitivity;
		rotationY = Mathf.Clamp (rotationY, -80, 80);

		//Rotate the player on the X axis
		transform.Rotate(0, rotationX, 0);
		//Move the camera on the Y axis
		playerCamera.transform.localRotation = Quaternion.Euler (rotationY, 0, 0);

	}

	void OnTriggerEnter(Collider other) {
		//Player encouters a key 
		if(other.gameObject.CompareTag("Key")) {
			nbKeys++;
			RefreshConsole ();
			if (nbKeys == 3) {
				//End of the game: open the exit
				GameObject maze = GameObject.FindGameObjectWithTag ("Maze");
				maze.SendMessage ("OpenExit");
			}
		}

		//Player encouters an enemy 
		if (other.gameObject.CompareTag ("Enemy")) {
			//Player dies: respawn at the start area
			transform.position = new Vector3 (-4.0f, 2.3f, -4.0f);
		}
	}

	void RefreshConsole(){
		if (nbKeys == 3) {
			console.text = "Get Out!";
		} else {
			console.text = nbKeys + "/3 keys";
		}

	}
		
		
}

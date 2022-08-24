using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour {

	public float speed = 2f;
	public float sensitivity = 2f;
	CharacterController player;

	//public GameObject eyes;

	float moveFB;
	float moveLR;

	float rotX;
	float rotY;
	float vertVelocity;

	public float jumpDist = 5f;
	public int jumpTimes;

	public float playerHeight = 2f;

	private bool isCrouching;



	// Use this for initialization
	void Start () {

		player = GetComponent<CharacterController> ();

	}

	// Update is called once per frame
	void Update () {

		moveFB = Input.GetAxis ("Vertical") * speed;
		moveLR = Input.GetAxis ("Horizontal") * speed;

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY -= Input.GetAxis ("Mouse Y") * sensitivity;

		rotY = Mathf.Clamp (rotY, -60f, 60f);

		Vector3 movement = new Vector3 (moveLR, vertVelocity, moveFB);
		transform.Rotate (0, rotX, 0);
		//eyes.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
		//eyes.transform.Rotate (-rotY, 0, 0);

		movement = transform.rotation * movement;
		player.Move (movement * Time.deltaTime);

		if (player.isGrounded == true) {
			jumpTimes = 0;
		}

		if (jumpTimes < 2) {
			if (Input.GetButtonDown ("Jump")) {
				vertVelocity += jumpDist;
				jumpTimes += 1;
			}
		}

		if (Input.GetButtonDown ("Fire2")) {
			if (isCrouching == false) {
				player.height = playerHeight / 2;
				isCrouching = true;
			} else {
				player.height = playerHeight;
				isCrouching = false;
			}
		}



	}

	void FixedUpdate(){
		if (player.isGrounded == false) {
			vertVelocity += Physics.gravity.y * Time.deltaTime;
		} else {
			vertVelocity = 0f;
		}
	}
}
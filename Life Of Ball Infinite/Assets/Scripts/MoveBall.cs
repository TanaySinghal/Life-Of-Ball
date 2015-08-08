using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {

	private float mySpeed, moveHorizontal, moveVertical, horizontalWeight = 10f, verticalWeight = 1f;
	private Transform cameraGO;
	private bool jump, isMobile;

	public static bool isFalling;
	public static float moveMaxSpeed = 8f;
	//Values are: 200, 8, 8000
	public float moveForce, jumpMaxSpeed, jumpForce;

	Rigidbody myRigidbody;
	BallManager BM;

	void Start() {
		mySpeed = 0f;
		myRigidbody = GetComponent<Rigidbody>();
		//cameraGO = transform.FindChild("FollowCamera");
		cameraGO = GameObject.FindGameObjectWithTag("MainCamera").transform;

		isMobile = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
		BM = GetComponent<BallManager>();
	}

	void Update () {
		if (isMobile) {
			moveHorizontal = Mathf.Clamp(Input.acceleration.x,-1,1);
			moveVertical = Mathf.Clamp(-Input.acceleration.z,-1,1);
		}
		else {
			moveHorizontal = Input.GetAxis("Horizontal");
			moveVertical = Input.GetAxis("Vertical");
		}

		jump = Input.GetMouseButton(0) || Input.GetKey("space");
	}

	void FixedUpdate() {
		//get camera angle
		Quaternion rotation = Quaternion.Euler(0, cameraGO.eulerAngles.y, 0); 

		mySpeed = myRigidbody.velocity.sqrMagnitude;
		//Don't allow ball to go past full speed
		if(mySpeed > moveMaxSpeed*moveMaxSpeed && !isFalling) {
			moveVertical = 0f;
			myRigidbody.AddForce(-myRigidbody.velocity);
		}
		else if(mySpeed > jumpMaxSpeed*jumpMaxSpeed && isFalling) {
			moveVertical = 0f;
			myRigidbody.AddForce(-myRigidbody.velocity);
		}

		//Controls are based on camera angle
		Vector3 direction = rotation * (new Vector3 (moveHorizontal*horizontalWeight, 0, moveVertical*verticalWeight));
		direction.Normalize ();
		//Forward force
		myRigidbody.AddForce(direction*moveForce*Time.deltaTime);
		
		//Allow jump if we are on the ground
		if (jump && !isFalling) {
			BM.playTapSound();
			myRigidbody.AddForce(Vector3.up*jumpForce*Time.deltaTime);
			isFalling = true;
			jump = false;
		}
		else if(jump && isFalling) {
			jump = false;
		}
	}
}

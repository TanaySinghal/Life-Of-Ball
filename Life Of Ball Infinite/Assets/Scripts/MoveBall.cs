using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {

	private float moveHorizontal, moveVertical;
	private Transform cameraGO;
	private bool jump, isFalling;

	private float mySpeed;
	private float myMass;

	public float moveForce;
	public float moveMaxSpeed;
	public float jumpMaxSpeed;
	public float jumpForce;

	Rigidbody myRigidbody;

	float horizontalWeight = 10f;
	float verticalWeight = 1f;

	Vector3 startingBallPos;

	void Start() {
		//set starting ball pos.
		startingBallPos = transform.position;

		mySpeed = 0f;
		myRigidbody = GetComponent<Rigidbody>();
		//cameraGO = transform.FindChild("FollowCamera");
		cameraGO = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	// Update is called once per frame
	void Update () {
		moveHorizontal = Input.GetAxis("Horizontal");
		moveVertical = Input.GetAxis("Vertical");
		if(Input.GetKeyDown("space")) {
			jump =true;
		}
	}

	void FixedUpdate() {
		//get camera angle
		Quaternion rotation = Quaternion.Euler(0, cameraGO.eulerAngles.y, 0); 

		mySpeed = myRigidbody.velocity.magnitude;
		//Don't allow ball to go past full speed
		if(mySpeed > moveMaxSpeed && !isFalling) {
			moveVertical = 0f;
			myRigidbody.AddForce(-myRigidbody.velocity);
		}
		else if(mySpeed > jumpMaxSpeed && isFalling) {
			moveVertical = 0f;
			myRigidbody.AddForce(-myRigidbody.velocity);
		}

		Vector3 direction = rotation * (new Vector3 (moveHorizontal*horizontalWeight, 0, moveVertical*verticalWeight)); //controls are based on camera angle
		direction.Normalize ();
		//Forward force
		myRigidbody.AddForce(direction*moveForce*Time.deltaTime);
		
		//Jump force
		if (jump && !isFalling) {
			myRigidbody.AddForce(Vector3.up*jumpForce*Time.deltaTime);
			isFalling = true;
			jump = false;
		}
		else if(jump && isFalling) {
			jump = false;
		}
	}

	/*void OnGUI() {
		GUI.Button(new Rect(Screen.width/2-150, 60, 300, 30), "High score: " + highscore);
		GUI.Button(new Rect(Screen.width/2-150, 20, 300, 30), "Score: " + score);
	}*/
	
	public void Underwater() {
		myRigidbody.drag = 2f;
		myRigidbody.angularDrag = 1f;
	}
	
	public void AboveWater() {
		myRigidbody.drag = 0.1f;
		myRigidbody.angularDrag = 0.05f;
	}
	
	public void KillPlayer() {
		Application.LoadLevel("Finish");
	}

	void OnCollisionEnter(Collision c) {
		isFalling = false;
		if(c.collider.name == "OceanFloor") {
			KillPlayer();
		}
	}
}

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

	bool isMobile;

	AudioSource myAudioSource;

	public AudioClip bounceSound, tapSound;

	void Start() {
		//set starting ball pos.
		startingBallPos = transform.position;

		mySpeed = 0f;
		myRigidbody = GetComponent<Rigidbody>();
		//cameraGO = transform.FindChild("FollowCamera");
		cameraGO = GameObject.FindGameObjectWithTag("MainCamera").transform;

		isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

		myAudioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame

	int frameRate;

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

		//instead take the average over time..
		frameRate = (int)(1/Time.deltaTime);
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
		
		//Allow jump if we are on the ground
		if (jump && !isFalling) {
			playTapSound();
			myRigidbody.AddForce(Vector3.up*jumpForce*Time.deltaTime);
			isFalling = true;
			jump = false;
		}
		else if(jump && isFalling) {
			jump = false;
		}
	}

	void OnGUI() {
		GUI.Button(new Rect(Screen.width/2-150, 60, 300, 30), "Frame Rate: " + frameRate);
	}
	
	public void Underwater() {
		myRigidbody.drag = 2f;
		myRigidbody.angularDrag = 1f;
	}
	
	public void AboveWater() {
		myRigidbody.drag = 0f;
		myRigidbody.angularDrag = 0.05f;
	}
	
	public void KillPlayer() {
		Application.LoadLevel("Finish");
	}

	void OnCollisionEnter(Collision c) {
		isFalling = false;
		if(c.collider.tag == "Platform") {
			PlayBounceSound();
		}
		else if(c.collider.tag == "OceanFloor") {
			KillPlayer();
		}
	}
	
	void playTapSound() {
		myAudioSource.clip = tapSound;
		myAudioSource.volume = 1f;
		myAudioSource.Play();
	}
	
	void PlayBounceSound() {
		myAudioSource.clip = bounceSound;
		myAudioSource.volume = Mathf.Abs(myRigidbody.velocity.y/moveMaxSpeed);
		myAudioSource.Play();
	}
}

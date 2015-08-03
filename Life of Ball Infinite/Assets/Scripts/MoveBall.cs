using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {

	public Transform mainCam;
	GameObject leftwing, rightwing, pause, gameOverMenu, explosion, collectible;

	Vector3 movement;
	float speed, jumpForce, maxSpeed, moveHorizontal, moveVertical;
	static int looppassed = 0; //serves as a backup
	int loopcount, totalcount = 0;
	string pas;
	bool isFalling, completed, jump, re, controls = true;

	public static bool gameOver, incomplete, completelyover, ground;

	GUIStyle fps;

	public int maxHeight;

	public AudioClip bouncesound, tapsound;
	
	float xatstart = 0f, zatstart = 0f; //for calibration

	void Start () {
		loopcount = looppassed;
	
		ground = true;
		isFalling = false;
		completed = false;
		incomplete = false;
		gameOver = false;
		completelyover = false;
		re = true; //change this and more
		speed = 300f;
		jumpForce = 12000f;

		leftwing = GameObject.Find ("Left Wing");
		rightwing = GameObject.Find ("Right Wing");

		//do not allow higher CP
		//remove wings if loopcount is way less than loopcount

		pause = GameObject.Find ("Pause menu");
		gameOverMenu = GameObject.Find ("Game over");
		explosion = GameObject.Find ("Explosion effect");
		collectible = GameObject.FindWithTag("CollectibleWings");

		//Uncomment to start at a certain location (change CP.cp and loopcount)
		//CP.cp = 1;
		//loopcount = 16;*/

		pause.SetActive (false);

		if(gameOverMenu != null)
			gameOverMenu.SetActive (false);

		GetComponent<Renderer>().enabled = true;

		GameObject st;

		st = GameObject.Find ("CP" + CP.cp.ToString());

		if(st != null) {
			ToCheckPoint(st);
		}

		//Count loops
		totalcount = GameObject.FindGameObjectsWithTag("Hole").Length;
		Shader.WarmupAllShaders();
		GetComponent<Rigidbody>().isKinematic = false;
	}

	void ToCheckPoint(GameObject st) {
		//This method returns ball to its checkpoint
		Vector3 offset = transform.position - mainCam.transform.position;

		transform.position = st.transform.position;
		transform.position += Vector3.up * st.transform.lossyScale.y*2;
		mainCam.transform.position = transform.position - offset;
		mainCam.transform.RotateAround (transform.position, Vector3.up, st.transform.eulerAngles.y - 90);
	}

	void Update () {
		if (ground) {
			if(GetComponent<Rigidbody>().velocity.y < -5) maxSpeed = 100;
			else maxSpeed = 6;
		}

		if(!completed && !gameOver) {
			NormalControls();
		}
	}

	void FixedUpdate() {
		Quaternion rotation = Quaternion.Euler(0, mainCam.transform.eulerAngles.y, 0); //get camera angle

		//Don't allow ball to go past full speed
		if(GetComponent<Rigidbody>().velocity.magnitude < maxSpeed) {
			movement = rotation * (new Vector3 (moveHorizontal, 0, moveVertical)); //controls are based on camera angle
			movement.Normalize ();
		}
		else {
			movement = rotation * (new Vector3 (moveHorizontal, 0, 0)); //controls are based on camera angle
			GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity);
		}

		GetComponent<Rigidbody>().AddForce(movement*speed*Time.deltaTime);

		//Ground controls
		if (jump && isFalling == false && ground) {
			GetComponent<Rigidbody>().AddForce(Vector3.up*jumpForce*Time.deltaTime);
			isFalling = true;
			jump = false;
		}

		//Flying controls
		if (!ground) {
			GetComponent<Rigidbody>().AddForce(Vector3.up * GetComponent<Rigidbody>().mass * 3);
			if (jump && transform.position.y < maxHeight && controls) {
				playAnimation(leftwing, "leftwinganimation");
				playAnimation(rightwing, "rightwinganimation");
				GetComponent<Rigidbody>().AddForce(Vector3.up*jumpForce*Time.deltaTime);
			}
		}

	}
	
	
	public void Calibrate() {
		xatstart = Mathf.Clamp(Input.acceleration.x,-1,1);
		zatstart = Mathf.Clamp(-Input.acceleration.z,-1,1);
	}
	
	public void ResetLoops() {
		looppassed = 0;
		loopcount = 0;
	}
	
	public void SetLoop() {
		looppassed = loopcount;
	}
	
	void playAnimation(GameObject go, string animationName) {
		go.GetComponent<Animation>().Play(animationName);
	}

	void NormalControls() {
		//decide whether we are mobile or laptop... get input
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			moveHorizontal = Mathf.Clamp(Input.acceleration.x-xatstart,-1,1);
			moveVertical = Mathf.Clamp(-Input.acceleration.z-zatstart,-1,1);
		}
		else {
			moveHorizontal = Input.GetAxis("Horizontal");
			moveVertical = Input.GetAxis("Vertical");
		}

		if (!ground) {
			jump = Input.GetMouseButton(0) || Input.GetKey ("space") || (Input.GetAxis("Jump") > 0);
		}

		else if(ground) {
			if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) || Input.GetAxis("Jump") > 0 && !isFalling) {
				jump = true;
				playTapSound();
			}
		}
	}

	void explode() {
		Instantiate(explosion, transform.position, Quaternion.identity);
		GetComponent<Renderer>().enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
		StartCoroutine (exploded ());
	}

	IEnumerator exploded() {
		yield return new WaitForSeconds(2.0f);
		GetComponent<Renderer>().enabled = false;
		gameOver = true;
	}

	public void disableFlightMode() {
		ground = true;
		jumpForce = 12000;

		leftwing.GetComponent<Renderer>().enabled = false;
		rightwing.GetComponent<Renderer>().enabled = false;
		GetComponent<Rigidbody>().freezeRotation = false;
	}

	//only played when hitting water

	void OnCollisionEnter(Collision c) {
		isFalling = false;
		//scale on local axis
		
		if (c.gameObject.name == "Finish Line") {
			disableFlightMode();
			if(totalcount > 0 && loopcount == totalcount)
				completed = true;
			else if (totalcount == 0) //if there were no holes in the first place
				completed = true;
			else
				incomplete = true;
			GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
		}
		
		switch (c.gameObject.tag) {
		case "Platform":
			PlayBounceSound();
			if(!ground) 
				disableFlightMode();
			break;
		case "Terrain":
			gameOver = true;
			break;
		case "Hole":
			disableFlightMode();
			break;
		case "Platform Finish":
			disableFlightMode();
			break;
		default:
			break;
		}
	}


	void OnCollisionStay(Collision c) {
		if(completed) {
			GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			GetComponent<Rigidbody>().freezeRotation = true;
				completelyover = true;
		} 
		else {
			GetComponent<Rigidbody>().freezeRotation = false;
		}

		//free roam
		if(c.gameObject.name == "Mountain" && !ground) {
			disableFlightMode();
			collectible.SetActive(true);
			Vector3 temp = new Vector3(transform.position.x +- 2, 0, transform.position.z +- 2);
			temp.y = Terrain.activeTerrain.SampleHeight(temp) + 1;
			collectible.gameObject.transform.position = temp;
		}
	}

	void EnableFlightMode() {
		jumpForce = 600;
		maxSpeed = 20;
		GetComponent<Rigidbody>().freezeRotation = true;
		ground = false;
		leftwing.GetComponent<Renderer>().enabled = true;
		rightwing.GetComponent<Renderer>().enabled = true;
		isFalling = false;
	}

	void OnTriggerEnter (Collider other) {
		//enable flight mode
		if(other.gameObject.tag == "CollectibleWings") {
			other.gameObject.SetActive(false);
			EnableFlightMode();
		}
		else if(other.gameObject.name == "Troll collectible") {
			other.gameObject.SetActive(false);
			explode();
		}

		//if we hit in, then add to a variable.
		else if(other.gameObject.tag == "In") {
			//loopcount ++;
			string a = other.gameObject.name;
			string b = string.Empty;
			//if the hole is in the 10th digits...
			if(a.Length == 4) {
				b += a[a.Length-2]; //then store the first digit (prob 1 or 2 at most)
			}
			
			b += a[a.Length-1]; //then store the end digit

			//if loopcount is one less than the holecount or greater than it..
			int parsedB = int.Parse(b);
			if(loopcount >= parsedB - 1) { 
				loopcount = parsedB; //then loopcount is now holecount
			}
			else {
				disableFlightMode();
				Debug.Log("You missed a loop. Please go back." + loopcount + " " + b);
			}
		}
		
		if(other.gameObject.tag == "Disable") {
			disableFlightMode();
		}
	}

	void OnGUI() {
		fps = new GUIStyle("button");
		fps.fontSize = 45;

		if(!ground) {
			//GUI.Button(new Rect(Screen.width/2-150, 30, 300, 30), "Flight mode. Hold SPACE to rise.");
			if(transform.position.y > maxHeight)
				GUI.Button(new Rect(Screen.width/2-150, 60, 300, 30), "You have reached the maximum height");
		}
		//GUI.Button(new Rect(Screen.width/2-150, 30, 300, 30), "FPS: " + Mathf.Round(1/Time.deltaTime));

		pauseMenu ();

		if(gameOver)
			GameOver ();
		else if(gameOverMenu != null)
			gameOverMenu.SetActive(false);
		
		if(totalcount > 0)
			rect(Screen.width-150, 130, 150, 30, "Holes: " + loopcount + "/" + totalcount);
	}

	void pauseMenu() {
		//if we are in resume state
		if(re) {
			pas = "Menu";
			Time.timeScale = 1;
		}
		//otherwise if paused
		else {
			pas = "Resume";
			Time.timeScale = 0;
		}

		//if game is not over 
		if(!gameOver) {
			if(GUI.Button(new Rect(5, 5, Screen.width/6, Screen.height/6), pas, fps)) {
				pause.SetActive(re);
				re = !re;
			}
		}
	}

	void GameOver() {
		Time.timeScale = 0;
		gameOverMenu.SetActive(true);
	}


	void rect(float x, float y, float w, float h, string s) {
		GUI.Button(new Rect(x, y, w, h), s);
	}

	void playTapSound() {
		GetComponent<AudioSource>().clip = tapsound;
		GetComponent<AudioSource>().volume = 1f;
		GetComponent<AudioSource>().Play();
	}

	void PlayBounceSound() {
		GetComponent<AudioSource>().clip = bouncesound;
		GetComponent<AudioSource>().volume = Mathf.Abs(GetComponent<Rigidbody>().velocity.y/maxSpeed);
		GetComponent<AudioSource>().Play();
	}
}
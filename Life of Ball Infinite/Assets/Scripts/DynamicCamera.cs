using UnityEngine;
using System.Collections;

public class DynamicCamera : MonoBehaviour {
	
	public Transform target;
	public static int level;

	public static Vector3 offset;
	
	float moveHorizontal, moveVertical;
	
	string text1, text2;

	bool follow;
	// Use this for initialization
	void Start () {
		follow = true;
		offset = transform.position - target.position;
		camgameover = false;

		string levelname = Application.loadedLevelName;
		levelname = levelname.Remove(0,6);
		level = int.Parse(levelname);
	}


	void Update() {
		//gameOver = MoveBall.gameOver;
		if (MoveBall.completelyover) {
			CP.cp = 0;
			Application.LoadLevel("YouWon");
			follow = true;
		}
		else if(MoveBall.incomplete) {
			Application.LoadLevel("Incomplete");
			follow = true;
		}
		else follow = true;
	}

	void FixedUpdate(){
		if(follow)
			followCamera(0.2f, 2.4f);
	}
	
	public static bool camgameover = false;
	
	void followCamera(float damp, float distance) 
	{	
		damp = Mathf.Clamp(damp, 0.01f, 10f); // clamps damping factor
		Vector3 pCam = transform.position;
		Vector3 pTarget = target.position;
		Vector3 diff = pTarget - pCam;  // diff = difference between positions
		
		float dist = diff.magnitude;  // dist = distance between them
		if (Mathf.Abs(diff.y) < 1*distance){
			diff.y = 0;  // doesn't modify camera height unless angle > 45
		} 
		
		Vector3 tp = transform.position;
		if (dist>distance){ // if distance too big...
			diff *= 1-distance/dist; // diff = position error
			// move a FPS independent little step towards the ideal position
			tp.z = pCam.z + diff.z * Time.deltaTime/damp;
			tp.x = pCam.x + diff.x * Time.deltaTime/damp;
		}

		if (camgameover) {
			tp.y = 3.1f;
		}
		else
			tp.y = target.position.y + offset.y;

		transform.position = new Vector3(tp.x, tp.y, tp.z);
		
		transform.LookAt(pTarget);
	}
}

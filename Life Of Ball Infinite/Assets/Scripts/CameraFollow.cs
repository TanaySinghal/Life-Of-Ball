using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Transform target;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		target = GameObject.Find("Ball").transform;
		offset = transform.position - target.position;
	}

	void FixedUpdate() {
		followCamera(0.2f, 3f);
		//followBall();
	}

	void followBall() {
		transform.position = target.position + offset;
		//transform.RotateAround(target.position, Vector3.up, 1f);
		transform.LookAt(target.position);
	}
	
	void followCamera(float damp, float distance)  {	
		damp = Mathf.Clamp(damp, 0.01f, 10f); // don't allow damp to be too high

		Vector3 pCam = transform.position;
		Vector3 pTarget = target.position;
		Vector3 diff = pTarget - pCam;
		
		float dist = diff.magnitude;  // dist = distance between them

		if (Mathf.Abs(diff.y) < 1*distance){
			diff.y = 0;  // doesn't modify camera height unless angle > 45
		} 
		
		Vector3 tempPosition = transform.position;
		if (dist>distance){ // if distance too big...
			diff *= 1-distance/dist; // diff = position error
			// move a FPS independent little step towards the ideal position
			tempPosition.z = pCam.z + diff.z * Time.deltaTime/damp;
			tempPosition.x = pCam.x + diff.x * Time.deltaTime/damp;
		}

		//something wrong with this line...?
		tempPosition.y = target.position.y + offset.y;

		transform.position = tempPosition;
		transform.LookAt(pTarget);
	}
}

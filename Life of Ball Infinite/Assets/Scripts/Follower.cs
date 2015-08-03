using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

	public Transform player;
	//public Rigidbody target;
	public Transform mainCam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position;
		transform.eulerAngles = new Vector3(0, mainCam.transform.eulerAngles.y + 90, -70);
	}
}

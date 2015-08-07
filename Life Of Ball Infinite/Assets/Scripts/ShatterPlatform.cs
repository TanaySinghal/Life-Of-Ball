using UnityEngine;
using System.Collections;

public class ShatterPlatform : MonoBehaviour {
	
	public void KillPlatform() {
		//This sound is killed as soon as gameobject is destroyed.. put it somewhere else
		//GetComponent<AudioSource>().Play();

		Rigidbody myRB;
		myRB = gameObject.AddComponent<Rigidbody>();
		myRB.mass = 5f;
		//Remove all scripts
		ShatterTool shatterTool = GetComponent<ShatterTool>();
		shatterTool.Shatter(transform.position);
		Destroy(gameObject);
	}
}

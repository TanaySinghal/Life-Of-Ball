using UnityEngine;
using System.Collections;

public class ShatterPlatform : MonoBehaviour {

	public void KillPlatform() {
		if(GetComponent<Renderer>().isVisible) {
			Rigidbody myRB = gameObject.AddComponent<Rigidbody>();
			myRB.mass = 5f;
			//Remove all scripts
			ShatterTool shatterTool = GetComponent<ShatterTool>();
			shatterTool.Shatter(transform.position);
		}
		Destroy(gameObject);
	}

}

using UnityEngine;
using System.Collections;

public class DrownPlatform : MonoBehaviour {

	void Update() {
		if(transform.position.y <= -10) {
			Destroy(gameObject);
		}
	}

}

using UnityEngine;
using System.Collections;

public class DestroyPlatform : MonoBehaviour {

	//Keep reducing this as score gets higher and higher
	//Go from 3 to 1
	public static float platformTimer = 3f;

	void OnCollisionEnter(Collision c) {
		if(c.collider.tag == "Player" && !this.name.Contains("-0")) {
			//From scores 0 to 20, decrease platform time from 3 to 1
			platformTimer = 3f - Mathf.Clamp(GeneratePlatforms.largestPlatformID/10f, 0f, 2f);
			StartCoroutine(RemovePlatform());
		}
	}

	IEnumerator RemovePlatform() {
		yield return new WaitForSeconds(platformTimer);
		Destroy(this.gameObject);

	}
}

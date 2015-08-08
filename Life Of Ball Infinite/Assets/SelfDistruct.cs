using UnityEngine;
using System.Collections;

public class SelfDistruct : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		StartCoroutine(Die());
	}
	
	// Update is called once per frame
	IEnumerator Die() {
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}
}

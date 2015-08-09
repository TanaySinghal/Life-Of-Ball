using UnityEngine;
using System.Collections;

public class SelfDistruct : MonoBehaviour {

	void Awake () {
		StartCoroutine(Die());
	}

	//Self destruct
	IEnumerator Die() {
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}
}

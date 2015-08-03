using UnityEngine;
using System.Collections;

public class ExplosionFollower : MonoBehaviour {

	// Use this for initialization
	GameObject ball;
	bool notstart = false;

	void Awake() {
		transform.position = new Vector3 (0, 100, 0);
		StartCoroutine (on ());
	}

	void Start () {
		ball = GameObject.Find ("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		if(notstart)
			transform.position = ball.transform.position;
	}

	
	IEnumerator on()
	{
		yield return new WaitForSeconds(10.0f);
		notstart = true;
	}
}

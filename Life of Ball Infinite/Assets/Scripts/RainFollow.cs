using UnityEngine;
using System.Collections;

public class RainFollow : MonoBehaviour {

	GameObject mainCamera;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find ("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 = new Vector3(ball.transform.position.x, ball.transform.position.y + 10, ball.transform.position.z);
		transform.position = mainCamera.transform.position + 5*Vector3.up;
		if(MoveBall.gameOver) 
			GetComponent<AudioSource>().Stop();
	}
}

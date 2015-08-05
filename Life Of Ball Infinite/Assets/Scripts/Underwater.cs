using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour {

	// Update is called once per frame
	Transform cam;
	MoveBall moveBall;

	void Start() {
		moveBall = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveBall> ();
		cam = GameObject.Find("FollowCamera").transform;
		RenderSettings.fogColor = new Color(0.156f, 0.27f, 0.39f);
		RenderSettings.fogDensity = 0.1f;
		RenderSettings.fog = true;
	}

	void Update () {
		if(cam.position.y < transform.position.y ) {
			RenderSettings.fog = true;
			moveBall.Underwater();
		}
		else {
			RenderSettings.fog = false;
			moveBall.AboveWater();
		}
	}
}

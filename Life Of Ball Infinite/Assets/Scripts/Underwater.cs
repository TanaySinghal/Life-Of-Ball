using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour {

	// Update is called once per frame
	Transform cam;
	MoveBall moveBall;

	Transform oceanBeds;
	bool alreadyUnderwater;

	void Start() {
		moveBall = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveBall> ();
		cam = GameObject.Find("FollowCamera").transform;
		oceanBeds = GameObject.Find("OceanBeds").transform;

		RenderSettings.fogColor = new Color(0.156f, 0.27f, 0.39f);
		RenderSettings.fogDensity = 0.1f;
		RenderSettings.fog = false;
		alreadyUnderwater = true;
	}

	void Update () {
		if(cam.position.y < transform.position.y && !alreadyUnderwater) {
			Debug.Log("One time1");
			alreadyUnderwater = true;
			RenderSettings.fog = true;
			followBall();
			moveBall.Underwater();
		}
		else if(cam.position.y >= transform.position.y && alreadyUnderwater) {
			Debug.Log("One time2");
			alreadyUnderwater = false;
			RenderSettings.fog = false;
			moveBall.AboveWater();
		}
	}

	
	public void followBall () {
		oceanBeds.position = new Vector3(cam.position.x, 0, cam.position.z);
	}
}

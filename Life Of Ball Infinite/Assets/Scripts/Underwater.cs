﻿using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour {

	// Update is called once per frame
	Transform cam;
	BallManager ballScript;

	Transform oceanBeds;
	bool alreadyUnderwater;

	void Start() {
		ballScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BallManager> ();

		cam = GameObject.Find("FollowCamera").transform;
		oceanBeds = GameObject.Find("OceanBeds").transform;
		oceanBeds.gameObject.SetActive(false);

		RenderSettings.fogColor = new Color(0.156f, 0.27f, 0.39f);
		RenderSettings.fogDensity = 0.1f;
		RenderSettings.fog = true;
		alreadyUnderwater = true;
	}

	void Update () {
		if(cam.position.y < transform.position.y && !alreadyUnderwater) {
			alreadyUnderwater = true;
			RenderSettings.fog = true;
			followBall();
			ballScript.Underwater();
		}
		else if(cam.position.y >= transform.position.y && alreadyUnderwater) {
			alreadyUnderwater = false;
			RenderSettings.fog = false;
			ballScript.AboveWater();
		}
	}

	
	public void followBall () {
		oceanBeds.gameObject.SetActive(true);
		oceanBeds.position = new Vector3(cam.position.x, 0, cam.position.z);
	}
}

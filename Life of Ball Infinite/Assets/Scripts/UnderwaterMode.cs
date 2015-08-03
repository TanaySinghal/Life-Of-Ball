using UnityEngine;
using System.Collections;

public class UnderwaterMode : MonoBehaviour {

	// Use this for initialization
	GameObject cam, ball, rain;
	
	MoveBall mainScript;
	void Start () {
		cam = GameObject.Find ("Main Camera");
		ball = GameObject.Find ("Ball");
		RenderSettings.fogColor = new Color(0.156f, 0.27f, 0.39f);
		RenderSettings.fogDensity = 0.1f;
		RenderSettings.fog = true;
		mainScript = FindObjectOfType<MoveBall>();
		rain = GameObject.Find("Rain");
	}

	void Update() {
		if(cam.transform.position.y <= transform.position.y) {
			RenderSettings.fog = true;
			if(!cam.GetComponent<AudioSource>().isPlaying)
				cam.GetComponent<AudioSource>().Play();
		}
		else {
			RenderSettings.fog = false;
			if(cam.GetComponent<AudioSource>().isPlaying)
				cam.GetComponent<AudioSource>().Stop();
		}

		if(ball.transform.position.y <= transform.position.y) {
			if(!MoveBall.ground) {
				mainScript.disableFlightMode();
			}
			Underwater();
		}
		else {
			AboveWater();
		}
	}

	void Underwater() {
		ball.GetComponent<Rigidbody>().AddForce(Vector3.up * ball.GetComponent<Rigidbody>().mass * 5);
		ball.GetComponent<Rigidbody>().drag = 1f;
		if(rain != null)
			rain.SetActive(false);
	}

	void AboveWater() {
		ball.GetComponent<Rigidbody>().drag = 0.1f;
		if(rain != null)
			rain.SetActive(true);
	}

}

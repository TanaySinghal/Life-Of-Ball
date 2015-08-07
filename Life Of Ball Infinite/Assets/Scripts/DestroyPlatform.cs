using UnityEngine;
using System.Collections;

public class DestroyPlatform : MonoBehaviour {

	//Keep reducing this as score gets higher and higher
	//Go from 3 to 1
	public static float platformTimer = 3f;

	Color startColor;
	Color endColor;

	Renderer myRenderer;
	float t;

	bool beginChangingColor;
	//This will play every time a new platform is created.. perhaps more efficient to put it elsewhere
	void Awake() {
		startColor = new Color(28f/255f, 165f/255f, 52f/255f);
		endColor = new Color(1f, 0f, 0f);
		myRenderer = GetComponent<Renderer>();
		t = 0;
		beginChangingColor = false;
	}

	void OnCollisionEnter(Collision c) {
		if(c.collider.tag == "Player" && !this.name.Contains("-0")) {
			//From scores 0 to 15, decrease platform time from 3 to 1
			platformTimer = 3f - Mathf.Clamp(GeneratePlatforms.largestPlatformID/10f, 0f, 1.5f);
			beginChangingColor = true;
		}

	}

	void Update() {
		if(beginChangingColor) ChangeColor();
	}

	void ChangeColor() {
		myRenderer.material.color = Color.Lerp(startColor, endColor, t);
		if (t < platformTimer){ // while t below the end limit...
			// increment it at the desired rate every update:
			t += Time.deltaTime;
		}
		else {
			//If t > 1, destroy timer.
			ShatterPlatform SP = GetComponent<ShatterPlatform>();
			SP.KillPlatform();
		}

	}
}

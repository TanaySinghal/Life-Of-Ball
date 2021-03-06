﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
	///iPhone screen resolutions:
	///1536x2048 = 1.426
	///1080x1920 = 1.778
	///750x1334 = 1.779
	///768x1024 = 1.333
	///640x960 1.5

	///900 by 1440
	/// 
	///

	public static float multiplier;

	void Awake() {

		int screenH = Screen.height;
		Debug.Log ("Screen size: " + Screen.width + ", " + screenH);

		//Adjusting
		int sizeY = (int)(screenH*0.75f);
		int sizeX = (int)((400f/350f)*sizeY);
		multiplier = sizeY/350f;

		Text highscore = GameObject.Find("Highscore").GetComponent<Text>();
		highscore.text = PlayerPrefs.GetInt("highscore").ToString();
		highscore.fontSize = (int)(70f*multiplier);

		foreach(GameObject textGO in GameObject.FindGameObjectsWithTag("MenuText")) {
			Text menuText = textGO.GetComponent<Text>();
			menuText.fontSize = (int)(60f*multiplier);
		}

		VerticalLayoutGroup VLG = GetComponent<VerticalLayoutGroup>();
		VLG.padding.left = (int)(40f*multiplier);
		VLG.padding.right = (int)(40f*multiplier);
		VLG.padding.top = 0;
		VLG.padding.bottom = (int)(20f*multiplier);
		VLG.spacing = (int)(10f*multiplier);

		//Fix horizontal spacing
		if(transform.FindChild("Controls") != null) {
			HorizontalLayoutGroup HLG = transform.FindChild("Controls").GetComponent<HorizontalLayoutGroup>();
			HLG.spacing = (int)(10f*multiplier);
		}

		RectTransform RT = GetComponent<RectTransform>();

		//RT.sizeDelta = new Vector2((int)(400f*multiplier), (int)(350f*multiplier));
		RT.sizeDelta = new Vector2(sizeX, sizeY);
	}

	public void StartGame() {
		//Check if pressed here
		MoveBall.useTilt = RadioButtons.tilt;
		Application.LoadLevel("Loading");
	}

	public void Highscore() {
		//Http request friend's scores
	}

	public void ReturnToMainMenu() {
		Application.LoadLevel("Start");
	}

	public void QuitGame() {
		Application.Quit();
	}
}

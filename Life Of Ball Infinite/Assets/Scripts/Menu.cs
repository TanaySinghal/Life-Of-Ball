using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	
	
	//1536x2048 = 1.426
	//1080x1920 = 1.778
	//750x1334 = 1.779
	//768x1024 = 1.333
	//640x960 1.5

	//900 by 1440
	void Awake() {

		int screenH = Screen.height;
		Debug.Log ("Screen size: " + Screen.width + ", " + screenH);
		float multiplier = 1f;
		if(screenH > 600) {
			multiplier = 1.5f;
		}
		if(screenH > 1000) {
			multiplier = 2f;
		}
		if(screenH > 1400) {
			multiplier = 2.5f;
		}

		Text highscore = GameObject.Find("Highscore").GetComponent<Text>();
		if(highscore != null) {
			highscore.text = PlayerPrefs.GetInt("highscore").ToString();
		}
		highscore.fontSize = (int)(70f*multiplier);

		foreach(GameObject textGO in GameObject.FindGameObjectsWithTag("MenuText")) {
			Text menuText = textGO.GetComponent<Text>();
			menuText.fontSize = (int)(60f*multiplier);
		}

		VerticalLayoutGroup VLG = GameObject.Find("Panel").GetComponent<VerticalLayoutGroup>();
		VLG.padding.left = (int)(40f*multiplier);
		VLG.padding.right = (int)(40f*multiplier);
		VLG.padding.top = (int)(-20f*multiplier);
		VLG.padding.bottom = (int)(20f*multiplier);
		VLG.spacing = (int)(10f*multiplier);

		RectTransform RT = GameObject.Find("Panel").GetComponent<RectTransform>();
		//RT.sizeDelta.Set((int)(400*multiplier),(int)(300*multiplier));
		RT.sizeDelta = new Vector2((int)(400*multiplier), (int)(300*multiplier));
	}

	public void StartGame() {
		Application.LoadLevel("Loading");
	}

	public void Highscore() {
		//Http request friend's scores
	}

	public void Settings() {
	}

	public void ReturnToMainMenu() {
		Application.LoadLevel("Start");
	}
	
}

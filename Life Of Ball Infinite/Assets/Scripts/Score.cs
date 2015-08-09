using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	int fontSize;

	public static Text score;

	int highscore;

	Color color;
	//iphone and ipad screen res

	//1536x2048 = 1.426
	//1080x1920 = 1.778
	//750x1334 = 1.779
	//768x1024 = 1.333
	//640x960 1.5

	//MINE: 859 416
	void Awake () {
		Debug.Log("Canvas size: " + Screen.width + ", " + Screen.height);
		score = gameObject.GetComponent<Text>();

		highscore = PlayerPrefs.GetInt("highscore");
		score.text = "0";
		color = Color.black;
		InitializeText();
	}

	void InitializeText() {
		float screenHeight = Screen.height;

		fontSize = (int)Mathf.Round((1f/8f)*screenHeight);
		score.fontSize = fontSize;
	}

	
	public void StoreHighscore() { 
		//Convert string to int
		int actualScore = int.Parse(score.text);

		if(actualScore > highscore) {
			PlayerPrefs.SetInt("highscore", actualScore);
			color = new Color(0.75f, 0f, 0f);
			score.color = color;
		}
	}
}

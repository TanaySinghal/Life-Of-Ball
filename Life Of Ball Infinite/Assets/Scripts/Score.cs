using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	// Use this for initialization
	float maxScreen;

	float rectWidth;
	float rectHeight;
	int fontSize;
	float screenOffset;

	RectTransform rectTransform;
	public static Text score;

	int highscore;
	//iphone and ipad screen res

	//1536x2048
	//1080x1920
	//750x1334
	//768x1024
	//640x960


	//MINE: 859 416
	void Awake () {
		rectTransform = gameObject.GetComponent<RectTransform>();
		score = gameObject.GetComponent<Text>();

		highscore = PlayerPrefs.GetInt("highscore");
		score.text = "0";
		RightText();
	}

	void RightText() {
		fontSize = (int)Mathf.Round((50f/416f)*Screen.height);
		rectHeight = (60f/416f)*Screen.height;
		rectWidth = fontSize*5f;
		screenOffset = (10f/416f)*Screen.height;
		
		float posXFormula = -(rectWidth/2 + screenOffset);
		
		//Setting
		rectTransform.anchoredPosition = new Vector3(posXFormula,0,0);
		rectTransform.sizeDelta = new Vector3(rectWidth, rectHeight);
		score.fontSize = fontSize;
	}

	void CenterText() {
		fontSize = (int)Mathf.Round((50f/416f)*Screen.height);
		rectHeight = (60f/416f)*Screen.height;
		screenOffset = (10f/416f)*Screen.height;
		
		float posYFormula = -(fontSize/2+screenOffset);
		
		//Setting
		rectTransform.anchoredPosition = new Vector3(0,posYFormula,0);
		rectTransform.sizeDelta = new Vector3(Screen.width, rectHeight);
		score.fontSize = fontSize;
	}
	
	public void StoreHighscore() { 
		//Convert string to int
		int actualScore = int.Parse(score.text);

		if(actualScore > highscore) {
			PlayerPrefs.SetInt("highscore", actualScore);
		}
	}
}

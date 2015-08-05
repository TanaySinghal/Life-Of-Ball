using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {


	void Awake() {
		Text highscore = transform.FindChild("Highscore").GetComponent<Text>();
		if(highscore != null) {
			highscore.text = PlayerPrefs.GetInt("highscore").ToString();
		}
	}

	public void StartGame() {
		Application.LoadLevel("Demo");
	}

	public void Highscore() {
		//Http request friend's scores
	}

	public void QuitGame() {
		Application.Quit();
	}
	
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscores : MonoBehaviour {

	string playerName = "";
	int score = 0;

	public void SubmitScores() {
		playerName = GameObject.Find("Name").GetComponent<Text>().text;
		Debug.Log(name);

		score = Random.Range(0,100);
		WWWForm form = new WWWForm();
		form.AddField("user", playerName);
		form.AddField("score", score);
		
		WWW w = new WWW("http://localhost:8888/Scoreboard", form);
		Debug.Log("sent??");
	}
}

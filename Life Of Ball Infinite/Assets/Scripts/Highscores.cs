using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscores : MonoBehaviour {

	string name = "";
	int score = 0;

	public void SubmitScores() {
		name = GameObject.Find("Name").GetComponent<Text>().text;
		Debug.Log(name);

		score = Random.Range(0,100);
		WWWForm form = new WWWForm();
		form.AddField("user", name);
		form.AddField("score", score);
		
		WWW w = new WWW("http://localhost:8888/Scoreboard", form);
		Debug.Log("sent??");
	}
}

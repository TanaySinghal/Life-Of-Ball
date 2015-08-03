using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	MoveBall mainScript;

	void Start() { 
		//Warm up everything
		Shader.WarmupAllShaders();
		//end warming up

		mainScript = FindObjectOfType<MoveBall>();
	}

	//Later find the average position of the user's phone

	public void NewGame() {
		DynamicCamera.level = 1;
		string mylevel = DynamicCamera.level.ToString();
		//Application.LoadLevel ("Level_"+mylevel);
		
		StartCoroutine(LoadThem("Level_" +mylevel));
		Debug.Log("Loading complete");
		CP.cp = 0;
		
	}

	IEnumerator LoadThem(string l) {
		AsyncOperation async = Application.LoadLevelAsync(l);
		yield return async;
	}

	public void FreeRoam() {
		//Application.LoadLevel ("Free_Roam");
		StartCoroutine(LoadThem("Level_0"));
	}
	
	public void LoadGame() {
		Application.LoadLevel ("LoadLevel");

		DynamicCamera.level = 1;
		string mylevel = DynamicCamera.level.ToString();
		//Application.LoadLevel ("Level_"+mylevel);
		StartCoroutine(LoadThem("Level_" +mylevel));
		Debug.Log("Loading complete");
		CP.cp = 0;
		if(mainScript != null)
			mainScript.ResetLoops();
	}

	
	public void RestartLevel() {
		RestartCheckpoint ();
		CP.cp = 0;
		mainScript.ResetLoops();
	}

	public void Calibrate() {
		Debug.Log("Opening calibrating menu...");
		mainScript.Calibrate();
	}


	public void RestartCheckpoint() {
		string mylevel = DynamicCamera.level.ToString();
		if(mylevel == "0") {
			mylevel = "1";
		}
		Debug.Log ("My level: " + mylevel);
		Application.LoadLevel ("Level_"+mylevel);
	}
	
	public void NextLevel() {
		DynamicCamera.level ++;
		Debug.Log ("Next level: " + DynamicCamera.level);

		string mylevel = DynamicCamera.level.ToString();

		if (Application.CanStreamedLevelBeLoaded ("Level_" + mylevel)) {
			PlayerPrefs.SetInt("level", DynamicCamera.level);
			Debug.Log ("saved level: " + DynamicCamera.level);
			StartCoroutine(LoadThem("Level_" +mylevel));
		} else {
			DynamicCamera.level --;
			Debug.Log("Couldn't load level");
		}
	}

	public void GameSettings() {
	}
	
	public void QuitToMenu() {
		Application.LoadLevel ("MainMenu");
		CP.cp = 0;
	}

	public void QuitGame() {
		Application.Quit ();
	}
}

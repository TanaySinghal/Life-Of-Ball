using UnityEngine;
using System.Collections;
using System;

public class LoadLevel : MonoBehaviour {

	GUIStyle fb, fl;
	float offsetx, offsety;
	float sizex, sizey;
	float space = 30f;
	int maxlevel;
	
	
	void Start() {
		offsetx = 0;
		offsety = Screen.height/5;
		sizex = Screen.width / 3 - offsetx/3;
		sizey = Screen.height / 3 - offsety/3;

		maxlevel = PlayerPrefs.GetInt ("level", 1);

		Debug.Log ("loaded level: " + maxlevel);
	}

	void OnGUI() {
		fb = new GUIStyle (GUI.skin.button);
		fb.fontSize = Screen.height/16;

		
		if (GUI.Button (new Rect (space/2 ,space/2, sizex/2 - space, offsety - space), "Back", fb)) {
			Application.LoadLevel("MainMenu");
		}


		for(int i = 1; i <= 3; i ++) { //row
			for (int j = 1; j <= 3; j ++) { //col
				int level = 3*(j-1) + i;
				if(level > maxlevel) break;
				if(GUI.Button(new Rect(space/2+offsetx+sizex*(i-1), space/2+offsety+sizey*(j-1), sizex-space, sizey-space), "Level " + level, fb)) {
					Application.LoadLevel("Level_" + level);
				}
			}
		}

	}
}

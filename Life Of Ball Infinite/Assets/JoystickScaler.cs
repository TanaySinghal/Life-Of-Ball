using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickScaler : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		float multiplier = 1f;
		if(Screen.height > 600) {
			multiplier = 1.5f;
		}
		if(Screen.height > 1000) {
			multiplier = 2.5f;
		}
		if(Screen.height > 1400) {
			multiplier = 3.5f;
		}
		//Menu.multiplier
		RectTransform rt = GetComponent<RectTransform>();

		Vector2 offset = rt.anchoredPosition - rt.sizeDelta/2;
		offset *= multiplier;
		rt.sizeDelta *= multiplier;
		rt.anchoredPosition = rt.sizeDelta/2 + offset;

		Text jump = transform.GetComponentInChildren<Text>();
		
		Joystick joystick = GetComponent<Joystick>();
		if(joystick != null) {
			joystick.MovementRange = (int)(joystick.MovementRange*multiplier);
		}

		if(jump != null) {
			jump.fontSize = (int)(0.9*rt.sizeDelta.y);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

	// Use this for initialization
	void Awake() {
		Shader.WarmupAllShaders();
		Application.LoadLevel("Demo");
	}

}

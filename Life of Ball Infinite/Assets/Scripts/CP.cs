using UnityEngine;
using System.Collections;

public class CP : MonoBehaviour {

	public static int cp = 0;
	MoveBall mainScript;

	void Start() { 
		//Warm up everything
		Shader.WarmupAllShaders();
		//end warming up
		mainScript = FindObjectOfType<MoveBall>();
	}

	void OnCollisionEnter (Collision other) {

		//if other game object contains cp or has CP tag

		string a = other.gameObject.name;


		if(a.Contains("CP")) {
			string b = string.Empty;

			if(a.Length == 3) {
				b += a[a.Length-1];
			}

			else if (a.Length == 4) {
				b += a[a.Length-2];
				b += a[a.Length-1];
			}
			int x = int.Parse(b);

			//if we have made some progress
			if(x > cp) {
				cp = int.Parse(b);
				Debug.Log ("New checkpoint: " + cp);
				mainScript.SetLoop();
			}
		}
	}

}

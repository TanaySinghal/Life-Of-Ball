using UnityEngine;
using System.Collections;

public class ShatterPlatform : MonoBehaviour {

	GameObject brokenPlatform;
	Transform shatteredParent;
	GameObject soundPlayer;

	void Awake() {
		brokenPlatform = Resources.Load("BrokenPlatform") as GameObject;
		soundPlayer = Resources.Load("SoundPlayer") as GameObject;
		shatteredParent = GameObject.Find("ShatteredPlatforms").transform;
	}
	

	public void KillPlatform() {

		//This is to use the shattertool thing.. in whihc case you'd get rid of the if statement below

		/*if(GetComponent<Renderer>().isVisible) {
			Rigidbody myRB = gameObject.AddComponent<Rigidbody>();
			myRB.mass = 5f;
			//Remove all scripts
			ShatterTool shatterTool = GetComponent<ShatterTool>();
			shatterTool.Shatter(transform.position);
		}*/
		if(GetComponent<Renderer>().isVisible) {
			GameObject GO = Instantiate(brokenPlatform, transform.position, Quaternion.identity) as GameObject;
			GO.transform.parent = transform;
			GO.transform.localEulerAngles = new Vector3(30, 270, 270);
			GO.transform.parent = transform.parent;

			Rigidbody[] RBs = GO.GetComponentsInChildren<Rigidbody>();
			for(int i = 0; i < GO.transform.childCount; i ++) {
				GO.transform.GetChild(i).parent = shatteredParent;
			}
			Destroy(GO);
			foreach(Rigidbody RB in RBs) {
				//Add a slight downward direction force
				RB.AddExplosionForce(200f, transform.position, 1f, -0.1f);
			}
			
			GameObject SO = Instantiate(soundPlayer, transform.position, Quaternion.identity) as GameObject;
			SO.name = "SO: " + gameObject.name;
			SO.transform.parent = transform.parent;
			SO.tag = "SoundPlayer";

		}

		if(gameObject.tag.Contains("Bad")) {
			transform.name = "PoolBadPlatform";
		}
		else {
			transform.name = "PoolPlatform";
		}

		//Instead of this, add to pool
		transform.position = new Vector3(0, -20, 0);
		transform.parent = GameObject.Find("PooledPlatforms").transform;


	}

}

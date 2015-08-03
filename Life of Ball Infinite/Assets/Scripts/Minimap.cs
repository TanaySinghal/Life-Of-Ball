using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
		transform.position = pos;
	}
}

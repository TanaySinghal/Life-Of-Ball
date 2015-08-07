using UnityEngine;
using System.Collections;

public class DrownPlatform : MonoBehaviour {
	
	// Update is called once per frame

	Material material;

	Renderer myRD;

	void Awake() {
		material = Resources.Load("Materials/badGrass", typeof(Material)) as Material;
		myRD = GetComponent<Renderer>();

		Material[] tmp = myRD.materials;
		myRD.materials = new Material[]{tmp[0]};
		myRD.material = material;
		//myRD.material.color = Color.red;
	}

	void Update () {
		//somehow it doesn't work in start so update it is..
		myRD.material.color = Color.red;
		if(transform.position.y < 0) {
			Destroy(gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class DrownPlatform : MonoBehaviour {
	
	// Update is called once per frame
	
	//Uncomment to save created meshes
	/*
	void Awake() {

		Material material;

		Renderer myRD;
		material = Resources.Load("Materials/badGrass", typeof(Material)) as Material;
		myRD = GetComponent<Renderer>();

		Material[] tmp = myRD.materials;
		myRD.materials = new Material[]{tmp[0]};
		myRD.material = material;

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		//Creating GO for each
		int i = Random.Range(1,1000);
		AssetDatabase.CreateAsset(mesh, "Assets/savedMesh/ShatteredPieces"+i+".asset"); // update line2
		var prefab = EditorUtility.CreateEmptyPrefab("Assets/savedMesh/ShatteredPieces"+i+".prefab");
		
		EditorUtility.ReplacePrefab(gameObject, prefab);
		AssetDatabase.Refresh();
	}
	*/

	void Update() {
		if(transform.position.y <= -10) {
			Destroy(gameObject);
		}
	}
}

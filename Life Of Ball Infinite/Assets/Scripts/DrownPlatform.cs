using UnityEngine;
using System.Collections;
using UnityEditor;

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

		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		//Creating GO for each
		int i = Random.Range(1,1000);


		//Uncomment to save created meshes
		/*AssetDatabase.CreateAsset(mesh, "Assets/savedMesh/ShatteredPieces"+i+".asset"); // update line2
		var prefab = EditorUtility.CreateEmptyPrefab("Assets/savedMesh/ShatteredPieces"+i+".prefab");
		
		EditorUtility.ReplacePrefab(gameObject, prefab);
		AssetDatabase.Refresh();
		//Done..*/
	}

	void Update () {
		//somehow it doesn't work in start so update it is..
		myRD.material.color = Color.red;
		if(transform.position.y < 0) {
			Destroy(gameObject);
		}
	}
}

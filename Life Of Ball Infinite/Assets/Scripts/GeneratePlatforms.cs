using UnityEngine;
using System.Collections;

public class GeneratePlatforms : MonoBehaviour {

	// Use this for initialization
	GameObject platform;
	Transform platformParent;

	int largestPlatformID;
	int deletePlatformFromLargest = 4;

	float jumpDistance;
	
	float minJumpDistance = 3f;
	float maxJumpDistance = 8f;

	void Start () {
		largestPlatformID = 0;
		//keep increasing jumpDistance as the level goes on..
		jumpDistance = minJumpDistance;
		//jumpDistance = Mathf.Clamp(jumpDistance, minJumpDistance, maxJumpDistance);
		platformParent = GameObject.Find("Platforms").transform;
		platform = Resources.Load("Platform") as GameObject;
	}

	void OnCollisionEnter(Collision c) {
		//if we hit a platform
		string colliderName = c.collider.name;

		if(colliderName.Contains("Platform")) {

			string[] tempSplit = colliderName.Split('-');
			int platformID = int.Parse(tempSplit[1]);

			if(platformID == largestPlatformID) {
				CreatePlatform(c.collider.transform);
				DeletePlatform();
			}
		}
	}

	void CreatePlatform(Transform thisPlatform) {
		largestPlatformID ++;
		float zOffset = thisPlatform.GetComponent<Renderer>().bounds.size.z + jumpDistance;
		Vector3 offsetFromCenter = new Vector3(0f, 0f, zOffset);
		Vector3 nextPlatformPosition = thisPlatform.position + offsetFromCenter;
		GameObject nextPlatform = Instantiate(platform, nextPlatformPosition, thisPlatform.rotation) as GameObject;
		nextPlatform.transform.parent = platformParent;
		nextPlatform.name = "Platform-" + largestPlatformID;
	}

	void DeletePlatform() {
		int toBeDestroyedID = largestPlatformID-deletePlatformFromLargest;
		GameObject toBeDestroyed = GameObject.Find("Platform-" + toBeDestroyedID);
		Destroy(toBeDestroyed);
	}
}

using UnityEngine;
using System.Collections;

public class GeneratePlatforms : MonoBehaviour {

	// Use this for initialization
	GameObject platform;
	GameObject badPlatform;
	
	Transform platformParent;

	int largestPlatformID;
	int deletePlatformFromLargest = 4;

	float jumpDistance;
	
	float minJumpDistance = 3f;
	float maxJumpDistance = 8f;

	MoveBall moveBall;

	void Start () {
		moveBall = this.GetComponent<MoveBall>();
		largestPlatformID = 0;
		//keep increasing jumpDistance as the level goes on..
		jumpDistance = minJumpDistance;
		//jumpDistance = Mathf.Clamp(jumpDistance, minJumpDistance, maxJumpDistance);
		platformParent = GameObject.Find("Platforms").transform;

		//load platform
		platform = Resources.Load("Platform") as GameObject;
		badPlatform = Resources.Load("BadPlatform") as GameObject;

		Transform startingPlatform = GameObject.Find("Platform-0").transform;
	}

	void OnCollisionEnter(Collision c) {
		//if we hit a platform
		string colliderName = c.collider.name;

		if(colliderName.Contains("Platform") && c.collider.tag == "Platform") {

			string[] tempSplit = colliderName.Split('-');
			int platformID = int.Parse(tempSplit[1]);

			if(platformID == largestPlatformID) {
				MoveBall.score  = largestPlatformID;
				moveBall.StoreHighscore();
				CreatePlatform(c.collider.transform);
				DeletePlatform(largestPlatformID-deletePlatformFromLargest);
			}
		}
		else if(c.collider.tag == "BadPlatform") {
			Destroy(c.gameObject);
		}
	}

	int amountOfAnglesOnEachSide = 2;
	float maxDegree = 30f;


	//As the score gets higher and higher....
	//Increase maxAngle incrementally...
	//Increase difficulty more and more...
	//Increase badplatformprob

	void CreatePlatform(Transform thisPlatform) {
		//Update largest platform id
		largestPlatformID ++;

		float minAngle = 30f;
		float maxAngle = 50f;
		float difficulty = 10f;
		float badPlatformProb = 0;
		badPlatformProb = Mathf.Clamp(badPlatformProb, 0, 8);
		int badPlatformCount = 0;
		

		for(int i = -1; i <= 1; i += 1) {
			float degreeAngle = i*Random.Range(minAngle,maxAngle);
			float radAngle = degreeAngle*Mathf.Deg2Rad;

			//Calculate size of current platform and add jumpDistance
			float platformLength = thisPlatform.GetComponent<MeshFilter>().mesh.bounds.size.x;

			//Calculate total distance
			float platformDistance = platformLength + jumpDistance;

			//Offset by hypotenuse.. 
			//-thisplatform.right means transform.forward
			//-thisplatform.up means transform.right
			//Fix this in the future using children or something..
			Vector3 forwardOffset = (platformDistance*-thisPlatform.right)*Mathf.Cos(radAngle);
			Vector3 sideWaysOffset = (platformDistance*-thisPlatform.up)*Mathf.Sin(radAngle);

			Vector3 nextPlatformPosition = thisPlatform.position + forwardOffset + sideWaysOffset;

			float xRot = 0;
			float zRot = 0;
			//zRot = Random.Range(0f,1f) * difficulty;
			//xRot = Random.Range(-1f,1f) * difficulty;

			//TODO: Something seriously wrong with this eulerAngles.y ... changing X changes Y as well. it's strange
			Quaternion nextPlatformRotation =  Quaternion.Euler(new Vector3(-90, degreeAngle+thisPlatform.eulerAngles.y, 0));

			GameObject nextPlatform;
			//50% chance that we color
			if(Random.Range(1,11-badPlatformProb) == 1 && badPlatformCount < 2) {
				nextPlatform = Instantiate(badPlatform, nextPlatformPosition, nextPlatformRotation) as GameObject;
				nextPlatform.tag = "BadPlatform";
				nextPlatform.GetComponent<Renderer>().material.color = Color.red;
				badPlatformCount ++;
			}
			else {
				nextPlatform = Instantiate(platform, nextPlatformPosition, nextPlatformRotation) as GameObject;
			}
			nextPlatform.name = "Platform-" + largestPlatformID;
			nextPlatform.transform.parent = platformParent;
		}
	}

	float getRandomDegree(float maxAngle) {
		//can't sqrt negatives
		float randomNum = Random.Range(0,maxAngle);
		float randomDir = Mathf.Round(Random.value)-0.5f;
		if(randomDir < 0) randomDir = -1f;
		else randomDir = 1f;
		//Sqrt function increases the probability of more extreme angles
		return Mathf.Sqrt(randomNum)*Mathf.Sqrt(maxAngle)*randomDir;
	}

	void DeletePlatform(int platformID) {
		//Destroy all of them
		foreach(GameObject platform in GameObject.FindGameObjectsWithTag("Platform")) {
			if(platform.name == "Platform-" + platformID) {
				Destroy(platform);
			}
		}
		foreach(GameObject platform in GameObject.FindGameObjectsWithTag("BadPlatform")) {
			if(platform.name == "Platform-" + platformID) {
				Destroy(platform);
			}
		}
	}
}

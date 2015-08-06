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


	Score score;

	void Awake () {
		score = GameObject.Find("ScoreText").GetComponent<Score>();
		largestPlatformID = 0;
		//jumpDistance = Mathf.Clamp(jumpDistance, minJumpDistance, maxJumpDistance);
		platformParent = GameObject.Find("Platforms").transform;

		//load platform
		platform = Resources.Load("Platform") as GameObject;
		badPlatform = Resources.Load("BadPlatform") as GameObject;
	}

	void OnCollisionEnter(Collision c) {
		//if we hit a platform
		string colliderName = c.collider.name;

		if(colliderName.Contains("Platform") && c.collider.tag == "Platform") {

			string[] tempSplit = colliderName.Split('-');
			int platformID = int.Parse(tempSplit[1]);

			if(platformID == largestPlatformID) {
				//TODO: Get rid of moveball scores
				//MoveBall.score  = largestPlatformID;
				Score.score.text = largestPlatformID.ToString();
				score.StoreHighscore();
				CreatePlatform(c.collider.transform);
				DeletePlatform(largestPlatformID-deletePlatformFromLargest);
			}
		}
		else if(c.collider.tag == "BadPlatform") {
			Destroy(c.gameObject);
		}
	}

	float currentJumpDistance;

	void CreatePlatform(Transform thisPlatform) {
		//Update largest platform id
		largestPlatformID ++;

		float minAngle = 30f;
		int badPlatformCount = 0;

		//TODO: As score gets higher, max angle increases

		float maxAngle = 50f;
		//float difficulty = 10f;
		int badPlatformProb = 0; //change from 0 to 3

		//THIS IS ALREADY CHANGING BY LEVEL
		float maxJumpDistance = 7f;
		float minJumpDistance = 3f;

		//Max jump distance varies based on 

		badPlatformProb = Mathf.Clamp(largestPlatformID/5, 0, 3);
		maxJumpDistance = minJumpDistance + Mathf.Clamp(largestPlatformID/10f, 0, maxJumpDistance-minJumpDistance);

		badPlatformProb = Mathf.Clamp(badPlatformProb, 0, 8);
		badPlatformProb = 11-badPlatformProb;

		for(int i = -1; i <= 1; i += 1) {
			jumpDistance = Random.Range(minJumpDistance, maxJumpDistance);
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

			//zRot = Random.Range(0f,1f) * difficulty;
			//xRot = Random.Range(-1f,1f) * difficulty;

			//TODO: Something seriously wrong with this eulerAngles.y ... changing X changes Y as well. it's strange
			Quaternion nextPlatformRotation =  Quaternion.Euler(new Vector3(-90, degreeAngle+thisPlatform.eulerAngles.y, 0));

			GameObject nextPlatform;
			//50% chance that we color

			//badPlatformProb = 8;
			if(Random.Range(1,6-badPlatformProb) == 1 && badPlatformCount < 2) {
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

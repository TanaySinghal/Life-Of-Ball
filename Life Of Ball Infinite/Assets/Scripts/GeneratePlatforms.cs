using UnityEngine;
using System.Collections;

public class GeneratePlatforms : MonoBehaviour {

	Transform platformParent;

	//Being accessed from DestroyPlatform
	public static int largestPlatformID;

	int deletePlatformFromLargest = 1;


	Score score;

	void Awake () {
		score = GameObject.Find("ScoreText").GetComponent<Score>();
		largestPlatformID = 0;
		//jumpDistance = Mathf.Clamp(jumpDistance, minJumpDistance, maxJumpDistance);
		platformParent = GameObject.Find("Platforms").transform;

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
				CreatePlatform(c.collider.transform, largestPlatformID - deletePlatformFromLargest);
			}
		}
		else if(c.collider.tag == "BadPlatform") {
			c.gameObject.GetComponent<ShatterPlatform>().DestroyAndRecyclePlatform();
		}
	}

	float currentJumpDistance;
	

	void CreatePlatform(Transform thisPlatform, int deletePlatformID) {
		//Update largest platform id
		largestPlatformID ++;
		
		float platformLength = thisPlatform.GetComponent<MeshFilter>().mesh.bounds.size.x;

		//From score 0 to 15, maxAngle increases from 30 to 70
		float minAngle = 30f;
		float maxAngle = Mathf.Clamp(largestPlatformID*2, 0f, 30f) + minAngle;

		//From score 0 to 15, increase probablity of making a bad platform from 1/6 to 1/2
		int rangeBadPlatformProb = 3;
		int badPlatformProb = 6 - Mathf.Clamp(largestPlatformID/5, 0, rangeBadPlatformProb);

		//From score 0 to 15, increase maxJumpDistance from 3 to 6
		float minJumpDistance = 3f;
		float rangeJumpDistance = 3f;
		float maxJumpDistance = minJumpDistance + Mathf.Clamp(largestPlatformID/5f, 0, rangeJumpDistance);

		//Keep track of bad platforms
		int badPlatformCount = 0;
		for(int i = -1; i <= 1; i += 1) {
			//Initialize all random values based on difficulty

			//Make game unpredictable using random values
			int badPlatformLottery = Random.Range(1,badPlatformProb);
			float jumpDistance = Random.Range(minJumpDistance, maxJumpDistance);
			float degreeAngle = i*Random.Range(minAngle,maxAngle);

			//Convert from degrees to radians
			float radAngle = degreeAngle*Mathf.Deg2Rad;

			//Calculate size of current platform and add jumpDistance

			//Calculate total distance
			float platformDistance = platformLength + jumpDistance;

			//Offset must equal hypotenuse, hence the trigonometry
			//-thisplatform.right means transform.forward
			//-thisplatform.up means transform.right
			Vector3 forwardOffset = (platformDistance*-thisPlatform.right)*Mathf.Cos(radAngle);
			Vector3 sideWaysOffset = (platformDistance*-thisPlatform.up)*Mathf.Sin(radAngle);

			//Add up all the offsets and this platform position to create next platform
			Vector3 nextPlatformPosition = thisPlatform.position + forwardOffset + sideWaysOffset;

			Quaternion nextPlatformRotation =  Quaternion.Euler(new Vector3(-90, degreeAngle+thisPlatform.eulerAngles.y, 0));

			GameObject nextPlatform = null;

			//Check if we have won the loterry to create a bad platform
			if(badPlatformLottery == 1 && badPlatformCount < 2) {
				nextPlatform = RecyclePlatform(deletePlatformID, "BadPlatform");
				nextPlatform.tag = "BadPlatform";
				badPlatformCount ++;
			}
			else {
				nextPlatform = RecyclePlatform(deletePlatformID, "Platform");
				nextPlatform.tag = "Platform";
			}

			nextPlatform.transform.position = nextPlatformPosition;
			nextPlatform.transform.rotation = nextPlatformRotation;
			nextPlatform.name = "Platform-" + largestPlatformID;

			//This is just for organization purposes..
			nextPlatform.transform.parent = platformParent;
		}
	}

	GameObject RecyclePlatform(int platformID, string tag) {
		foreach(GameObject platform in GameObject.FindGameObjectsWithTag(tag)) {
			if(platform.name.Contains("-")) {
				string[] tempSplit = platform.name.Split('-');
				int tempID = int.Parse(tempSplit[1]);
				if(tempID <= platformID) {
					return platform;
				}
			}
			else if(platform.name.Contains("Pool")) {
				return platform;
			}
		}
		//If nothing works..
		return null;
	}
}

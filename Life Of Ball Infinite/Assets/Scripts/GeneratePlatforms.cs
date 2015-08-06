using UnityEngine;
using System.Collections;

public class GeneratePlatforms : MonoBehaviour {

	// Use this for initialization
	GameObject platform;
	GameObject badPlatform;
	
	Transform platformParent;

	//Being accessed from DestroyPlatform
	public static int largestPlatformID;

	int deletePlatformFromLargest = 4;


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
				score.OnScoreChange();
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

		//From score 0 to 15, maxAngle increases from 30 to 70
		float maxAngle = Mathf.Clamp(largestPlatformID*2, 0f, 30f) + 40f;

		//From score 0 to 15, increase probablity of making a bad platform from 1/6 to 1/2
		int rangeBadPlatformProb = 3;
		int badPlatformProb = 6 - Mathf.Clamp(largestPlatformID/5, 0, rangeBadPlatformProb);

		//From score 0 to 15, increase maxJumpDistance from 3 to 6
		float minJumpDistance = 3f;
		float rangeJumpDistance = 3f;
		float maxJumpDistance = minJumpDistance + Mathf.Clamp(largestPlatformID/5f, 0, rangeJumpDistance);

		for(int i = -1; i <= 1; i += 1) {
			//Initialize all random values based on difficulty

			//Likelyhood of creating a bad platform (1 means yes)
			int badPlatformLottery = Random.Range(1,badPlatformProb);

			//Gap between platforms
			float jumpDistance = Random.Range(minJumpDistance, maxJumpDistance);

			//Angle between current platform and next platform
			float degreeAngle = i*Random.Range(minAngle,maxAngle);

			//Convert from degrees to radians
			float radAngle = degreeAngle*Mathf.Deg2Rad;

			//Calculate size of current platform and add jumpDistance
			float platformLength = thisPlatform.GetComponent<MeshFilter>().mesh.bounds.size.x;

			//Calculate total distance
			float platformDistance = platformLength + jumpDistance;

			//Offset must equal hypotenuse.. 
			//-thisplatform.right means transform.forward
			//-thisplatform.up means transform.right
			//TODO: Something seriously wrong here because the above values are strange...
			Vector3 forwardOffset = (platformDistance*-thisPlatform.right)*Mathf.Cos(radAngle);
			Vector3 sideWaysOffset = (platformDistance*-thisPlatform.up)*Mathf.Sin(radAngle);

			//Add up all the offsets and this platform position to create next platform
			Vector3 nextPlatformPosition = thisPlatform.position + forwardOffset + sideWaysOffset;


			//TODO: Something seriously wrong with this eulerAngles.y ... changing X changes Y as well. it's strange
			Quaternion nextPlatformRotation =  Quaternion.Euler(new Vector3(-90, degreeAngle+thisPlatform.eulerAngles.y, 0));

			GameObject nextPlatform;

			//Check if we have won the loterry to create a bad platform
			//Make sure not all the next platforms are bad platforms
			//If these are true, create bad platform
			if(badPlatformLottery == 1 && badPlatformCount < 2) {
				nextPlatform = Instantiate(badPlatform, nextPlatformPosition, nextPlatformRotation) as GameObject;
				nextPlatform.tag = "BadPlatform";
				nextPlatform.GetComponent<Renderer>().material.color = Color.red;
				//keep track of how many bad platforms we have
				badPlatformCount ++;
			}
			else {
				//Otherwise just create a normal platform
				nextPlatform = Instantiate(platform, nextPlatformPosition, nextPlatformRotation) as GameObject;
			}

			nextPlatform.name = "Platform-" + largestPlatformID;

			//This is just for organization purposes..
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

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
				//DeletePlatform(largestPlatformID-deletePlatformFromLargest);
			}
		}
		else if(c.collider.tag == "BadPlatform") {
			c.gameObject.GetComponent<ShatterPlatform>().KillPlatform();
		}
	}

	float currentJumpDistance;


	//TODO: This method is wayyy too heavy..

	void CreatePlatform(Transform thisPlatform, int platformID) {
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

			//Likelyhood of creating a bad platform (1 means yes)
			int badPlatformLottery = Random.Range(1,badPlatformProb);
		
			//Gap between platforms
			float jumpDistance = Random.Range(minJumpDistance, maxJumpDistance);

			//Angle between current platform and next platform
			float degreeAngle = i*Random.Range(minAngle,maxAngle);

			//Convert from degrees to radians
			float radAngle = degreeAngle*Mathf.Deg2Rad;

			//Calculate size of current platform and add jumpDistance

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

			GameObject nextPlatform = null;

			//Check if we have won the loterry to create a bad platform
			//Make sure not all the next platforms are bad platforms
			//If these are true, create bad platform


			if(badPlatformLottery == 1 && badPlatformCount < 2) {
				foreach(GameObject platform in GameObject.FindGameObjectsWithTag("BadPlatform")) {
					if(platform.name.Contains("-")) {
						string[] tempSplit = platform.name.Split('-');
						int tempID = int.Parse(tempSplit[1]);
						if(tempID <= platformID) {
							nextPlatform = platform;
							break;
						}
					}
					else if(platform.name.Contains("Pool")) {
						nextPlatform = platform;
						break;
					}
				}

				nextPlatform.tag = "BadPlatform";

				badPlatformCount ++;
			}
			else {
				foreach(GameObject platform in GameObject.FindGameObjectsWithTag("Platform")) {
					if(platform.name.Contains("-")) {
						string[] tempSplit = platform.name.Split('-');
						int tempID = int.Parse(tempSplit[1]);
						if(tempID <= platformID) {
							nextPlatform = platform;
							break;
						}
					}
					else if(platform.name.Contains("Pool")) {
						nextPlatform = platform;
						break;
					}
				}
				nextPlatform.tag = "Platform";
			}

			//Reset so it doesn't get destroyed if it had already begun

			nextPlatform.transform.position = nextPlatformPosition;
			nextPlatform.transform.rotation = nextPlatformRotation;
			nextPlatform.name = "Platform-" + largestPlatformID;

			//This is just for organization purposes..
			nextPlatform.transform.parent = platformParent;
		}
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

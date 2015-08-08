using UnityEngine;
using System.Collections;

public class BallManager : MonoBehaviour {

	// Use this for initialization
	
	AudioSource myAudioSource;
	Rigidbody myRigidbody;
	
	public AudioClip bounceSound, tapSound, underwaterSound;

	void Start () {
		myAudioSource = GetComponent<AudioSource>();
		myRigidbody = GetComponent<Rigidbody>();
	
	}

	public void Underwater() {
		myRigidbody.drag = 2f;
		myRigidbody.angularDrag = 1f;
		myAudioSource.clip = underwaterSound;
		myAudioSource.Play ();
	}
	
	public void AboveWater() {
		myRigidbody.drag = 0f;
		myRigidbody.angularDrag = 0.05f;
	}
	
	public void KillPlayer() {
		Application.LoadLevel("Finish");
	}
	
	void OnCollisionEnter(Collision c) {
		MoveBall.isFalling = false;
		if(c.collider.tag == "Platform") {
			PlayBounceSound();
		}
		else if(c.collider.tag == "OceanFloor") {
			KillPlayer();
		}
	}
	
	public void playTapSound() {
		myAudioSource.clip = tapSound;
		myAudioSource.volume = 1f;
		myAudioSource.Play();
	}
	
	private void PlayBounceSound() {
		myAudioSource.clip = bounceSound;
		myAudioSource.volume = Mathf.Abs(myRigidbody.velocity.y/MoveBall.moveMaxSpeed);
		myAudioSource.Play();
	}
}

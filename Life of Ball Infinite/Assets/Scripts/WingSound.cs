using UnityEngine;
using System.Collections;

public class WingSound : MonoBehaviour {

	void PlayAudio(AudioClip aud) {
		GetComponent<AudioSource>().clip = aud;
		GetComponent<AudioSource>().Play();
	}
}
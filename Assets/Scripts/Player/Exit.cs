using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public AudioClip carStart;

	bool canExit;
	AudioSource sound;
	LevelManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.FindGameObjectWithTag ("Manager").GetComponent<LevelManager> ();
		sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		ActionContext.canLeave = canExit;
		if (canExit && Input.GetButton ("Action")) {
			sound.PlayOneShot (carStart);
			manager.beginFade (false);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			canExit = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			canExit = false;
		}
	}
}

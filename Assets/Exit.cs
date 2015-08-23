using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	bool canExit;

	LevelManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.FindGameObjectWithTag ("Manager").GetComponent<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		ActionContext.canLeave = canExit;
		if (canExit && Input.GetButton ("Action")) {
			manager.endLevel ();
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DumpBody : MonoBehaviour {

	int bodyCount;

	List<GameObject> inRange;

	// Use this for initialization
	void Start () {
		inRange = new List<GameObject> ();
	}
	
	public bool kill(GameObject human) {
		bool contains = inRange.Contains (human);
		if (contains) {
			inRange.Remove(human);
			bodyCount ++;
		}
		return contains;
	}

	void Update() {
		ActionContext.canDumpBody = inRange.Count > 0;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Body")) {
			inRange.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag ("Body") && inRange.Contains (other.gameObject)) {
			inRange.Remove(other.gameObject);
		}
	}
}

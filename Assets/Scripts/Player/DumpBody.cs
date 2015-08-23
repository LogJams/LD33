using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DumpBody : MonoBehaviour {

	public float killWeight = 0.10f; //how much each kill pleases the voices
	float silenceMod;
	public Slider slider;

	List<GameObject> inRange;

	// Use this for initialization
	void Start () {
		inRange = new List<GameObject> ();
		slider.value = 1;
		silenceMod = GameInfo.silencingModifier;
	}
	
	public bool kill(GameObject human) {
		bool contains = inRange.Contains (human);
		if (contains) {
			inRange.Remove(human);
			GameInfo.bodyCount ++;
			slider.value -= (killWeight * silenceMod);
			slider.value = Mathf.Max(0, slider.value);
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

using UnityEngine;
using System.Collections;

public class Behavior : MonoBehaviour {
	public Vision vision;

	// Use this for initialization
	void Start () {
		vision = this.gameObject.GetComponent<Vision> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (vision.inMain == true){
			Debug.Log("DETECTED");
		} else if (vision.inPeripheral == true) {
			Debug.Log("IN PERIPHERAL");
		}
	}
}
